using QbSync.QbXml.Struct;
using QbSync.QbXml.Type;

namespace QbSync.QbXml.Objects
{
    public class DataExtDefDelRet
    {
        public GuidType OwnerID { get; set; }
        public StrType DataExtName { get; set; }

        public DateTimeType TimeDeleted { get; set; }
    }
}
