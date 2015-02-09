using QbSync.QbXml.Type;
using QbSync.QuickbooksDesktopSync.Extensions;
using System.Xml;

namespace QbSync.QbXml.Filters
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
