using System;
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
            schemas.Add(string.Empty, "./Schemas/qbxml130_modified.xsd");

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

        public static TimeZoneInfo GetPacificStandardTimeZoneInfo()
        {
            // From Windows 8.1 Pro
            return TimeZoneInfo.FromSerializedString("Pacific Standard Time;-480;(UTC-08:00) Pacific Time (US & Canada);Pacific Standard Time;Pacific Daylight Time;[01:01:0001;12:31:2006;60;[0;02:00:00;4;1;0;];[0;02:00:00;10;5;0;];][01:01:2007;12:31:9999;60;[0;02:00:00;3;2;0;];[0;02:00:00;11;1;0;];];");
        }
    }
}
