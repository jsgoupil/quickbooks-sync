using QbSync.QbXml.Extensions;
using QbSync.QbXml.Objects;
using QbSync.QbXml.Type;
using System;
using System.Collections.Generic;
using System.Xml;

namespace QbSync.QbXml.Messages.Requests
{
    public class InvoiceAddRequest : InvoiceRequest
    {
        public InvoiceAddRequest()
            : base("InvoiceAddRq")
        {
        }

        public GuidType ExternalGUID { get; set; }
        public IEnumerable<IdType> LinkToTxnID { get; set; }
        public InvoiceLineAdd InvoiceLineAdd { get; set; }
        public InvoiceLineGroupAdd InvoiceLineGroupAdd { get; set; }

        protected override void BuildRequest(XmlElement parent)
        {
            if (CustomerRef == null)
            {
                throw new Exception("CustomerRef is mandatory.");
            }

            var doc = parent.OwnerDocument;
            var invoiceAdd = doc.CreateElement("InvoiceAdd");
            parent.AppendChild(invoiceAdd);
            base.BuildRequest(invoiceAdd);

            invoiceAdd.AppendTagIfNotNull("ExternalGUID", ExternalGUID);
            invoiceAdd.AppendTagsIfNotNull("LinkToTxnID", LinkToTxnID);
            invoiceAdd.AppendTagsIfNotNull("SetCredit", SetCredit);
            invoiceAdd.AppendTagIfNotNull("InvoiceLineAdd", InvoiceLineAdd);
            invoiceAdd.AppendTagIfNotNull("InvoiceLineGroupAdd", InvoiceLineGroupAdd);
            invoiceAdd.AppendTagsIfNotNull("IncludeRetElement", IncludeRetElement);
        }
    }
}
