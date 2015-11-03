using System;
using System.Globalization;
using System.Xml.Serialization;

namespace QbSync.QbXml.Objects
{
    public partial class GUIDTYPE : ITypeWrapper, IComparable<GUIDTYPE>, IXmlSerializable
    {
        protected Guid value;

        public GUIDTYPE()
        {
            this.value = Guid.Empty;
        }

        public GUIDTYPE(string value)
            : this(Parse(value))
        {
        }

        public GUIDTYPE(Guid value)
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
            var objType = obj as GUIDTYPE;
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

        public static bool operator ==(GUIDTYPE a, GUIDTYPE b)
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

        public static bool operator !=(GUIDTYPE a, GUIDTYPE b)
        {
            return !(a == b);
        }

        public static implicit operator GUIDTYPE(Guid value)
        {
            return new GUIDTYPE(value);
        }

        public static implicit operator Guid(GUIDTYPE value)
        {
            if (value != null)
            {
                return value.ToGuid();
            }

            return default(Guid);
        }

        public static implicit operator GUIDTYPE(string value)
        {
            if (value != null)
            {
                return new GUIDTYPE(value);
            }

            return default(GUIDTYPE);
        }

        public static implicit operator string(GUIDTYPE value)
        {
            if (value != null)
            {
                return value.ToString();
            }

            return default(string);
        }

        public int CompareTo(GUIDTYPE other)
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

        private static Guid Parse(string value)
        {
            Guid output;
            if (Guid.TryParse(value, out output))
            {
                return output;
            }

            return Guid.Empty;
        }
    }
}
