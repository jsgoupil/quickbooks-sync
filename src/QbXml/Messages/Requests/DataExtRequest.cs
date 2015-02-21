using QbSync.QbXml.Objects;
using QbSync.QbXml.Type;

namespace QbSync.QbXml.Messages.Requests
{
    public abstract class DataExtRequest<T> : QbXmlObject<T>
        where T : new()
    {
        public GuidType OwnerID { get; set; }
        public string DataExtName { get; set; }

        public ListDataExtType? ListDataExtType { get; set; }
        public ListObjRef ListObjRef { get; set; }
        public OtherDataExtType? OtherDataExtType { get; set; }
        public TxnDataExtType? TxnDataExtType { get; set; }
        public string TxnLineID { get; set; }
    }
}