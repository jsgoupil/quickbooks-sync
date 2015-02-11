using QbSync.QbXml.Type;
using System.Collections.Generic;

namespace QbSync.QbXml.Objects
{
    public class Customer : ContactType
    {
        public StrType Name { get; set; }
        public StrType FullName { get; set; }
        public BoolType IsActive { get; set; }
        public Ref ClassRef { get; set; }
        public Ref ParentRef { get; set; }
        public IntType Sublevel { get; set; }
        public StrType CompanyLevel { get; set; }
        public Address BillAddress { get; set; }
        public AddressBlock BillAddressBlock { get; set; }
        public Address ShipAddress { get; set; }
        public AddressBlock ShipAddressBlock { get; set; }
        public ShipAddress ShipToAddress { get; set; }
        public StrType Phone { get; set; }
        public StrType AltPhone { get; set; }
        public StrType Fax { get; set; }
        public StrType Email { get; set; }
        public StrType Cc { get; set; }
        public StrType AltContact { get; set; }
        public IEnumerable<ContactRef> AdditionalContactRef { get; set; }
        public IEnumerable<ContactType> ContactContactsRet { get; set; }
        public Ref CustomerTypeRef { get; set; }
        public Ref TermsRef { get; set; }
        public Ref SalesRepRef { get; set; }
        public AmtType Balance { get; set; }
        public AmtType TotalBalance { get; set; }
        public Ref SalesTaxCodeRef { get; set; }
        public Ref ItemSalesTaxRef { get; set; }
        public StrType ResaleNumber { get; set; }
        public StrType AccountNumber { get; set; }
        public AmtType CreditLimit { get; set; }
        public Ref PreferredPaymentMethodRef { get; set; }
        public CreditCard CreditCardInfo { get; set; }
        public JobStatus JobStatus { get; set; }
        public DateTimeType JobStartDate { get; set; }
        public DateTimeType JobProjectedEndDate { get; set; }
        public DateTimeType JobEndDate { get; set; }
        public StrType JobDesc { get; set; }
        public Ref JobTypeRef { get; set; }
        public StrType Notes { get; set; }
        public IEnumerable<AdditionalNoteRet> AdditionalNotesRet { get; set; }
        public PreferredDeliveryMethod PreferredDeliveryMethod { get; set; }
        public Ref PriceLevelRef { get; set; }
        public GuidType ExternalGUID { get; set; }
        public Ref CurrencyRef { get; set; }
        public IEnumerable<DataExtRet> DataExtRet { get; set; }
    }
}