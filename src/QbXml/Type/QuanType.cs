using System;
using System.Globalization;

namespace QbSync.QbXml.Type
{
    public class QuanType : IStringConvertible, IComparable<QuanType>
    {
        private decimal value;

        public QuanType(string value)
        {
            this.value = Decimal.Parse(value);
        }

        public QuanType(decimal value)
        {
            this.value = value;
        }

        public override string ToString()
        {
            return value.ToString("F", CultureInfo.InvariantCulture);
        }

        public decimal ToDecimal()
        {
            return value;
        }

        public override bool Equals(object obj)
        {
            var objType = obj as QuanType;
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

        public static bool operator ==(QuanType a, QuanType b)
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

        public static bool operator !=(QuanType a, QuanType b)
        {
            return !(a == b);
        }

        public static implicit operator QuanType(decimal value)
        {
            return new QuanType(value);
        }

        public static implicit operator decimal(QuanType type)
        {
            if (type != null)
            {
                return type.ToDecimal();
            }

            return default(decimal);
        }

        public int CompareTo(QuanType other)
        {
            return this.value.CompareTo(other.value);
        }
    }
}
