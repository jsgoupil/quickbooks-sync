using System;
using System.Globalization;

namespace QbSync.QbXml.Type
{
    public class IntType : QuickBooksType, IStringConvertible, IComparable<IntType>
    {
        private int value;

        public IntType(string value)
        {
            this.value = Int32.Parse(value);
        }

        public IntType(int value)
        {
            this.value = value;
        }

        public override string ToString()
        {
            return value.ToString(CultureInfo.InvariantCulture);
        }

        public int ToInt()
        {
            return value;
        }

        public override bool Equals(object obj)
        {
            var objType = obj as IntType;
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

        public static bool operator ==(IntType a, IntType b)
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

        public static bool operator !=(IntType a, IntType b)
        {
            return !(a == b);
        }

        public static implicit operator IntType(int value)
        {
            return new IntType(value);
        }

        public static implicit operator int(IntType type)
        {
            if (type != null)
            {
                return type.ToInt();
            }

            return default(int);
        }

        public int CompareTo(IntType other)
        {
            return this.value.CompareTo(other.value);
        }
    }
}
