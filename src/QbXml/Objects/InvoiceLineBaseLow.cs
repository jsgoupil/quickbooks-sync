using QbSync.QbXml.Extensions;
using QbSync.QbXml.Type;
using System.Xml;

namespace QbSync.QbXml.Objects
{
    public abstract class InvoiceLineBaseLow
    {
        public StrType Desc { get; set; }
        public QuanType Quantity { get; set; }
        public StrType UnitOfMeasure { get; set; }
        public Ref OverrideUOMSetRef { get; set; }

        public virtual void AppendXml(XmlElement parent)
        {
            if (Desc != null)
            {
                parent.AppendTag("Desc", Desc);
            }

            if (Quantity != null)
            {
                parent.AppendTag("Quantity", Quantity);
            }

            if (UnitOfMeasure != null)
            {
                parent.AppendTag("UnitOfMeasure", UnitOfMeasure);
            }

            if (OverrideUOMSetRef != null)
            {
                parent.AppendTag("OverrideUOMSetRef", OverrideUOMSetRef);
            }
        }
    }
}
