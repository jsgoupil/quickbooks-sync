using System;
using System.Globalization;
using System.Xml.Serialization;

namespace QbSync.QbXml.Objects
{
    public partial class BOOLTYPE : ITypeWrapper, IComparable<BOOLTYPE>, IXmlSerializable
    {
        protected bool value;

        public BOOLTYPE()
        {
        }

        public BOOLTYPE(string value)
            : this(Parse(value))
        {
        }

        public BOOLTYPE(bool value)
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
            var objType = obj as BOOLTYPE;
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

        public static bool operator ==(BOOLTYPE a, BOOLTYPE b)
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

        public static bool operator !=(BOOLTYPE a, BOOLTYPE b)
        {
            return !(a == b);
        }

        public static implicit operator BOOLTYPE(bool value)
        {
            return new BOOLTYPE(value);
        }

        public static implicit operator bool(BOOLTYPE type)
        {
            if (type != null)
            {
                return type.ToBoolean();
            }

            return default(bool);
        }

        public int CompareTo(BOOLTYPE other)
        {
            return this.value.CompareTo(other.value);
        }

        public System.Xml.Schema.XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(System.Xml.XmlReader reader)
        {
            reader.MoveToContent();
            var isEmptyElement = reader.IsEmptyElement;
            reader.ReadStartElement();
            if (!isEmptyElement)
            {
                value = Parse(reader.ReadContentAsString());
                reader.ReadEndElement();
            }
        }

        public void WriteXml(System.Xml.XmlWriter writer)
        {
            writer.WriteString(ToString());
        }

        private static bool Parse(string value)
        {
            bool output;
            if (bool.TryParse(value, out output))
            {
                return output;
            }

            return false;
        }
    }
}
