using QbSync.QbXml.Extensions;
using QbSync.QbXml.Struct;
using QbSync.QbXml.Type;
using System.Collections.Generic;
using System.Xml;

namespace QbSync.QbXml.Messages.Requests
{
    public class DataExtDefAddRequest : QbXmlRequest
    {
        public DataExtDefAddRequest()
            : base("DataExtDefAddRq")
        {
        }

        public GuidType OwnerID { get; set; }
        public StrType DataExtName { get; set; }
        public DataExtType DataExtType { get; set; }
        public IEnumerable<AssignToObject> AssignToObject { get; set; }
        public BoolType DataExtListRequire { get; set; }
        public BoolType DataExtTxnRequire { get; set; }
        public StrType DataExtFormatString { get; set; }
        public IEnumerable<StrType> IncludeRetElement { get; set; }

        protected override void BuildRequest(XmlElement parent)
        {
            var doc = parent.OwnerDocument;
            var dataExtDefAdd = doc.CreateElement("DataExtDefAdd");
            dataExtDefAdd.AppendTag("OwnerID", OwnerID);
            dataExtDefAdd.AppendTag("DataExtName", DataExtName);

            dataExtDefAdd.AppendChild(doc.CreateElementWithValue("DataExtType", DataExtType.ToString()));

            if (AssignToObject != null)
            {
                foreach (var assignment in AssignToObject)
                {
                    dataExtDefAdd.AppendChild(doc.CreateElementWithValue("AssignToObject", assignment.ToString()));
                }
            }

            if (DataExtListRequire != null)
            {
                dataExtDefAdd.AppendTag("DataExtListRequire", DataExtListRequire);
            }

            if (DataExtTxnRequire != null)
            {
                dataExtDefAdd.AppendTag("DataExtTxnRequire", DataExtTxnRequire);
            }

            if (DataExtFormatString != null)
            {
                dataExtDefAdd.AppendTag("DataExtFormatString", DataExtFormatString);
            }

            parent.AppendChild(dataExtDefAdd);
            if (IncludeRetElement != null)
            {
                parent.AppendTags("IncludeRetElement", IncludeRetElement);
            }

            base.BuildRequest(dataExtDefAdd);
        }
    }
}
