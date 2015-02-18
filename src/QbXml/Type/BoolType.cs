using System;
using System.Globalization;

namespace QbSync.QbXml.Type
{
    public class BoolType : IStringConvertible, IComparable<BoolType>
    {
        private bool value;

        public BoolType(string value)
        {
            this.value = Boolean.Parse(value);
        }

        public BoolType(bool value)
        {
            this.value = value;
        }

        public override string ToString()
        {
            return value.ToString(CultureInfo.InvariantCulture).ToLowerInvariant();
        }

        public bool ToBoolean()
        {
            return value;
        }

        public override bool Equals(object obj)
        {
            var objType = obj as BoolType;
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

        public static bool operator ==(BoolType a, BoolType b)
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

        public static bool operator !=(BoolType a, BoolType b)
        {
            return !(a == b);
        }

        public static implicit operator BoolType(bool value)
        {
            return new BoolType(value);
        }

        public static implicit operator bool(BoolType type)
        {
            if (type != null)
            {
                return type.ToBoolean();
            }

            return default(bool);
        }

        public int CompareTo(BoolType other)
        {
            return this.value.CompareTo(other.value);
        }
    }
}
