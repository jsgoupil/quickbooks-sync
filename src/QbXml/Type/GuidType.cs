using System;
using System.Globalization;

namespace QbSync.QbXml.Type
{
    public class GuidType : ITypeWrapper, IComparable<GuidType>
    {
        private Guid value;

        public GuidType()
        {
            this.value = Guid.Empty;
        }

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

        public static implicit operator Guid(GuidType value)
        {
            if (value != null)
            {
                return value.ToGuid();
            }

            return default(Guid);
        }

        public static implicit operator GuidType(string value)
        {
            if (value != null)
            {
                return new GuidType(value);
            }

            return default(GuidType);
        }

        public static implicit operator string(GuidType value)
        {
            if (value != null)
            {
                return value.ToString();
            }

            return default(string);
        }

        public int CompareTo(GuidType other)
        {
            return this.value.CompareTo(other.value);
        }
    }
}
