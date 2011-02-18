using System;
using System.Collections.Generic;
using System.Text;
using HtmlAgilityPack;

namespace canread_sharp
{
    public class Paragraph
    {
        public HtmlNode Node { get; set; }

        public Paragraph() 
        {
            this.Node = null;
        }

        public Paragraph(HtmlNode node)
        {
            this.Node = node;
        }
    }
}
