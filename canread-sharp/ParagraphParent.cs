using System;
using System.Collections.Generic;
using System.Text;
using HtmlAgilityPack;

namespace canread_sharp
{
    public class ParagraphParent
    {
        public List<Paragraph> Paragraphs { get; set; }
        public HtmlNode Node { get; set; }
        public int Score { get; set; }

        public ParagraphParent()
        {
            Paragraphs = new List<Paragraph>();
            Score = 0;
        }

        public void Add(Paragraph paragraph)
        {
            Paragraphs.Add(paragraph);
        }

        public void Clean(string name, int minWords = 1000000)
        {
            HtmlNodeCollection childrenOf = Node.SelectNodes("//" + name);
            if (childrenOf != null)
                foreach (HtmlNode child in childrenOf)
                    if (GetCharCount(child, ' ') < minWords)
                        Node.RemoveChild(child);
        }

        public void KillDivs()
        {
            HtmlNodeCollection divs = Node.SelectNodes("//div");
            List<HtmlNode> toRemove = new List<HtmlNode>();

            foreach (HtmlNode div in divs)
            {
                int p = div.SelectNodes("//p") == null ? 0 : div.SelectNodes("//p").Count;
                int img = div.SelectNodes("/img") == null ? 0 : div.SelectNodes("//img").Count;
                int li = div.SelectNodes("//li") == null ? 0 : div.SelectNodes("//li").Count;
                int a = div.SelectNodes("//a") == null ? 0 : div.SelectNodes("//a").Count;
                var embed = div.SelectNodes("//embed") == null ? 0 : div.SelectNodes("//embed").Count;

                if (GetCharCount(div) < 10)
                    if (img > p || li > p || a > p || p == 0 || embed > 0)
                        toRemove.Add(div);
            }

            foreach (HtmlNode r in toRemove)
                Node.RemoveChild(r);
        }

        public void KillBreaks()
        {
            Node.InnerHtml = System.Text.RegularExpressions.Regex.Replace(Node.InnerHtml, @"/(<br\s*\/?>(\s|&nbsp;?)*){1,}/g", "<br />");
        }

        static int GetCharCount(HtmlNode node, char c = ',')
        {
            return node.InnerText.Split(c).Length;
        }
    }
}
