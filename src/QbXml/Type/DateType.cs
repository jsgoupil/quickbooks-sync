using System;
using System.Globalization;

namespace QbSync.QbXml.Type
{
    public class DateType : IStringConvertible, IComparable<DateType>
    {
        private DateTime value;

        public DateType(string value)
        {
            this.value = DateTime.ParseExact(value, "yyyy-MM-dd", CultureInfo.InvariantCulture);
        }

        public DateType(DateTime value)
        {
            this.value = value;
        }

        public override string ToString()
        {
            return value.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
        }

        public DateTime ToDateTime()
        {
            return value;
        }

        public override bool Equals(object obj)
        {
            var objType = obj as DateType;
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

        public static bool operator ==(DateType a, DateType b)
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

        public static bool operator !=(DateType a, DateType b)
        {
            return !(a == b);
        }

        public static implicit operator DateType(DateTime value)
        {
            return new DateType(value);
        }

        public static implicit operator DateTime(DateType type)
        {
            if (type != null)
            {
                return type.ToDateTime();
            }

            return default(DateTime);
        }

        public int CompareTo(DateType other)
        {
            return this.value.CompareTo(other.value);
        }
    }
}