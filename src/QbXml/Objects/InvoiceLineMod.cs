using QbSync.QbXml.Extensions;
using QbSync.QbXml.Type;
using System.Xml;

namespace QbSync.QbXml.Objects
{
    public class InvoiceLineMod : InvoiceLineAddMod, IXmlConvertible
    {
        public IdType TxnLineID { get; set; }

        public override void AppendXml(XmlElement parent)
        {
            parent.AppendTag("TxnLineID", TxnLineID);
            base.AppendXml(parent);
        }
    }
}