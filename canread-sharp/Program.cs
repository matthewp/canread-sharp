using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using HtmlAgilityPack;

namespace canread_sharp
{
    class Program
    {
        static void Main(string[] args)
        {
            HtmlDocument doc = new HtmlDocument();
            doc.Load("somearticle.htm");
            Article a = GrabArticle(doc);
            string articleText = a.ToString();
        }

        static Article GrabArticle(HtmlDocument doc)
        {
            Article article = new Article();

            HtmlNode articleContent = doc.CreateElement("div");

            ParagraphParentCollection paragraphParents = new ParagraphParentCollection(new ParagraphCollection(doc.DocumentNode.SelectNodes("//p")));

            // Replace br tags with paragraph tags.
            doc.DocumentNode.InnerHtml = Regex.Replace(doc.DocumentNode.InnerHtml, @"<br/?>[ \r\n\s]*<br/?>", @"</p><p>");

            // TODO handle title.
            article.Title = doc.DocumentNode.SelectSingleNode("//title") == null ? null : doc.DocumentNode.SelectSingleNode("//title").InnerText;

            foreach (ParagraphParent parent in paragraphParents)
            {
                foreach (HtmlAttribute att in parent.Node.Attributes.AttributesWithName("class"))
                {
                    if (Regex.IsMatch(att.Name, @"/(comment|meta|footer|footnote)/"))
                        parent.Score -= 50;
                    else if (Regex.IsMatch(att.Name, @"/((^|\\s)(post|hentry|entry[-]?(content|text|body)?|article[-]?(content|text|body)?)(\\s|$))/"))
                        parent.Score += 25;
                    break;
                }

                foreach (HtmlAttribute att in parent.Node.Attributes.AttributesWithName("id"))
                {
                    if (Regex.IsMatch(att.Name, @"/(comment|meta|footer|footnote)/"))
                        parent.Score -= 50;
                    else if (Regex.IsMatch(att.Name, @"/^(post|hentry|entry[-]?(content|text|body)?|article[-]?(content|text|body)?)$/"))
                        parent.Score += 25;
                }

                foreach (Paragraph paragraph in parent.Paragraphs)
                {
                    if (paragraph.Node.InnerText.Length > 10)
                        parent.Score++;

                    parent.Score += GetCharCount(paragraph.Node);
                }
            }

            ParagraphParent winner = paragraphParents.OrderByDescending(a => a.Score).FirstOrDefault();

            // TODO cleanup.
            winner.Clean("style");
            winner.KillDivs();
            winner.KillBreaks();
            winner.Clean("form");
            winner.Clean("object");
            winner.Clean("table", 250);
            winner.Clean("h1");
            winner.Clean("h2");
            winner.Clean("iframe");
            winner.Clean("script");
            
            article.Content.DocumentNode.AppendChild(winner.Node);
            return article;
        }

        static int GetCharCount(HtmlNode node, char c = ',')
        {
            return node.InnerText.Split(c).Length;
        }
    }
}
