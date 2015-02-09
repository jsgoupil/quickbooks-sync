using QbSync.QbXml.Type;

namespace QbSync.QbXml.Objects
{
    public class CreditCard
    {
        public StrType CreditCardNumber { get; set; }
        public IntType ExpirationMonth { get; set; }
    }
}
