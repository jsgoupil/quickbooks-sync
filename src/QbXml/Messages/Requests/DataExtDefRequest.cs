using QbSync.QbXml.Extensions;
using QbSync.QbXml.Objects;
using QbSync.QbXml.Struct;
using QbSync.QbXml.Type;
using System.Collections.Generic;
using System.Xml;

namespace QbSync.QbXml.Messages.Requests
{
    public abstract class DataExtDefRequest : QbXmlRequest
    {
        public DataExtDefRequest(string rootElementName)
            : base(rootElementName)
        {
        }

        public GuidType OwnerID { get; set; }
        public StrType DataExtName { get; set; }
        public DataExtType DataExtType { get; set; }
        public IEnumerable<AssignToObject> AssignToObject { get; set; }
        public BoolType DataExtListRequire { get; set; }
        public BoolType DataExtTxnRequire { get; set; }
        public StrType DataExtFormatString { get; set; }

        protected override void BuildRequest(XmlElement parent)
        {
            base.BuildRequest(parent);

            var doc = parent.OwnerDocument;
            parent.AppendTag("OwnerID", OwnerID);
            parent.AppendTag("DataExtName", DataExtName);

            parent.AppendChild(doc.CreateElementWithValue("DataExtType", DataExtType.ToString()));

            if (AssignToObject != null)
            {
                foreach (var assignment in AssignToObject)
                {
                    parent.AppendChild(doc.CreateElementWithValue("AssignToObject", assignment.ToString()));
                }
            }

            if (DataExtListRequire != null)
            {
                parent.AppendTag("DataExtListRequire", DataExtListRequire);
            }

            if (DataExtTxnRequire != null)
            {
                parent.AppendTag("DataExtTxnRequire", DataExtTxnRequire);
            }

            if (DataExtFormatString != null)
            {
                parent.AppendTag("DataExtFormatString", DataExtFormatString);
            }
        }
    }
}
