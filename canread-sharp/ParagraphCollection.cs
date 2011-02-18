using System;
using System.Collections.Generic;
using System.Text;
using HtmlAgilityPack;

namespace canread_sharp
{
    public class ParagraphCollection : List<Paragraph>
    {
        public ParagraphCollection(HtmlNodeCollection nodes)
        {
            Add(nodes);
        }

        public void Add(HtmlNodeCollection nodes)
        {
            foreach (HtmlNode node in nodes)
                Add(new Paragraph(node));
        }
    }
}
