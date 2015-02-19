using QbSync.QbXml.Type;
using System.Collections.Generic;

namespace QbSync.QbXml.Objects
{
    public class Invoice
    {
        public IdType TxnID { get; set; }
        public DateTimeType TimeCreated { get; set; }
        public DateTimeType TimeModified { get; set; }
        public StrType EditSequence { get; set; }
        public IntType TxnNumber { get; set; }
        public Ref CustomerRef { get; set; }
        public Ref ClassRef { get; set; }
        public Ref ARAccountRef { get; set; }
        public Ref TemplateRef { get; set; }
        public DateType TxnDate { get; set; }
        public StrType RefNumber { get; set; }
        public Address BillAddress { get; set; }
        public AddressBlock BillAddressBlock { get; set; }
        public Address ShipAddress { get; set; }
        public AddressBlock ShipAddressBlock { get; set; }
        public BoolType IsPending { get; set; }
        public BoolType IsFinanceCharge { get; set; }
        public StrType PONumber { get; set; }
        public Ref TermsRef { get; set; }
        public DateType DueDate { get; set; }
        public Ref SalesRepRef { get; set; }
        public StrType FOB { get; set; }
        public DateType ShipDate { get; set; }
        public Ref ShipMethodRef { get; set; }
        public AmtType Subtotal { get; set; }
        public Ref ItemSalesTaxRef { get; set; }
        public PercentType SalesTaxPercentage { get; set; }
        public AmtType SalesTaxTotal { get; set; }
        public AmtType AppliedAmount { get; set; }
        public AmtType BalanceRemaining { get; set; }
        public Ref CurrencyRef { get; set; }
        public FloatType ExchangRate { get; set; }
        public AmtType BalanceRemainingInHomeCurrency { get; set; }
        public StrType Memo { get; set; }
        public BoolType IsPaid { get; set; }
        public Ref CustomerMsgRef { get; set; }
        public BoolType IsToBePrinted { get; set; }
        public BoolType IsToBeEmailed { get; set; }
        public Ref CustomerSalesTaxCodeRef { get; set; }
        public AmtType SuggestedDiscountAmount { get; set; }
        public DateType SuggestedDiscountDate { get; set; }
        public StrType Other { get; set; }
        public GuidType ExternalGUID { get; set; }
        public IEnumerable<LinkedTxn> LinkedTxn { get; set; }
        public InvoiceLine InvoiceLineRet { get; set; }
        public InvoiceLineGroup InvoiceLineGroupRet { get; set; }
        public IEnumerable<DataExt> DataExtRet { get; set; }
    }
}