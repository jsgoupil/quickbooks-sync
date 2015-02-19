using QbSync.QbXml.Extensions;
using QbSync.QbXml.Type;
using System.Collections.Generic;
using System.Xml;

namespace QbSync.QbXml.Objects
{
    public class InvoiceLineGroupMod : InvoiceLineGroupAddMod, IXmlConvertible
    {
        public IdType TxnLineID { get; set; }
        public Ref OverrideUOMSetRef { get; set; }
        public IEnumerable<InvoiceLineMod> InvoiceLineMod { get; set; }

        public override void AppendXml(XmlElement parent)
        {
            parent.AppendTagIfNotNull("TxnLineID", TxnLineID);
            base.AppendXml(parent);
            parent.AppendTagIfNotNull("OverrideUOMSetRef", OverrideUOMSetRef);
            parent.AppendTagsIfNotNull("InvoiceLineMod", InvoiceLineMod);
        }
    }
}
