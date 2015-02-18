using QbSync.QbXml.Extensions;
using QbSync.QbXml.Struct;
using QbSync.QbXml.Type;
using System.Collections.Generic;
using System.Xml;

namespace QbSync.QbXml.Messages.Requests
{
    public class DataExtDefModRequest : QbXmlRequest
    {
        public DataExtDefModRequest()
            : base("DataExtDefModRq")
        {
        }

        public GuidType OwnerID { get; set; }
        public StrType DataExtName { get; set; }
        public StrType DataExtNewName { get; set; }
        public IEnumerable<AssignToObject> AssignToObject { get; set; }
        public IEnumerable<AssignToObject> RemoveFromObject { get; set; }
        public BoolType DataExtListRequire { get; set; }
        public BoolType DataExtTxnRequire { get; set; }
        public StrType DataExtFormatString { get; set; }
        public IEnumerable<StrType> IncludeRetElement { get; set; }

        protected override void BuildRequest(XmlElement parent)
        {

            var doc = parent.OwnerDocument;
            var dataExtDefMod = doc.CreateElement("DataExtDefMod");
            dataExtDefMod.AppendTag("OwnerID", OwnerID);
            dataExtDefMod.AppendTag("DataExtName", DataExtName);

            if (DataExtNewName != null)
            {
                dataExtDefMod.AppendTag("DataExtNewName", DataExtNewName);
            }

            if (AssignToObject != null)
            {
                foreach (var assignment in AssignToObject)
                {
                    dataExtDefMod.AppendChild(doc.CreateElementWithValue("AssignToObject", assignment.ToString()));
                }
            }

            if (RemoveFromObject != null)
            {
                foreach (var assignment in RemoveFromObject)
                {
                    dataExtDefMod.AppendChild(doc.CreateElementWithValue("RemoveFromObject", assignment.ToString()));
                }
            }

            if (DataExtListRequire != null)
            {
                dataExtDefMod.AppendTag("DataExtListRequire", DataExtListRequire);
            }

            if (DataExtTxnRequire != null)
            {
                dataExtDefMod.AppendTag("DataExtTxnRequire", DataExtTxnRequire);
            }

            if (DataExtFormatString != null)
            {
                dataExtDefMod.AppendTag("DataExtFormatString", DataExtFormatString);
            }

            parent.AppendChild(dataExtDefMod);
            if (IncludeRetElement != null)
            {
                parent.AppendTags("IncludeRetElement", IncludeRetElement);
            }

            base.BuildRequest(dataExtDefMod);
        }
    }
}
