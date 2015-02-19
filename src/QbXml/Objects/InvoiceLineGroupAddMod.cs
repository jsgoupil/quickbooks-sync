using QbSync.QbXml.Extensions;
using QbSync.QbXml.Type;
using System.Xml;

namespace QbSync.QbXml.Objects
{
    public class InvoiceLineGroupAddMod : IXmlConvertible
    {
        public Ref ItemGroupRef { get; set; }
        public QuanType Quantity { get; set; }
        public StrType UnitOfMeasure { get; set; }

        public virtual void AppendXml(XmlElement parent)
        {
            parent.AppendTagIfNotNull("ItemGroupRef", ItemGroupRef);
            parent.AppendTagIfNotNull("Quantity", Quantity);
            parent.AppendTagIfNotNull("UnitOfMeasure", UnitOfMeasure);
        }
    }
}
