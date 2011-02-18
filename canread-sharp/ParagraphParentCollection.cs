using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace canread_sharp
{
    public class ParagraphParentCollection : List<ParagraphParent>
    {
        public ParagraphParentCollection(ParagraphCollection collection)
        {
            if (collection.Count > 0)
            {
                Paragraph last = new Paragraph();
                foreach (Paragraph paragraph in collection)
                {
                    ParagraphParent parent = this.FirstOrDefault(a => a.Paragraphs.Exists(b => b.Node.ParentNode == paragraph.Node.ParentNode));
                    if (parent != null && !parent.Paragraphs.Contains(paragraph))
                        parent.Add(paragraph);
                    else
                    {
                        ParagraphParent newParent = new ParagraphParent();
                        newParent.Node = paragraph.Node.ParentNode;
                        newParent.Add(paragraph);
                        this.Add(newParent);
                    }
                }
            }
        }
    }
}
