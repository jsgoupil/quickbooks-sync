using QbSync.QbXml.Extensions;
using QbSync.QbXml.Struct;
using QbSync.QbXml.Type;
using System.Xml;

namespace QbSync.QbXml.Objects
{
    public abstract class InvoiceLineAddMod : InvoiceLineBase, IXmlConvertible
    {
        public Ref PriceLevelRef { get; set; }
        public OptionForPriceRuleConflict? OptionForPriceRuleConflict { get; set; }
        public Ref OverrideItemAccountRef { get; set; }

        public override void AppendXml(XmlElement parent)
        {
            var doc = parent.OwnerDocument;

            parent.AppendTag("ItemRef", ItemRef);
            base.AppendXml(parent);
            parent.AppendTag("Rate", Rate);
            parent.AppendTag("RatePercent", RatePercent);
            parent.AppendTag("PriceLevelRef", PriceLevelRef);
            parent.AppendTag("ClassRef", ClassRef);
            parent.AppendTag("Amount", Amount);
            if (OptionForPriceRuleConflict.HasValue)
            {
                parent.AppendChild(doc.CreateElementWithValue("OptionForPriceRuleConflict", OptionForPriceRuleConflict.Value.ToString()));
            }

            parent.AppendTag("InventorySiteRef", InventorySiteRef);
            parent.AppendTag("InventorySiteLocationRef", InventorySiteLocationRef);
            parent.AppendTag("SerialNumber", SerialNumber);
            parent.AppendTag("LotNumber", LotNumber);
            parent.AppendTag("ServiceDate", ServiceDate);
            parent.AppendTag("SalesTaxCodeRef", SalesTaxCodeRef);
            parent.AppendTag("OverrideItemAccountRef", OverrideItemAccountRef);
            parent.AppendTag("Other1", Other1);
            parent.AppendTag("Other2", Other2);
        }
    }
}