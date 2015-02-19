using QbSync.QbXml.Extensions;
using QbSync.QbXml.Type;
using System.Xml;

namespace QbSync.QbXml.Objects
{
    public class AddressBlock : IXmlConvertible
    {
        public StrType Addr1 { get; set; }
        public StrType Addr2 { get; set; }
        public StrType Addr3 { get; set; }
        public StrType Addr4 { get; set; }
        public StrType Addr5 { get; set; }

        public virtual void AppendXml(XmlElement parent)
        {
            parent.AppendTagIfNotNull("Addr1", Addr1);
            parent.AppendTagIfNotNull("Addr2", Addr2);
            parent.AppendTagIfNotNull("Addr3", Addr3);
            parent.AppendTagIfNotNull("Addr4", Addr4);
            parent.AppendTagIfNotNull("Addr5", Addr5);
        }
    }
}
