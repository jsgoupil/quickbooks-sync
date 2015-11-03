using System;
using System.Globalization;
using System.Xml.Serialization;

namespace QbSync.QbXml.Objects
{
    public partial class FLOATTYPE : ITypeWrapper, IComparable<FLOATTYPE>, IXmlSerializable
    {
        protected decimal value;

        public FLOATTYPE()
        {
        }

        public FLOATTYPE(string value)
            : this(Parse(value))
        {
        }

        public FLOATTYPE(decimal value)
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
            var objType = obj as FLOATTYPE;
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

        public static bool operator ==(FLOATTYPE a, FLOATTYPE b)
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

        public static bool operator !=(FLOATTYPE a, FLOATTYPE b)
        {
            return !(a == b);
        }

        public static implicit operator FLOATTYPE(decimal value)
        {
            return new FLOATTYPE(value);
        }

        public static implicit operator decimal(FLOATTYPE type)
        {
            if (type != null)
            {
                return type.ToDecimal();
            }

            return default(decimal);
        }

        public int CompareTo(FLOATTYPE other)
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

        private static decimal Parse(string value)
        {
            decimal output;
            if (decimal.TryParse(value, out output))
            {
                return output;
            }

            return 0m;
        }
    }
}
