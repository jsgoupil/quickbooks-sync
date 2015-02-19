using QbSync.QbXml.Extensions;
using QbSync.QbXml.Struct;
using QbSync.QbXml.Type;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace QbSync.QbXml.Filters
{
    public class RefNumberRangeFilter
    {
        public StrType FromToRefNumber { get; set; }
        public StrType ToRefNumber { get; set; }

        public void AppendXml(XmlElement parent)
        {
            if (FromToRefNumber != null)
            {
                parent.AppendTag("FromToRefNumber", FromToRefNumber);
            }

            if (ToRefNumber != null)
            {
                parent.AppendTag("ToRefNumber", ToRefNumber);
            }
        }
    }
}
