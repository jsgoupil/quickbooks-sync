using QbSync.QbXml.Extensions;
using QbSync.QbXml.Struct;
using QbSync.QbXml.Type;
using System;
using System.Collections.Generic;
using System.Xml;

namespace QbSync.QbXml.Messages.Requests
{
    public class DataExtDefQueryRequest : QbXmlRequest
    {
        public DataExtDefQueryRequest()
            : base("DataExtDefQueryRq")
        {
        }

        public IEnumerable<GuidType> OwnerID { get; set; }
        public IEnumerable<AssignToObject> AssignToObject { get; set; }
        public IEnumerable<StrType> IncludeRetElement { get; set; }

        protected override void BuildRequest(XmlElement parent)
        {
            base.BuildRequest(parent);

            var doc = parent.OwnerDocument;

            if (OwnerID != null && AssignToObject != null)
            {
                throw new ArgumentException("You cannot set OwnerID and AssignToObject at the same time.");
            }

            if (OwnerID != null)
            {
                parent.AppendTags("OwnerID", OwnerID);
            }

            if (AssignToObject != null)
            {
                foreach (var assignment in AssignToObject)
                {
                    parent.AppendChild(doc.CreateElementWithValue("AssignToObject", assignment.ToString()));
                }
            }

            if (IncludeRetElement != null)
            {
                parent.AppendTags("IncludeRetElement", IncludeRetElement);
            }
        }
    }
}
