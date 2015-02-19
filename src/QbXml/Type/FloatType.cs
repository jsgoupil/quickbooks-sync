using System;
using System.Globalization;

namespace QbSync.QbXml.Type
{
    public class PercentType : IStringConvertible, IComparable<PercentType>
    {
        private decimal value;

        public PercentType(string value)
        {
            this.value = Decimal.Parse(value);
        }

        public PercentType(decimal value)
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
            var objType = obj as PercentType;
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

        public static bool operator ==(PercentType a, PercentType b)
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

        public static bool operator !=(PercentType a, PercentType b)
        {
            return !(a == b);
        }

        public static implicit operator PercentType(decimal value)
        {
            return new PercentType(value);
        }

        public static implicit operator decimal(PercentType type)
        {
            if (type != null)
            {
                return type.ToDecimal();
            }

            return default(decimal);
        }

        public int CompareTo(PercentType other)
        {
            return this.value.CompareTo(other.value);
        }
    }
}
