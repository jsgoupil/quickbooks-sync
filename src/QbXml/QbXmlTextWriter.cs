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
            base.WriteRaw(htmlEncodeSpecialCharacters(text));
        }

        private string htmlEncodeSpecialCharacters(string text)
        {
            text = HttpUtility.HtmlEncode(text);
            StringBuilder sb = new StringBuilder();
            foreach (char c in text)
            {
                if (c > 127)
                {
                    sb.Append(string.Format("&#{0};", (int)c));
                }
                else
                {
                    sb.Append(c);
                }
            }

            return sb.ToString();
        }
    }
}