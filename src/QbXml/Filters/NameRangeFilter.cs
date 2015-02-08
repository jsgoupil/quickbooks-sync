using QBSync.QbXml.Type;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using QBSync.QuickbooksDesktopSync.Extensions;

namespace QBSync.QbXml.Filters
{
    public class NameRangeFilter
    {
        public StrType FromName
        {
            get;
            set;
        }

        public StrType ToName
        {
            get;
            set;
        }

        public void AppendXml(XmlElement parent)
        {
            if (FromName != null)
            {
                parent.AppendTag("FromName", FromName);
            }

            if (ToName != null)
            {
                parent.AppendTag("ToName", ToName);
            }
        }
    }
}
