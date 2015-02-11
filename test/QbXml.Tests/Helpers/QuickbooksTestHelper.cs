namespace QbSync.QbXml.Tests.Helpers
{
    class QuickBooksTestHelper
    {
        public static string CreateQbXmlWithEnvelope(string inner, string rootElementName)
        {
            var str = string.Format(@"<?xml version=""1.0"" ?><QBXML><QBXMLMsgsRs><" + rootElementName + @" requestID=""1"" statusCode=""0"" statusSeverity=""Info"" statusMessage=""Status OK"">{0}</" + rootElementName + @"></QBXMLMsgsRs></QBXML>", inner);
            return str;
        }
    }
}
