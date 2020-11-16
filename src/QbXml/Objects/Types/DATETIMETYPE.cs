using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace QbSync.QbXml.Objects
{
    /// <summary>
    /// Represents a datetime.
    /// </summary>
    public partial class DATETIMETYPE : ITypeWrapper, IComparable<DATETIMETYPE>, IXmlSerializable
    {
        private DateTimeOffset _value;
        private bool _hasOffset;
        private bool _isLocal;

        // Private constructor as only xml deserialization should be using this
        private DATETIMETYPE()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DATETIMETYPE"/> class using the specified <see cref="DateTime"/>.
        /// NOTE: The offset from UTC, if any, that will be sent to QuickBooks along with this date depends on the <see cref="DateTime.Kind"/> of the specified <paramref name="value"/>.
        /// <see cref="DateTimeKind"/>.<see cref="DateTimeKind.Utc"/> will send "+00:00".
        /// <see cref="DateTimeKind"/>.<see cref="DateTimeKind.Local"/> will send an offset as determined by the time zone of the machine this application is running on.
        /// <see cref="DateTimeKind"/>.<see cref="DateTimeKind.Unspecified"/> will send no offset, which QuickBooks will interpret to mean the local time of the QuickBooks host computer.
        /// </summary>
        /// <seealso cref="DateTimeKind"/>
        public DATETIMETYPE(DateTime value)
        {
            switch (value.Kind)
            {
                case DateTimeKind.Utc:
                    _value = new DateTimeOffset(value, TimeSpan.Zero);
                    _hasOffset = true;
                    break;
                case DateTimeKind.Local:
                    _value = new DateTimeOffset(value);
                    _hasOffset = true;
                    _isLocal = true;
                    break;
                default:
                    _value = new DateTimeOffset(value, TimeSpan.Zero);
                    break;
            }

            if (this > MaxValue)
            {
                throw new ArgumentOutOfRangeException(nameof(value), value, "The supplied value is greater than the QuickBooks epoch of 2038-01-19T03:14:07+00:00");
            }

            if (this < MinValue)
            {
                throw new ArgumentOutOfRangeException(nameof(value), value, "The supplied value is less than the minimum allowed date of 1970-01-01");
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DATETIMETYPE"/> class using the specified year, month, day, hour, minute, second.
        /// </summary>
        /// <param name="year">The year (1970 - 2037)</param>
        /// <param name="month">The month of year (1 - 12)</param>
        /// <param name="day">The day of month (1 - number of days in month)</param>
        /// <param name="hour">The hour of day (0 - 23)</param>
        /// <param name="minute">The minute of the hour (0-59)</param>
        /// <param name="second">The second of the minute (0-59)</param>
        public DATETIMETYPE(int year, int month, int day, int hour = 0, int minute = 0, int second = 0)
        {
            if (year < 1970)
            {
                throw new ArgumentOutOfRangeException(nameof(year), year, "Year is less than minimum allowed value of 1970");
            }

            _value = new DateTimeOffset(year, month, day, hour, minute, second, TimeSpan.Zero);
            _hasOffset = false;

            if (_value > MaxValue._value)
            {
                throw new ArgumentOutOfRangeException(message: "The created date is greater than the QuickBooks epoch of 2038-01-19T03:14:07+00:00", innerException: null);
            }
        }

        /// <summary>
        /// Gets the original uncorrected raw string value parsed from QuickBooks, including
        /// the potentially incorrect offset value. Will be null if not deserialized from XML.
        /// </summary>
        public string QuickBooksRawString { get; private set; }

        /// <summary>
        /// Gets the uncorrected <see cref="DateTimeOffset"/> value, as parsed from QuickBooks.
        /// During DST, this will appear an hour off (or whatever the DST correction is in the QuickBooks host time zone).
        /// </summary>
        public DateTimeOffset? UncorrectedDate
        {
            get
            {
                if (!_hasOffset)
                {
                    return null;
                }

                return _value;
            }
        }
        
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
        /// Gets the hour component of the date (0-23).
        /// </summary>
        public int Hour => _value.Hour;

        /// <summary>
        /// Gets the minute component of the date (0-59).
        /// </summary>
        public int Minute => _value.Minute;

        /// <summary>
        /// Gets the second component of the date (0-59).
        /// </summary>
        public int Second => _value.Second;

        /// <summary>
        /// Returns a local date formatted string "yyyy-MM-ddTHH:mm:ss" (no offset).
        /// </summary>
        public override string ToString()
        {
            // QuickBooks has inaccurate offset values.
            // The DateTime is otherwise accurate (follows DST), but the offset does not follow DST.
            // To accomodate for this, simply ignore the offset portion of the string, but only when parsing from XML.

            return _value.ToString("yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Returns a date formatted in either yyyy-MM-ddTHH:mm:ss or yyyy-MM-ddTHH:mm:ssK format (offset included if requested and available).
        /// </summary>
        /// <param name="includeUncorrectedOffset">If the uncorrected offset should be included</param>
        public string ToString(bool includeUncorrectedOffset)
        {
            return includeUncorrectedOffset && _hasOffset
                ? _value.ToString("yyyy-MM-ddTHH:mm:ssK", CultureInfo.InvariantCulture)
                : _value.ToString("yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture);
        }
        
        /// <summary>
        /// Gets a <see cref="DateTime"/> value of this date. Will always have an <see cref="DateTimeKind.Unspecified"/> <see cref="DateTimeKind"/>
        /// for instances returned from QuickBooks. <see cref="DateTime.Kind"/> will match input value if constructed from a <see cref="DateTime"/>.
        /// </summary>
        public DateTime ToDateTime()
        {
            if (!_hasOffset)
            {
                return _value.DateTime;
            }

            if (_isLocal)
            {
                return DateTime.SpecifyKind(_value.DateTime, DateTimeKind.Local);
            }

            if (_value.Offset == TimeSpan.Zero)
            {
                return DateTime.SpecifyKind(_value.DateTime, DateTimeKind.Utc);
            }

            return _value.DateTime;
        }

        /// <summary>
        /// Returns a new <see cref="DATETIMETYPE"/> that adds the value of the specified <see cref="TimeSpan"/> to this instance.
        /// </summary>
        /// <param name="value">A positive or negative time interval</param>
        public DATETIMETYPE Add(TimeSpan value)
        {
            return new DATETIMETYPE
            {
                _value = _value.Add(value),
                _hasOffset = _hasOffset,
                _isLocal = _isLocal
            };
        }

        /// <summary>
        /// Compares two DATETIMETYPEs.
        /// </summary>
        /// <param name="obj">A DATETIMETYPE.</param>
        /// <returns>True on equality.</returns>
        public override bool Equals(object obj)
        {
            if (obj is DATETIMETYPE other)
            {
                return _value == other._value && 
                       _hasOffset == other._hasOffset;
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
        public static bool operator ==(DATETIMETYPE a, DATETIMETYPE b)
        {
            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(a, b))
            {
                return true;
            }

            // If one is null, but not both, return false.
            if ((a is null) ^ (b is null))
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
        public static bool operator !=(DATETIMETYPE a, DATETIMETYPE b)
        {
            return !(a == b);
        }

        /// <summary>
        /// Compares if the operand 1 is smaller than the operand 2.
        /// </summary>
        /// <param name="a">Operand 1.</param>
        /// <param name="b">Operand 2.</param>
        /// <returns>True on equality.</returns>
        public static bool operator <(DATETIMETYPE a, DATETIMETYPE b)
        {
            return a.CompareTo(b) < 0;
        }

        /// <summary>
        /// Compares if the operand 1 is bigger than the operand 2.
        /// </summary>
        /// <param name="a">Operand 1.</param>
        /// <param name="b">Operand 2.</param>
        /// <returns>True on equality.</returns>
        public static bool operator >(DATETIMETYPE a, DATETIMETYPE b)
        {
            return a.CompareTo(b) > 0;
        }

        /// <summary>
        /// Compares to another DATETIMETYPE.
        /// </summary>
        /// <param name="other">Another DATETIMETYPE.</param>
        /// <returns>True if equals.</returns>
        public int CompareTo(DATETIMETYPE other)
        {
            if (other == null)
            {
                return 1;
            }

            if (!_hasOffset || !other._hasOffset)
            {
                return _value.DateTime.CompareTo(other._value.DateTime);
            }

            return _value.CompareTo(other._value);
        }

        /// <summary>
        /// Returns null.
        /// </summary>
        /// <returns>Null.</returns>
        public System.Xml.Schema.XmlSchema GetSchema()
        {
            return null;
        }

        /// <summary>
        /// Reads the XML and populate the inner value.
        /// </summary>
        /// <param name="reader">XmlReader.</param>
        public void ReadXml(System.Xml.XmlReader reader)
        {
            var str = reader.ReadElementContentAsString();
            QuickBooksRawString = str;

            if (string.IsNullOrWhiteSpace(str))
            {
                _value = MinValue._value;
                _hasOffset = false;
                return;
            }

            try
            {
                _value = ParseValue(str, out _hasOffset);
            }
            catch (Exception)
            {
                _value = MinValue._value;
                _hasOffset = false;
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

        private static DateTimeOffset ParseValue(string value, out bool hasOffset)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            // Check if the offset is supplied at the end of supplied value
            hasOffset = Regex.IsMatch(value, "(?:Z|[+-](?:2[0-3]|[01][0-9]):[0-5][0-9])$");

            // According to MSDN: If <Offset> is missing, its default value is the offset of the local time zone.
            // We assume universal below instead so offset is 0 when not supplied to the time does not get adjusted
            
            return DateTimeOffset.Parse(value, DateTimeFormatInfo.InvariantInfo, DateTimeStyles.AssumeUniversal);
        }


        /// <summary>
        /// Parses from a string value. When no offset is included, QuickBooks will interpret the date as the local time of the QuickBooks host computer.
        /// </summary>
        /// <param name="value">Date string in yyyy-mm-dd[Thh][:mm][:ss][K] format (though other standard formats will work as well). K is the offset from UTC (ie "Z", "-08:00", "+00:00", etc)</param>
        /// <returns>A new instance of the <see cref="DATETIMETYPE"/> class</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="value"/> is null</exception>
        /// <exception cref="FormatException">If <paramref name="value"/> is in an unsupported format</exception>
        public static DATETIMETYPE Parse(string value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            var date = ParseValue(value, out var hasOffset);

            return !hasOffset 
                ? new DATETIMETYPE(date.DateTime) 
                : FromUncorrectedDate(date);
        }

        /// <summary>
        /// Parses from a string value. If the <paramref name="value"/> cannot be parsed, the provided <paramref name="defaultValue"/> will be returned.
        /// If an offset from Coordinated Universal Time (UTC) is missing, QuickBooks will interpret the date as the local time of the QuickBooks host computer.
        /// </summary>
        /// <param name="value">Date string in yyyy-mm-dd[Thh][:mm][:ss][K] format (though other standard formats will work as well). K is the offset from UTC (ie "Z", "-08:00", "+00:00", etc)</param>
        /// <param name="defaultValue">The default value to use if parsing is unsuccessful</param>
        /// <returns>A new instance of <see cref="DATETIMETYPE"/>, or <paramref name="defaultValue"/></returns>
        public static DATETIMETYPE ParseOrDefault(string value, DATETIMETYPE defaultValue = default(DATETIMETYPE))
        {
            if (TryParse(value, out var result))
            {
                return result;
            }

            return defaultValue;
        }

        /// <summary>
        /// Tries to parse <see cref="DATETIMETYPE"/> from a string. If an offset is not provided in the string, one will not be inferred.
        /// When no offset is defined, QuickBooks will interpret the date as the local time of the QuickBooks host computer.
        /// </summary>
        /// <param name="value">Date string in yyyy-mm-dd[Thh][:mm][:ss][K] format (though other standard formats will work as well). K is the offset from UTC (ie "Z", "-08:00", "+00:00", etc)</param>
        /// <param name="date">If parsing was successful, the new instance of <see cref="DATETIMETYPE"/>, otherwise null</param>
        /// <returns>True is parsing was successful</returns>
        public static bool TryParse(string value, out DATETIMETYPE date)
        {
            try
            {
                date = Parse(value);
                return true;
            }
            catch
            {
                date = null;
                return false;
            }
        }

        /// <summary>
        /// Creates a new instance of the <see cref="DATETIMETYPE"/> class using an uncorrected <see cref="DateTimeOffset"/>.
        /// </summary>
        /// <param name="uncorrectedDate">The uncorrected <see cref="DateTimeOffset"/> value</param>
        public static DATETIMETYPE FromUncorrectedDate(DateTimeOffset uncorrectedDate)
        {
            if (uncorrectedDate > MaxValue._value)
            {
                throw new ArgumentOutOfRangeException(nameof(uncorrectedDate), uncorrectedDate, "The supplied value is greater than the QuickBooks epoch of 2038-01-19T03:14:07+00:00");
            }

            if (uncorrectedDate < MinValue._value)
            {
                throw new ArgumentOutOfRangeException(nameof(uncorrectedDate), uncorrectedDate, "The supplied value is less than the minimum allowed date of 1970-01-01");
            }

            return new DATETIMETYPE
            {
                _value = uncorrectedDate,
                _hasOffset = true,
                _isLocal = false
            };
        }

        /// <summary>
        /// The QuickBooks max value for a date &amp; time is 2038-01-19T03:14:07+00:00.
        /// </summary>
        public static readonly DATETIMETYPE MaxValue = new DATETIMETYPE
        {
            _value = new DateTimeOffset(2038, 1, 19, 3, 14, 7, 0, TimeSpan.Zero),
            _hasOffset = true,
        };

        /// <summary>
        /// Minimum date QuickBooks allows (1970-01-01).
        /// </summary>
        public static readonly DATETIMETYPE MinValue = new DATETIMETYPE
        {
            _value = new DateTimeOffset(1970, 1, 1, 0, 0, 0, 0, TimeSpan.Zero),
            _hasOffset = false,
        };
    }
}