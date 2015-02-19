using QbSync.QbXml.Extensions;
using QbSync.QbXml.Type;
using System.Xml;

namespace QbSync.QbXml.Objects
{
    public class DataExtBase : IXmlConvertible
    {
        public GuidType OwnerID { get; set; }
        public StrType DataExtName { get; set; }
        public StrType DataExtValue { get; set; }

        public virtual void AppendXml(XmlElement parent)
        {
            parent.AppendTag("OwnerID", OwnerID);
            parent.AppendTag("DataExtName", DataExtName);
            parent.AppendTag("DataExtValue", DataExtValue);
        }
    }
}
