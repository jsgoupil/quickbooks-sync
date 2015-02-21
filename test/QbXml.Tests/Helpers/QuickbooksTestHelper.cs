using System.Xml;
using System.Xml.Schema;

namespace QbSync.QbXml.Tests.Helpers
{
    class QuickBooksTestHelper
    {
        public static string CreateQbXmlWithEnvelope(string inner, string rootElementName)
        {
            var str = string.Format(@"<?xml version=""1.0"" ?><QBXML><QBXMLMsgsRs><" + rootElementName + @" requestID=""1"" statusCode=""0"" statusSeverity=""Info"" statusMessage=""Status OK"">{0}</" + rootElementName + @"></QBXMLMsgsRs></QBXML>", inner);
            return str;
        }

        public static string GetXmlValidation(string xml)
        {
            var schemas = new XmlSchemaSet();
            schemas.Add(string.Empty, "./Schemas/qbxmltypes130.xsd");
            schemas.Add(string.Empty, "./Schemas/qbxmlso130.xsd");
            schemas.Add(string.Empty, "./Schemas/qbxmlops130.xsd");
            schemas.Add(string.Empty, "./Schemas/qbxml130.xsd");

            var doc = new XmlDocument();
            doc.Schemas.Add(schemas);
            doc.LoadXml(xml);
            var str = string.Empty;
            doc.Validate((o, e) =>
            {
                str += e.Message + System.Environment.NewLine;
            });

            return str;
        }
    }
}
