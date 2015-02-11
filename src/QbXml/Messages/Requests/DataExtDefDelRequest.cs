using QbSync.QbXml.Extensions;
using QbSync.QbXml.Type;
using System.Collections.Generic;
using System.Xml;

namespace QbSync.QbXml.Messages.Requests
{
    public class DataExtDefDelRequest : QbXmlRequest
    {
        public DataExtDefDelRequest()
            : base("DataExtDefDelRq")
        {
        }

        public GuidType OwnerID { get; set; }
        public StrType DataExtName { get; set; }

        protected override void BuildRequest(XmlElement parent)
        {
            base.BuildRequest(parent);
            parent.AppendTag("OwnerID", OwnerID);
            parent.AppendTag("DataExtName", DataExtName);
        }
    }
}
