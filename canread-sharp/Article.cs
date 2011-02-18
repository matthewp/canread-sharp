using System;
using System.Collections.Generic;
using System.Text;
using HtmlAgilityPack;

namespace canread_sharp
{
    public class Article
    {
        public string Title { get; set; }
        public HtmlDocument Content { get; set; }

        public Article()
        {
            Content = new HtmlDocument();
        }

        public override string ToString()
        {
            return Content.DocumentNode.InnerText;
        }
    }
}
