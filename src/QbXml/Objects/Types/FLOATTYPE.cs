using System;
using System.Globalization;
using System.Xml.Serialization;

namespace QbSync.QbXml.Objects
{
    /// <summary>
    /// Represents a float, handle with a decimal.
    /// </summary>
    public partial class FLOATTYPE : ITypeWrapper, IComparable<FLOATTYPE>, IXmlSerializable
    {
        /// <summary>
        /// The internal value.
        /// </summary>
        protected decimal _value;

        /// <summary>
        /// Creates a FLOATTYPE class.
        /// </summary>
        public FLOATTYPE()
        {
        }

        /// <summary>
        /// Creates an FLOATTYPE class and parse the value as a decimal.
        /// </summary>
        /// <param name="value">A decimal.</param>
        public FLOATTYPE(string value)
            : this(Parse(value))
        {
        }

        /// <summary>
        /// Creates an FLOATTYPE class with a value as a decimal.
        /// </summary>
        /// <param name="value">A decimal.</param>
        public FLOATTYPE(decimal value)
        {
            this._value = value;
        }

        /// <summary>
        /// A string representation of the decimal.
        /// </summary>
        /// <returns>Decimal in 10.5 format.</returns>
        public override string ToString()
        {
            return _value.ToString("0.#####", CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Gets the decimal.
        /// </summary>
        /// <returns>decimal.</returns>
        public decimal ToDecimal()
        {
            return _value;
        }

        /// <summary>
        /// Compares two FLOATTYPEs.
        /// </summary>
        /// <param name="obj">A FLOATTYPE.</param>
        /// <returns>True on equality.</returns>
        public override bool Equals(object? obj)
        {
            if (obj is FLOATTYPE objType)
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
        /// Compares two FLOATTYPEs.
        /// </summary>
        /// <param name="a">Operand 1.</param>
        /// <param name="b">Operand 2.</param>
        /// <returns>True on equality.</returns>
        public static bool operator ==(FLOATTYPE? a, FLOATTYPE? b)
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
        /// Compares two FLOATTYPEs.
        /// </summary>
        /// <param name="a">Operand 1.</param>
        /// <param name="b">Operand 2.</param>
        /// <returns>True on equality.</returns>
        public static bool operator !=(FLOATTYPE? a, FLOATTYPE? b)
        {
            return !(a == b);
        }

        /// <summary>
        /// Converts a decimal to FLOATTYPE automatically.
        /// </summary>
        /// <param name="value">A FLOATTYPE.</param>
        public static implicit operator FLOATTYPE(decimal value)
        {
            return new FLOATTYPE(value);
        }

        /// <summary>
        /// Converts a FLOATTYPE to decimal automatically.
        /// </summary>
        /// <param name="value">A decimal.</param>
        public static implicit operator decimal(FLOATTYPE? value)
        {
            if (value != null)
            {
                return value.ToDecimal();
            }

            return default;
        }

        /// <summary>
        /// Compares to another FLOATTYPE.
        /// </summary>
        /// <param name="other">Another FLOATTYPE.</param>
        /// <returns>True if equals.</returns>
        public int CompareTo(FLOATTYPE? other)
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

        private static decimal Parse(string value)
        {
            if (decimal.TryParse(value, out decimal output))
            {
                return output;
            }

            return 0m;
        }
    }
}
