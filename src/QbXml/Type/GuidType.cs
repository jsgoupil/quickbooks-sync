using System;
using System.Globalization;

namespace QbSync.QbXml.Type
{
    public class GuidType : QuickBooksType, IStringConvertible, IComparable<GuidType>
    {
        private Guid value;

        public GuidType(string value)
        {
            this.value = Guid.Parse(value);
        }

        public GuidType(Guid value)
        {
            this.value = value;
        }

        public override string ToString()
        {
            return value.ToString("B", CultureInfo.InvariantCulture);
        }

        public Guid ToGuid()
        {
            return value;
        }

        public override bool Equals(object obj)
        {
            var objType = obj as GuidType;
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

        public static bool operator ==(GuidType a, GuidType b)
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

        public static bool operator !=(GuidType a, GuidType b)
        {
            return !(a == b);
        }

        public static implicit operator GuidType(Guid value)
        {
            return new GuidType(value);
        }

        public static implicit operator Guid(GuidType type)
        {
            if (type != null)
            {
                return type.ToGuid();
            }

            return default(Guid);
        }

        public int CompareTo(GuidType other)
        {
            return this.value.CompareTo(other.value);
        }
    }
}
