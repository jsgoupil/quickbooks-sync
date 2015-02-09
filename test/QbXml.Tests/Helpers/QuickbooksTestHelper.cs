using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QbSync.QuickbooksDesktopSync.Tests.Helpers
{
    class QuickbooksTestHelper
    {
        public static string CreateQbXmlWithEnvelope(string inner)
        {
            var str = string.Format(@"<?xml version=""1.0"" ?><QBXML><QBXMLMsgsRs><CustomerQueryRs requestID=""1"" statusCode=""0"" statusSeverity=""Info"" statusMessage=""Status OK"">{0}</CustomerQueryRs></QBXMLMsgsRs></QBXML>", inner);
            return str;
        }
    }
}
