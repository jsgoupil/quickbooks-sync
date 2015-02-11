using QbSync.QbXml.Extensions;
using QbSync.QbXml.Type;
using System.Collections.Generic;
using System.Xml;

namespace QbSync.QbXml.Messages.Requests
{
    public class DataExtDefAddRequest : DataExtDefRequest
    {
        public DataExtDefAddRequest()
            : base("DataExtDefAddRq")
        {
        }

        public IEnumerable<StrType> IncludeRetElement { get; set; }

        protected override void BuildRequest(XmlElement parent)
        {
            var doc = parent.OwnerDocument;
            var dataExtDefAdd = doc.CreateElement("DataExtDefAdd");
            parent.AppendChild(dataExtDefAdd);
            base.BuildRequest(dataExtDefAdd);

            if (IncludeRetElement != null)
            {
                parent.AppendTags("IncludeRetElement", IncludeRetElement);
            }
        }
    }
}
