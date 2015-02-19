using QbSync.QbXml.Type;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using QbSync.QbXml.Extensions;

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
