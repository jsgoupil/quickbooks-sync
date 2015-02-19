using QbSync.QbXml.Type;

namespace QbSync.QbXml.Objects
{
    public class DataExtDefDel
    {
        public GuidType OwnerID { get; set; }
        public StrType DataExtName { get; set; }

        public DateTimeType TimeDeleted { get; set; }
    }
}
