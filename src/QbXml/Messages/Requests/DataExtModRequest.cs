using QbSync.QbXml.Extensions;
using QbSync.QbXml.Type;
using System.Xml;

namespace QbSync.QbXml.Messages.Requests
{
    public class DataExtModRequest : DataExtRequest
    {
        public DataExtModRequest()
            : base("DataExtModRq")
        {
        }

        public StrType DataExtValue { get; set; }

        protected override void BuildRequest(XmlElement parent)
        {
            var doc = parent.OwnerDocument;
            var dataExtMod = doc.CreateElement("DataExtMod");
            parent.AppendChild(dataExtMod);
            base.BuildRequest(dataExtMod);
            dataExtMod.AppendTag("DataExtValue", DataExtValue);
        }
    }
}
