using QbSync.QbXml.Struct;
using QbSync.QbXml.Type;

namespace QbSync.QbXml.Objects
{
    public class DataExtRet
    {
        public GuidType OwnerID { get; set; }
        public StrType DataExtName { get; set; }
        public DataExtType DataExtType { get; set; }
        public StrType DataExtValue { get; set; }
    }
}
