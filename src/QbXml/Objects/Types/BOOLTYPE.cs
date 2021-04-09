using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Xml.Serialization;

namespace QbSync.QbXml.Objects
{
    /// <summary>
    /// Represents a boolean.
    /// </summary>
    public partial class BOOLTYPE : ITypeWrapper, IComparable<BOOLTYPE>, IXmlSerializable
    {
        /// <summary>
        /// The inner value.
        /// </summary>
        protected bool _value;

        /// <summary>
        /// Creates a BOOLTYPE class.
        /// </summary>
        public BOOLTYPE()
        {
        }

        /// <summary>
        /// Creates an BOOLTYPE class and parse the value as a boolean.
        /// </summary>
        /// <param name="value">A boolean.</param>
        public BOOLTYPE(string value)
            : this(Parse(value))
        {
        }

        /// <summary>
        /// Creates an BOOLTYPE class with a value as a boolean.
        /// </summary>
        /// <param name="value">A boolean.</param>
        public BOOLTYPE(bool value)
        {
            this._value = value;
        }

        /// <summary>
        /// Returns the string representation of the boolean, lowercase.
        /// </summary>
        /// <returns>true or false as a string.</returns>
        public override string ToString()
        {
            return _value.ToString(CultureInfo.InvariantCulture).ToLowerInvariant();
        }

        /// <summary>
        /// Gets the boolean.
        /// </summary>
        /// <returns>Boolean.</returns>
        public bool ToBoolean()
        {
            return _value;
        }

        /// <summary>
        /// Compares two BOOLTYPEs.
        /// </summary>
        /// <param name="obj">A BOOLTYPE.</param>
        /// <returns>True on equality.</returns>
        public override bool Equals(object? obj)
        {
            if (obj is BOOLTYPE objType)
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
        /// Compares two BOOLTYPEs.
        /// </summary>
        /// <param name="a">Operand 1.</param>
        /// <param name="b">Operand 2.</param>
        /// <returns>True on equality.</returns>
        public static bool operator ==(BOOLTYPE? a, BOOLTYPE? b)
        {
            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(a, b))
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
        /// Compares two BOOLTYPEs.
        /// </summary>
        /// <param name="a">Operand 1.</param>
        /// <param name="b">Operand 2.</param>
        /// <returns>True on equality.</returns>
        public static bool operator !=(BOOLTYPE? a, BOOLTYPE? b)
        {
            return !(a == b);
        }

        /// <summary>
        /// Converts a boolean to BOOLTYPE automatically.
        /// </summary>
        /// <param name="value">A BOOLTYPE.</param>
        public static implicit operator BOOLTYPE(bool value)
        {
            return new BOOLTYPE(value);
        }

        /// <summary>
        /// Converts a BOOLTYPE to boolean automatically.
        /// </summary>
        /// <param name="value">A boolean.</param>
        public static implicit operator bool(BOOLTYPE? value)
        {
            if (value is null)
            {
                return default;
            }

            return value.ToBoolean();
        }

        /// <summary>
        /// Compares to another BOOLTYPE.
        /// </summary>
        /// <param name="other">Another BOOLTYPE.</param>
        /// <returns>True if equals.</returns>
        public int CompareTo(BOOLTYPE? other)
        {
            return _value.CompareTo(other?._value);
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
                _value = Parse(reader.ReadContentAsString());
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

        private static bool Parse(string value)
        {
            if (bool.TryParse(value, out bool output))
            {
                return output;
            }

            return false;
        }
    }
}
