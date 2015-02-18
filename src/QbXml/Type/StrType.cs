using System;

namespace QbSync.QbXml.Type
{
    public class StrType : IStringConvertible, IComparable<StrType>
    {
        private string value;

        public StrType(string value)
        {
            this.value = value;
        }

        public override string ToString()
        {
            // TODO QbSync to we need to encode?
            return value;
        }

        public override bool Equals(object obj)
        {
            var objType = obj as StrType;
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

        public static bool operator ==(StrType a, StrType b)
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

        public static bool operator !=(StrType a, StrType b)
        {
            return !(a == b);
        }

        public static implicit operator StrType(string value)
        {
            if (value == null)
            {
                return null;
            }

            return new StrType(value);
        }

        public static implicit operator string(StrType type)
        {
            if (type != null)
            {
                return type.ToString();
            }

            return default(string);
        }

        public int CompareTo(StrType other)
        {
            return this.value.CompareTo(other.value);
        }
    }
}
