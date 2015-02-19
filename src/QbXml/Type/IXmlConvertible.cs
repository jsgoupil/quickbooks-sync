using System.Xml;

namespace QbSync.QbXml.Type
{
    public interface IXmlConvertible
    {
        void AppendXml(XmlElement parent);
    }
}
