using QbSync.QbXml.Extensions;
using QbSync.QbXml.Type;
using System.Xml;

namespace QbSync.QbXml.Objects
{
    public class Ref : IXmlConvertible
    {
        public IdType ListID { get; set; }
        public StrType FullName { get; set; }

        public virtual void AppendXml(XmlElement parent)
        {
            if (ListID != null)
            {
                parent.AppendTag("ListID", ListID);
            }

            if (FullName != null)
            {
                parent.AppendTag("FullName", FullName);
            }
        }
    }
}