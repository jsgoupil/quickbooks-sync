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
        /// Used to interpret the UTC offset of <see cref="DATETIMETYPE"/> values returned in QuickBooks responses.
        /// When [accurately] set, <see cref="DATETIMETYPE.Offset"/> will be populated for <see cref="DATETIMETYPE"/>
        /// values returned from QuickBooks, and <see cref="DATETIMETYPE.GetDateTimeOffset"/> can be used.
        ///
        /// If a <see cref="TimeZoneInfo"/> is not set, or the <see cref="TimeZoneInfo.BaseUtcOffset"/> does not match the
        /// values returned from QuickBooks, the UTC <see cref="DATETIMETYPE.Offset"/> will be null, and the date
        /// cannot be converted to UTC or a UTC based value. 
        /// </summary>
        /// <remarks>
        /// The reason this option exists is because of the issue that QuickBooks has with regards to handling DST.
        /// The Date & time components of returned dates follow DST rules, but the offset always represents standard time.
        /// This means during daylight saving time, the offset will be an hour off (for most DST enabled time zones at least).
        /// Because  different time zones have different DST deltas, and change to/from DST at different times, the
        /// actual offset for a date cannot be determined unless the time zone of the host computer is known.
        ///
        /// However, because offsets are optional in QuickBooks date range queries, we can use the values as returned from QuickBooks
        /// with the offset component removed, and subsequent requests to QuickBooks with this value will work as expected
        /// because QuickBooks will interpret the date & time as being in the local zone of its host computer.
        /// </remarks>
        public TimeZoneInfo QuickBooksDesktopTimeZone { get; set; }

        /// <summary>
        /// While the QuickBooks bug is still an issue, how it is handled has slightly changed so
        /// implementing this fix can be optional now. In turn, this method has been renamed
        /// to better match the updated implementation. 
        /// </summary>
        [Obsolete("This has been renamed to QuickBooksDesktopTimeZone")]
        public TimeZoneInfo TimeZoneBugFix
        {
            get => QuickBooksDesktopTimeZone;
            set => QuickBooksDesktopTimeZone = value;
        }
    }
}