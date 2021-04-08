using QbSync.QbXml.Objects;
using System;

namespace WebApplication.Sample.Extensions
{
    public static class DATETIMETYPEExtensions
    {
        public static DATETIMETYPE GetQueryFromModifiedDate(this DATETIMETYPE? dateTimeType)
        {
            if (dateTimeType == null)
            {
                return DATETIMETYPE.MinValue;
            }

            return dateTimeType.Add(TimeSpan.FromSeconds(1));
        }

        public static DateTimeOffset GetCorrectedDate(this DATETIMETYPE date, TimeZoneInfo quickBooksTimeZone)
        {
            var dateTime = date.ToDateTime();
            var correctedOffset = quickBooksTimeZone.GetUtcOffset(dateTime);
            return new DateTimeOffset(dateTime, correctedOffset);
        }
    }
}
