using QbSync.QbXml.Struct;
using QbSync.QbXml.Type;

namespace QbSync.QbXml.Objects
{
    public class LinkedTxn
    {
        public IdType TxnID { get; set; }
        public TxnType TxnType { get; set; }
        public DateType TxnDate { get; set; }
        public StrType RefNumber { get; set; }
        public LinkType LinkType { get; set; }
        public AmtType Amount { get; set; }
    }
}