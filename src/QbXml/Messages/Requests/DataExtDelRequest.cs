using System.Xml;

namespace QbSync.QbXml.Messages.Requests
{
    public class DataExtDelRequest : DataExtRequest
    {
        public DataExtDelRequest()
            : base("DataExtDelRq")
        {
        }

        protected override void BuildRequest(XmlElement parent)
        {
            var doc = parent.OwnerDocument;
            var dataExtDel = doc.CreateElement("DataExtDel");
            parent.AppendChild(dataExtDel);
            base.BuildRequest(dataExtDel);
        }
    }
}
