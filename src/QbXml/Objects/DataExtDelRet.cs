using QbSync.QbXml.Struct;
using QbSync.QbXml.Type;

namespace QbSync.QbXml.Objects
{
    public class DataExtDelRet
    {
        public GuidType OwnerID { get; set; }
        public StrType DataExtName { get; set; }

        public ListDataExtType ListDataExtType { get; set; }
        public Ref ListObjRef { get; set; }

        public TxnDataExtType TxnDataExtType { get; set; }
        public IdType TxnID { get; set; }
        public IdType TxnLineID { get; set; }

        public OtherDataExtType? OtherDataExtType { get; set; }

        public DateTimeType TimeDeleted { get; set; }
    }
}
