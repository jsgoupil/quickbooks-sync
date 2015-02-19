using QbSync.QbXml.Extensions;
using QbSync.QbXml.Type;
using System.Xml;

namespace QbSync.QbXml.Objects
{
    public class LinkToTxn : IXmlConvertible
    {
        public IdType TxnID { get; set; }
        public IdType TxnLineID { get; set; }

        public virtual void AppendXml(XmlElement parent)
        {
            parent.AppendTag("TxnID", TxnID);
            parent.AppendTag("TxnLineID", TxnLineID);
        }
    }
}
