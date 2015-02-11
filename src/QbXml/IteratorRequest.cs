using QbSync.QbXml.Type;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
namespace QbSync.QbXml
{
    public abstract class IteratorRequest : QbXmlRequest
    {
        public IteratorRequest(string rootElementName)
            : base(rootElementName)
        {
        }

        public IntType MaxReturned
        {
            get;
            set;
        }

        public string IteratorID
        {
            get;
            set;
        }

        protected override void BuildRequest(XmlElement parent)
        {
            base.BuildRequest(parent);

            if (MaxReturned != null)
            {
                if (string.IsNullOrEmpty(IteratorID))
                {
                    parent.SetAttribute("iterator", "Start");
                }
                else
                {
                    parent.SetAttribute("iterator", "Continue");
                    parent.SetAttribute("iteratorID", IteratorID);
                }
            }
        }
    }
}
