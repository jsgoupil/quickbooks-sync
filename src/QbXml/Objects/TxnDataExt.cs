using QbSync.QbXml.Type;
using System.Xml;
using QbSync.QbXml.Extensions;
using QbSync.QbXml.Struct;

namespace QbSync.QbXml.Objects
{
    public class TxnDataExt
    {
        public TxnDataExtType TxnDataExtType { get; set; }
        public IdType TxnID { get; set; }
        public IdType TxnLineID { get; set; }

        public void AppendXml(XmlElement parent)
        {
            parent.AppendChild(parent.OwnerDocument.CreateElementWithValue("TxnDataExtType", TxnDataExtType.ToString()));
            parent.AppendTag("TxnID", TxnID);

            if (TxnLineID != null)
            {
                parent.AppendTag("TxnLineID", TxnLineID);
            }
        }
    }
}