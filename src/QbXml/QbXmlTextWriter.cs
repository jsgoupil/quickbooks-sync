using System.IO;
using System.Text;
using System.Web;
using System.Xml;

namespace QbSync.QbXml
{
    internal class QbXmlTextWriter : XmlTextWriter
    {
        public QbXmlTextWriter(TextWriter textWriter)
            : base(textWriter)
        {
        }

        public override void WriteString(string? text)
        {
            if (text != null)
            {
               base.WriteRaw(HtmlEncodeSpecialCharacters(text));
            }
        }

        private string HtmlEncodeSpecialCharacters(string text)
        {
            text = HttpUtility.HtmlEncode(text);
            var sb = new StringBuilder();
            foreach (char c in text)
            {
                if (c > 127)
                {
                    sb.Append($"&#{(int) c};");
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