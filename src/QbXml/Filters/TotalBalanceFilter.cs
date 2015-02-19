using QbSync.QbXml.Extensions;
using QbSync.QbXml.Struct;
using QbSync.QbXml.Type;
using System.Xml;

namespace QbSync.QbXml.Filters
{
    public class TotalBalanceFilter : IXmlConvertible
    {
        public Operator Operator
        {
            get;
            set;
        }

        public AmtType Amount
        {
            get;
            set;
        }

        public virtual void AppendXml(XmlElement parent)
        {
            parent.AppendChild(parent.OwnerDocument.CreateElementWithValue("Operator", Operator.ToString()));
            parent.AppendTag("Amount", Amount);
        }
    }
}
