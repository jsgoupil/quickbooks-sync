using System;
using System.Globalization;

namespace QbSync.QbXml.Type
{
    public class DateTimeType : ITypeWrapper, IComparable<DateTimeType>
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
            var k = value.ToString(" K", CultureInfo.InvariantCulture).Trim();

            // QuickBooks doesn't support Z format.
            if (k == "Z")
            {
                k = string.Empty;
            }

            return value.ToString("yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture) + k;
        }

        public DateTime ToDateTime()
        {
            return value;
        }

        public override bool Equals(object obj)
        {
            var objType = obj as DateTimeType;
            if (objType != null)
            {
                return value == objType.value;
            }

            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return value.GetHashCode();
        }

        public static bool operator ==(DateTimeType a, DateTimeType b)
        {
            // If both are null, or both are same instance, return true.
            if (System.Object.ReferenceEquals(a, b))
            {
                return true;
            }

            // If one is null, but not both, return false.
            if (((object)a == null) || ((object)b == null))
            {
                return false;
            }

            return a.Equals(b);
        }

        public static bool operator !=(DateTimeType a, DateTimeType b)
        {
            return !(a == b);
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

        public int CompareTo(DateTimeType other)
        {
            return this.value.CompareTo(other.value);
        }
    }
}