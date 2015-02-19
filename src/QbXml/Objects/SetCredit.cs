using QbSync.QbXml.Extensions;
using QbSync.QbXml.Type;
using System.Xml;

namespace QbSync.QbXml.Objects
{
    public class SetCredit : IXmlConvertible
    {
        public IdType CreditTxnID { get; set; }
        public AmtType AppliedAmount { get; set; }
        public BoolType Override { get; set; }

        public virtual void AppendXml(XmlElement parent)
        {
            parent.AppendTag("CreditTxnID", CreditTxnID);
            parent.AppendTag("AppliedAmount", AppliedAmount);

            if (Override != null)
            {
                parent.AppendTag("Override", Override);
            }
        }
    }
}
