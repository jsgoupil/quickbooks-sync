using QBSync.QbXml.Type;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace QBSync.QbXml
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

        protected override void BuildRequest(XmlDocument doc, XmlElement parent)
        {
            base.BuildRequest(doc, parent);

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
