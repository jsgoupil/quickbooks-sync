using QbSync.QbXml.Extensions;
using QbSync.QbXml.Objects;
using QbSync.QbXml.Type;
using System.Xml;

namespace QbSync.QbXml.Messages.Requests
{
    public class InvoiceModRequest : InvoiceRequest
    {
        public InvoiceModRequest()
            : base("InvoiceModRq")
        {
        }

        public IdType TxnID { get; set; }
        public StrType EditSequence { get; set; }
        public InvoiceLineMod InvoiceLineMod { get; set; }
        public InvoiceLineGroupMod InvoiceLineGroupMod { get; set; }

        protected override void BuildRequest(XmlElement parent)
        {
            var doc = parent.OwnerDocument;
            var invoiceMod = doc.CreateElement("InvoiceMod");
            parent.AppendChild(invoiceMod);

            invoiceMod.AppendTag("TxnID", TxnID);
            invoiceMod.AppendTag("EditSequence", EditSequence);
            base.BuildRequest(invoiceMod);
            invoiceMod.AppendTagsIfNotNull("SetCredit", SetCredit);
            invoiceMod.AppendTagIfNotNull("InvoiceLineMod", InvoiceLineMod);
            invoiceMod.AppendTagIfNotNull("InvoiceLineGroupMod", InvoiceLineGroupMod);
            invoiceMod.AppendTagsIfNotNull("IncludeRetElement", IncludeRetElement);
        }
    }
}
