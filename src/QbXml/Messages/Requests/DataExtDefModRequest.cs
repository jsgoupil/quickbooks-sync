using QbSync.QbXml.Extensions;
using QbSync.QbXml.Type;
using System.Collections.Generic;
using System.Xml;

namespace QbSync.QbXml.Messages.Requests
{
    public class DataExtDefModRequest : DataExtDefRequest
    {
        public DataExtDefModRequest()
            : base("DataExtDefModRq")
        {
        }

        public StrType DataExtNewName { get; set; }
        public IEnumerable<StrType> IncludeRetElement { get; set; }

        protected override void BuildRequest(XmlElement parent)
        {
            var doc = parent.OwnerDocument;
            var dataExtDefMod = doc.CreateElement("DataExtDefMod");
            parent.AppendChild(dataExtDefMod);
            base.BuildRequest(dataExtDefMod);

            if (DataExtNewName != null)
            {
                dataExtDefMod.AppendTag("DataExtNewName", DataExtNewName);
            }

            if (IncludeRetElement != null)
            {
                parent.AppendTags("IncludeRetElement", IncludeRetElement);
            }
        }
    }
}
