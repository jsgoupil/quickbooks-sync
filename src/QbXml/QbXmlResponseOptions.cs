using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QbSync.QbXml.Objects;

namespace QbSync.QbXml
{
    public class QbXmlResponseOptions
    {
        /// <summary>
        /// Used to interpret UTC offsets on <see cref="DATETIMETYPE"/> values returned from QuickBooks.
        /// Set this value to the time zone of the QuickBooks host machine so UTC offsets returned
        /// from QuickBooks will be corrected. If null, returned offsets will be ignored.
        ///
        /// If a <see cref="TimeZoneBugFix"/> zone is specified, offsets may still be ignored if
        /// the zone is not accurate for the QuickBooks host machine. Offsets returned from
        /// QuickBooks requiring more correction than the max <see cref="TimeZoneInfo.AdjustmentRule.DaylightDelta"/>
        /// for the adjustment rules for that zone are ignored.
        /// </summary>
        /// <remarks>
        /// The UTC offset portion of dates returned from QuickBooks do not follow
        /// DST rules, and always follow the "Base Offset" for the zone.
        /// Without knowing the time zone of the client computer QuickBooks
        /// is running on, the offset can not be accurately interpreted. 
        /// </remarks>
        public TimeZoneInfo TimeZoneBugFix { get; set; }
    }
}
