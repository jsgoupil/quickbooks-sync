using System;
using System.Globalization;

namespace QbSync.QbXml.Type
{
    public class AmtType : QuickBooksType, IStringConvertible, IComparable<AmtType>
    {
        private decimal value;

        public AmtType(string value)
        {
            this.value = Decimal.Parse(value);
        }

        public AmtType(decimal value)
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
            var objType = obj as AmtType;
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

        public static bool operator ==(AmtType a, AmtType b)
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

        public static bool operator !=(AmtType a, AmtType b)
        {
            return !(a == b);
        }

        public static implicit operator AmtType(decimal value)
        {
            return new AmtType(value);
        }

        public static implicit operator decimal(AmtType type)
        {
            if (type != null)
            {
                return type.ToDecimal();
            }

            return default(decimal);
        }

        public int CompareTo(AmtType other)
        {
            return this.value.CompareTo(other.value);
        }
    }
}
