using System;
using System.Globalization;
using System.Xml.Serialization;

namespace QbSync.QbXml.Objects
{
    /// <summary>
    /// Represents a date.
    /// </summary>
    public partial class DATETYPE : ITypeWrapper, IComparable<DATETYPE>, IXmlSerializable
    {
        private DateTime _value;

        /// <summary>
        /// Creates a DATETYPE class.
        /// </summary>
        public DATETYPE()
        {
        }

        /// <summary>
        /// Creates an DATETYPE class and parse the value as a date.
        /// </summary>
        /// <param name="value">A date.</param>
        public DATETYPE(string value)
            : this(Parse(value))
        {
        }

        /// <summary>
        /// Creates an DATETYPE class with a value as a date.
        /// </summary>
        /// <param name="value">A date.</param>
        public DATETYPE(DateTime value)
        {
            this._value = value;
        }

        /// <summary>
        /// Gets the original raw string value parsed from QuickBooks. Will be null if not deserialized from XML.
        /// </summary>
        public string? QuickBooksRawString { get; private set; }

        /// <summary>
        /// Gets the number of ticks for the date.
        /// </summary>
        public long Ticks => _value.Ticks;

        /// <summary>
        /// Gets the year component of the date (1970-2038).
        /// </summary>
        public int Year => _value.Year;

        /// <summary>
        /// Gets the month component of the date (1-12).
        /// </summary>
        public int Month => _value.Month;

        /// <summary>
        /// Gets the day of month component of the date (1-31).
        /// </summary>
        public int Day => _value.Day;

        /// <summary>
        /// A string representation of the date.
        /// </summary>
        /// <returns>Date in yyyy-MM-dd format.</returns>
        public override string ToString()
        {
            return _value.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Gets the DateTime.
        /// </summary>
        /// <returns>DateTime.</returns>
        public DateTime ToDateTime()
        {
            return _value;
        }

        /// <summary>
        /// Compares two DATETYPEs.
        /// </summary>
        /// <param name="obj">A DATETYPE.</param>
        /// <returns>True on equality.</returns>
        public override bool Equals(object? obj)
        {
            if (obj is DATETYPE objType)
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
        /// Compares two DATETYPEs.
        /// </summary>
        /// <param name="a">Operand 1.</param>
        /// <param name="b">Operand 2.</param>
        /// <returns>True on equality.</returns>
        public static bool operator ==(DATETYPE? a, DATETYPE? b)
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
        /// Compares two DATETYPEs.
        /// </summary>
        /// <param name="a">Operand 1.</param>
        /// <param name="b">Operand 2.</param>
        /// <returns>True on equality.</returns>
        public static bool operator !=(DATETYPE? a, DATETYPE? b)
        {
            return !(a == b);
        }

        /// <summary>
        /// Converts a DateTime to DATETYPE automatically.
        /// </summary>
        /// <param name="value">A DATETYPE.</param>
        public static implicit operator DATETYPE(DateTime value)
        {
            return new DATETYPE(value);
        }

        /// <summary>
        /// Converts a DATETYPE to DateTime automatically.
        /// </summary>
        /// <param name="value">A DateTime.</param>
        public static implicit operator DateTime(DATETYPE? value)
        {
            if (value != null)
            {
                return value.ToDateTime();
            }

            return default;
        }

        /// <summary>
        /// Compares to another DATETYPE.
        /// </summary>
        /// <param name="other">Another DATETYPE.</param>
        /// <returns>True if equals.</returns>
        public int CompareTo(DATETYPE? other)
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
                QuickBooksRawString = reader.ReadContentAsString();
                _value = Parse(QuickBooksRawString);
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

        private static DateTime Parse(string value)
        {
            if (DateTime.TryParse(value, out DateTime output))
            {
                return output;
            }

            return DateTime.MinValue;
        }
    }
}