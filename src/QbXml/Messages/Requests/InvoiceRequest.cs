using QbSync.QbXml.Extensions;
using QbSync.QbXml.Objects;
using QbSync.QbXml.Type;
using System.Collections.Generic;

namespace QbSync.QbXml.Messages.Requests
{
    public class InvoiceRequest : QbXmlRequest
    {
        public InvoiceRequest(string rootElementName)
            : base(rootElementName)
        {
        }

        public Ref CustomerRef { get; set; }
        public Ref ClassRef   { get; set; }
        public Ref ARAccountRef { get; set; }
        public Ref TemplateRef { get; set; }
        public DateType TxnDate { get; set; }
        public StrType RefNumber { get; set; }
        public Address BillAddress { get; set; }
        public Address ShipAddress  { get; set; }
        public BoolType IsPending { get; set; }
        public BoolType IsFinanceCharge { get; set; }
        public StrType PONumber { get; set; }
        public Ref TermsRef { get; set; }
        public DateType DueDate { get; set; }
        public Ref SalesRepRef { get; set; }
        public StrType FOB { get; set; }
        public DateType ShipDate { get; set; }
        public Ref ShipMethodRef { get; set; }
        public Ref ItemsSalesTaxRef { get; set; }
        public StrType Memo { get; set; }
        public Ref CustomerMsgRef { get; set; }
        public BoolType IsToBePrinted { get; set; }
        public BoolType IsToBeEmailed { get; set; }
        public Ref CustomerSalesTaxCodeRef { get; set; }
        public StrType Other { get; set; }
        public FloatType ExchangeRate { get; set; }
        public IEnumerable<SetCredit> SetCredit { get; set; }
        public IEnumerable<StrType> IncludeRetElement { get; set; }

        protected override void BuildRequest(System.Xml.XmlElement parent)
        {
            base.BuildRequest(parent);

            parent.AppendTagIfNotNull("CustomerRef", CustomerRef);
            parent.AppendTagIfNotNull("ClassRef", ClassRef);
            parent.AppendTagIfNotNull("ARAccountRef", ARAccountRef);
            parent.AppendTagIfNotNull("TemplateRef", TemplateRef);
            parent.AppendTagIfNotNull("TxnDate", TxnDate);
            parent.AppendTagIfNotNull("RefNumber", RefNumber);
            parent.AppendTagIfNotNull("BillAddress", BillAddress);
            parent.AppendTagIfNotNull("ShipAddress", ShipAddress);
            parent.AppendTagIfNotNull("IsPending", IsPending);
            parent.AppendTagIfNotNull("IsFinanceCharge", IsFinanceCharge);
            parent.AppendTagIfNotNull("PONumber", PONumber);
            parent.AppendTagIfNotNull("TermsRef", TermsRef);
            parent.AppendTagIfNotNull("DueDate", DueDate);
            parent.AppendTagIfNotNull("SalesRepRef", SalesRepRef);
            parent.AppendTagIfNotNull("FOB", FOB);
            parent.AppendTagIfNotNull("ShipDate", ShipDate);
            parent.AppendTagIfNotNull("ShipMethodRef", ShipMethodRef);
            parent.AppendTagIfNotNull("ItemsSalesTaxRef", ItemsSalesTaxRef);
            parent.AppendTagIfNotNull("Memo", Memo);
            parent.AppendTagIfNotNull("CustomerMsgRef", CustomerMsgRef);
            parent.AppendTagIfNotNull("IsToBePrinted", IsToBePrinted);
            parent.AppendTagIfNotNull("IsToBeEmailed", IsToBeEmailed);
            parent.AppendTagIfNotNull("CustomerSalesTaxCodeRef", CustomerSalesTaxCodeRef);
            parent.AppendTagIfNotNull("Other", Other);
            parent.AppendTagIfNotNull("ExchangeRate", ExchangeRate);
        }
    }
}
