using System;
using System.Globalization;

namespace QbSync.QbXml.Type
{
    public class DateTimeType : IStringConvertible
    {
        private DateTime value;

        public DateTimeType(string value)
        {
            this.value = DateTime.Parse(value);
        }

        public DateTimeType(DateTime value)
        {
            this.value = value;
        }

        public override string ToString()
        {
            return value.ToString("s", CultureInfo.InvariantCulture); // TODO QbSync confirm timezone
        }

        public DateTime ToDateTime()
        {
            return value;
        }

        public static implicit operator DateTimeType(DateTime value)
        {
            return new DateTimeType(value);
        }

        public static implicit operator DateTime(DateTimeType type)
        {
            if (type != null)
            {
                return type.ToDateTime();
            }

            return default(DateTime);
        }
    }
}