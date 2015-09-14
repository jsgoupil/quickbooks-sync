using System;
using System.Globalization;
using System.Xml.Serialization;

namespace QbSync.QbXml.Objects
{
    public partial class DATETYPE : ITypeWrapper, IComparable<DATETYPE>, IXmlSerializable
    {
        protected DateTime value;

        public DATETYPE()
        {
        }

        public DATETYPE(string value)
        {
            this.value = DateTime.ParseExact(value, "yyyy-MM-dd", CultureInfo.InvariantCulture);
        }

        public DATETYPE(DateTime value)
        {
            this.value = value;
        }

        public override string ToString()
        {
            return value.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
        }

        public DateTime ToDateTime()
        {
            return value;
        }

        public override bool Equals(object obj)
        {
            var objType = obj as DATETYPE;
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

        public static bool operator ==(DATETYPE a, DATETYPE b)
        {
            // If both are null, or both are same instance, return true.
            if (System.Object.ReferenceEquals(a, b))
            {
                return true;
            }

            // If one is null, but not both, return false.
            if (((object)a == null) ^ ((object)b == null))
            {
                return false;
            }

            return a.Equals(b);
        }

        public static bool operator !=(DATETYPE a, DATETYPE b)
        {
            return !(a == b);
        }

        public static implicit operator DATETYPE(DateTime value)
        {
            return new DATETYPE(value);
        }

        public static implicit operator DateTime(DATETYPE type)
        {
            if (type != null)
            {
                return type.ToDateTime();
            }

            return default(DateTime);
        }

        public int CompareTo(DATETYPE other)
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
                value = DateTime.Parse(reader.ReadContentAsString());
                reader.ReadEndElement();
            }
        }

        public void WriteXml(System.Xml.XmlWriter writer)
        {
            writer.WriteString(ToString());
        }
    }
}
