using System;
using System.Globalization;

namespace QbSync.QbXml.Type
{
    public class FloatType : QuickBooksType, IStringConvertible, IComparable<FloatType>
    {
        private decimal value;

        public FloatType(string value)
        {
            this.value = Decimal.Parse(value);
        }

        public FloatType(decimal value)
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
            var objType = obj as FloatType;
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

        public static bool operator ==(FloatType a, FloatType b)
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

        public static bool operator !=(FloatType a, FloatType b)
        {
            return !(a == b);
        }

        public static implicit operator FloatType(decimal value)
        {
            return new FloatType(value);
        }

        public static implicit operator decimal(FloatType type)
        {
            if (type != null)
            {
                return type.ToDecimal();
            }

            return default(decimal);
        }

        public int CompareTo(FloatType other)
        {
            return this.value.CompareTo(other.value);
        }
    }
}
