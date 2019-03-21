using QbSync.QbXml.Objects;

namespace WebApplication.Sample.Extensions
{
    public static class DATETIMETYPEExtensions
    {
        public static DATETIMETYPE GetQueryFromModifiedDate(this DATETIMETYPE dateTimeType)
        {
            if (dateTimeType == null)
            {
                return DATETIMETYPE.MinValue;
            }

            return new DATETIMETYPE(dateTimeType.ToDateTime().AddSeconds(1));
        }
    }
}
