using QbSync.QbXml.Type;
using System.Xml;
using QbSync.QbXml.Extensions;

namespace QbSync.QbXml.Objects
{
    public class Ref
    {
        public IdType ListID { get; set; }
        public StrType FullName { get; set; }

        public void AppendXml(XmlElement parent)
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