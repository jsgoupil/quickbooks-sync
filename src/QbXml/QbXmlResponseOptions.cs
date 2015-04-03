using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QbSync.QbXml
{
    public class QbXmlResponseOptions
    {
        /// <summary>
        /// Used to fix all dates provided by QuickBooks.
        /// The Date in QuickBooks are not correct when the DST is in effect on the
        /// client computer. If the date returned by QuickBooks is within a DST range,
        /// we will substract the offset appropriately.
        /// </summary>
        public TimeZoneInfo TimeZoneBugFix { get; set; }
    }
}
