using QbSync.QbXml.Type;

namespace QbSync.QbXml.Objects
{
    public class ErrorRecovery
    {
        public IdType ListID { get; set; }
        public GuidType OwnerID { get; set; }
        public IdType TxnID { get; set; }
        public IntType TxnNumber { get; set; }
        public StrType EditSequence { get; set; }
        public GuidType ExternalGUID { get; set; }
    }
}
