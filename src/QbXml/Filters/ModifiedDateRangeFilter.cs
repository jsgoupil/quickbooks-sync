using QbSync.QbXml.Extensions;
using QbSync.QbXml.Type;
using System.Xml;

namespace QbSync.QbXml.Filters
{
    public class ModifiedDateRangeFilter
    {
        public DateTimeType FromModifiedDate { get; set; }
        public DateTimeType ToModifiedDate { get; set; }

        public void AppendXml(XmlElement parent)
        {
            if (FromModifiedDate != null)
            {
                parent.AppendTag("FromModifiedDate", FromModifiedDate);
            }

            if (ToModifiedDate != null)
            {
                parent.AppendTag("ToModifiedDate", ToModifiedDate);
            }
        }
    }
}
