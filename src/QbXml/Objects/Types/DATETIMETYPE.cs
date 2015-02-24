using System;
using System.Globalization;
using System.Xml.Serialization;

namespace QbSync.QbXml.Objects
{
    public partial class DATETIMETYPE : ITypeWrapper, IComparable<DATETIMETYPE>, IXmlSerializable
    {
        protected DateTime value;

        public DATETIMETYPE()
        {
        }

        public DATETIMETYPE(string value)
        {
            this.value = DateTime.Parse(value);
        }

        public DATETIMETYPE(DateTime value)
        {
            this.value = value;
        }

        public override string ToString()
        {
            var k = value.ToString(" K", CultureInfo.InvariantCulture).Trim();

            // QuickBooks doesn't support Z format.
            if (k == "Z")
            {
                k = string.Empty;
            }

            return value.ToString("yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture) + k;
        }

        public DateTime ToDateTime()
        {
            return value;
        }

        public override bool Equals(object obj)
        {
            var objType = obj as DATETIMETYPE;
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

        public static bool operator ==(DATETIMETYPE a, DATETIMETYPE b)
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

        public static bool operator !=(DATETIMETYPE a, DATETIMETYPE b)
        {
            return !(a == b);
        }

        public static implicit operator DATETIMETYPE(DateTime value)
        {
            return new DATETIMETYPE(value);
        }

        public static implicit operator DateTime(DATETIMETYPE type)
        {
            if (type != null)
            {
                return type.ToDateTime();
            }

            return default(DateTime);
        }

        public int CompareTo(DATETIMETYPE other)
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