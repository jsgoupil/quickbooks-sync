using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QbSync.WebConnector.Tests.Helpers
{
    class QuickBooksTestHelper
    {
        public static TimeZoneInfo GetPacificStandardTimeZoneInfo()
        {
            // From Windows 8.1 Pro
            return TimeZoneInfo.FromSerializedString("Pacific Standard Time;-480;(UTC-08:00) Pacific Time (US & Canada);Pacific Standard Time;Pacific Daylight Time;[01:01:0001;12:31:2006;60;[0;02:00:00;4;1;0;];[0;02:00:00;10;5;0;];][01:01:2007;12:31:9999;60;[0;02:00:00;3;2;0;];[0;02:00:00;11;1;0;];];");
        }
    }
}
