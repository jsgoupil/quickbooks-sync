using System.IO;
using System.Text;
using System.Web;
using System.Xml;

namespace QbSync.QbXml
{
    internal class QbXmlTextWriter : XmlTextWriter
    {
        public QbXmlTextWriter(MemoryStream s, Encoding encoding)
            : base(s, encoding)
        {
        }

        public override void WriteString(string text)
        {
            base.WriteRaw(HttpUtility.HtmlEncode(text));
        }
    }
}