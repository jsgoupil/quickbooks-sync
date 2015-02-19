using QbSync.QbXml.Extensions;
using QbSync.QbXml.Type;
using System.Xml;

namespace QbSync.QbXml.Filters
{
    public class NameRangeFilter : IXmlConvertible
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

        public virtual void AppendXml(XmlElement parent)
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
