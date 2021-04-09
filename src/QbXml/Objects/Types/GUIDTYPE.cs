using System;
using System.Globalization;
using System.Xml.Serialization;

namespace QbSync.QbXml.Objects
{
    /// <summary>
    /// Represents a GUID.
    /// </summary>
    public partial class GUIDTYPE : ITypeWrapper, IComparable<GUIDTYPE>, IXmlSerializable
    {
        private Guid _value;
        private bool _isZero;

        /// <summary>
        /// Creates a GUIDTYPE class.
        /// </summary>
        public GUIDTYPE()
        {
            this._value = Guid.Empty;
        }

        /// <summary>
        /// Creates an GUIDTYPE class and parse the value as a GUID.
        /// You can pass in "0" to satisfy QuickBooks 0-GUID.
        /// </summary>
        /// <param name="value">A GUID.</param>
        public GUIDTYPE(string value)
        {
            this._value = Parse(value);
            if (value == "0")
            {
                _isZero = true;
            }
        }

        /// <summary>
        /// Creates an GUIDTYPE class with a value as a GUID.
        /// </summary>
        /// <param name="value">A GUID.</param>
        public GUIDTYPE(Guid value)
        {
            this._value = value;
        }

        /// <summary>
        /// A string representation of the GUID or "0".
        /// </summary>
        /// <returns>A GUID in the B format or "0".</returns>
        public override string ToString()
        {
            if (_isZero && _value == Guid.Empty)
            {
                return "0";
            }

            return _value.ToString("B", CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Gets the GUID.
        /// </summary>
        /// <returns>GUID.</returns>
        public Guid ToGuid()
        {
            return _value;
        }

        /// <summary>
        /// Compares two GUIDTYPEs.
        /// </summary>
        /// <param name="obj">A GUIDTYPE.</param>
        /// <returns>True on equality.</returns>
        public override bool Equals(object? obj)
        {
            if (obj is GUIDTYPE objType)
            {
                return _value == objType._value;
            }

            return base.Equals(obj);
        }

        /// <summary>
        /// Gets the HashCode.
        /// </summary>
        /// <returns>HashCode.</returns>
        public override int GetHashCode()
        {
            return _value.GetHashCode();
        }

        /// <summary>
        /// Compares two GUIDTYPEs.
        /// </summary>
        /// <param name="a">Operand 1.</param>
        /// <param name="b">Operand 2.</param>
        /// <returns>True on equality.</returns>
        public static bool operator ==(GUIDTYPE? a, GUIDTYPE? b)
        {
            // If both are null, or both are same instance, return true.
            if (System.Object.ReferenceEquals(a, b))
            {
                return true;
            }

            // If one is null, but not both, return false.
            if ((a is null) || (b is null))
            {
                return false;
            }

            return a.Equals(b);
        }

        /// <summary>
        /// Compares two GUIDTYPEs.
        /// </summary>
        /// <param name="a">Operand 1.</param>
        /// <param name="b">Operand 2.</param>
        /// <returns>True on equality.</returns>
        public static bool operator !=(GUIDTYPE a, GUIDTYPE b)
        {
            return !(a == b);
        }

        /// <summary>
        /// Converts a GUID to GUIDTYPE automatically.
        /// </summary>
        /// <param name="value">A GUIDTYPE.</param>
        public static implicit operator GUIDTYPE(Guid value)
        {
            return new GUIDTYPE(value);
        }

        /// <summary>
        /// Converts a GUIDTYPE to GUID automatically.
        /// </summary>
        /// <param name="value">A GUID.</param>
        public static implicit operator Guid(GUIDTYPE? value)
        {
            if (value is null)
            {
                return default;
            }

            return value.ToGuid();
        }

        /// <summary>
        /// Converts a string to GUID automatically.
        /// The string can be "0".
        /// </summary>
        /// <param name="value">A GUIDTYPE.</param>
        public static implicit operator GUIDTYPE?(string value)
        {
            if (value != null)
            {
                return new GUIDTYPE(value);
            }

            return default;
        }

        /// <summary>
        /// Converts the GUIDTYPE to a string automatically.
        /// </summary>
        /// <param name="value">A GUID or "0").</param>
        public static implicit operator string?(GUIDTYPE value)
        {
            if (value is null)
            {
                return default;
            }

            return value.ToString();
        }

        /// <summary>
        /// Compares to another GUIDTYPE.
        /// </summary>
        /// <param name="other">Another GUIDTYPE.</param>
        /// <returns>True if equals.</returns>
        public int CompareTo(GUIDTYPE? other)
        {
            return this._value.CompareTo(other?._value);
        }

        /// <summary>
        /// Returns null.
        /// </summary>
        /// <returns>Null.</returns>
        public System.Xml.Schema.XmlSchema? GetSchema()
        {
            return null;
        }

        /// <summary>
        /// Reads the XML and populate the inner value.
        /// </summary>
        /// <param name="reader">XmlReader.</param>
        public void ReadXml(System.Xml.XmlReader reader)
        {
            reader.MoveToContent();
            var isEmptyElement = reader.IsEmptyElement;
            reader.ReadStartElement();
            if (!isEmptyElement)
            {
                var str = reader.ReadContentAsString();
                _isZero = str == "0";
                _value = Parse(str);

                reader.ReadEndElement();
            }
        }

        /// <summary>
        /// Writes the XML from the inner value.
        /// </summary>
        /// <param name="writer">XmlWriter.</param>
        public void WriteXml(System.Xml.XmlWriter writer)
        {
            writer.WriteString(ToString());
        }

        private static Guid Parse(string value)
        {
            if (Guid.TryParse(value, out Guid output))
            {
                return output;
            }

            return Guid.Empty;
        }
    }
}
