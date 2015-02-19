using System;

namespace QbSync.QbXml.Type
{
    public class IdType : QuickBooksType, IStringConvertible, IComparable<IdType>
    {
        private string value;

        public IdType(string value)
        {
            this.value = value;
        }

        public override string ToString()
        {
            return value;
        }

        public override bool Equals(object obj)
        {
            var objType = obj as IdType;
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

        public static bool operator ==(IdType a, IdType b)
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

        public static bool operator !=(IdType a, IdType b)
        {
            return !(a == b);
        }

        public static implicit operator IdType(string value)
        {
            if (value == null)
            {
                return null;
            }

            return new IdType(value);
        }

        public int CompareTo(IdType other)
        {
            return this.value.CompareTo(other.value);
        }
    }
}
