using System.Xml;

namespace QbSync.QbXml.Type
{
    public class QuickBooksType : IStringConvertible, IXmlConvertible
    {
        public void AppendXml(XmlElement parent)
        {
            parent.InnerText = ToString();
        }
    }
}
