using QbSync.QbXml.Extensions;
using QbSync.QbXml.Struct;
using QbSync.QbXml.Type;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace QbSync.QbXml.Filters
{
    public class RefNumberFilter
    {
        public MatchCriterion MatchCriterion { get; set; }
        public StrType RefNumber { get; set; }

        public void AppendXml(XmlElement parent)
        {
            parent.AppendChild(parent.OwnerDocument.CreateElementWithValue("MatchCriterion", MatchCriterion.ToString()));
            parent.AppendTag("RefNumber", RefNumber);
        }
    }
}
