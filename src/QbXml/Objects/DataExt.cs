using QbSync.QbXml.Type;

namespace QbSync.QbXml.Objects
{
    public class DataExt
    {
        public GuidType OwnerID { get; set; }
        public StrType DataExtName { get; set; }
        public DataType DataExtType { get; set; }
        public StrType DataExtValue { get; set; }
    }
}