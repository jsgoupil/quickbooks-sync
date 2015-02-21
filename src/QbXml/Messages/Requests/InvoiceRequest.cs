using QbSync.QbXml.Objects;
using QbSync.QbXml.Type;
using System.Collections.Generic;

namespace QbSync.QbXml.Messages.Requests
{
    public class InvoiceRequest<T> : QbXmlObject<T>
        where T : new()
    {
        public CustomerRef CustomerRef { get; set; }
        public ClassRef ClassRef { get; set; }
        public ARAccountRef ARAccountRef { get; set; }
        public TemplateRef TemplateRef { get; set; }
        public DateType TxnDate { get; set; }
        public string RefNumber { get; set; }
        public BillAddress BillAddress { get; set; }
        public ShipAddress ShipAddress { get; set; }
        public BoolType IsPending { get; set; }
        public string PONumber { get; set; }
        public TermsRef TermsRef { get; set; }
        public DateType DueDate { get; set; }
        public SalesRepRef SalesRepRef { get; set; }
        public string FOB { get; set; }
        public DateType ShipDate { get; set; }
        public ShipMethodRef ShipMethodRef { get; set; }
        public ItemSalesTaxRef ItemSalesTaxRef { get; set; }
        public string Memo { get; set; }
        public CustomerMsgRef CustomerMsgRef { get; set; }
        public BoolType IsToBePrinted { get; set; }
        public BoolType IsToBeEmailed { get; set; }
        public BoolType IsTaxIncluded { get; set; }
        public CustomerSalesTaxCodeRef CustomerSalesTaxCodeRef { get; set; }
        public string Other { get; set; }
        public FloatType ExchangeRate { get; set; }
        public IEnumerable<SetCredit> SetCredit { get; set; }
        public IEnumerable<string> IncludeRetElement { get; set; }
    }
}
