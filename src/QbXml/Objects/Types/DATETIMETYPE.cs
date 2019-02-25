using System;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace QbSync.QbXml.Objects
{
    public partial class DATETIMETYPE : ITypeWrapper, IComparable<DATETIMETYPE>, IXmlSerializable
    {
        private DateTimeOffset _value;
        private bool _hasOffset;
        private readonly bool _isLocal;

        // Private constructor as only xml deserialization should be using this
        private DATETIMETYPE()
        {
        }

        
        [Obsolete("Use static DATETIMETYPE.Parse")]
        public DATETIMETYPE(string value)
        {
            _value = ParseValue(value, out _hasOffset);

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
        /// Initializes a new instance of the <see cref="DATETIMETYPE"/> class using the specified <see cref="DateTime"/>.
        /// NOTE: The offset from UTC, if any, that will be sent to QuickBooks along with this date depends on the <see cref="DateTime.Kind"/> of the specified <see cref="value"/>.
        /// <see cref="DateTimeKind"/>.<see cref="DateTimeKind.Utc"/> will send "+00:00".
        /// <see cref="DateTimeKind"/>.<see cref="DateTimeKind.Local"/> will send an offset as determined by the timezone of the machine this application is running on.
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
        /// Initializes a new instance of the <see cref="DATETIMETYPE"/> class using the specified <see cref="DateTimeOffset"/>.
        /// <see cref="DateTimeOffset.Offset"/> will be included with requests to QuickBooks.
        /// </summary>
        /// <param name="value">The <see cref="DateTimeOffset"/> value</param>
        public DATETIMETYPE(DateTimeOffset value)
        {
            if (value > MaxValue._value)
            {
                throw new ArgumentOutOfRangeException(nameof(value), value, "The supplied value is greater than the QuickBooks epoch of 2038-01-19T03:14:07+00:00");
            }

            if (value < MinValue._value)
            {
                throw new ArgumentOutOfRangeException(nameof(value), value, "The supplied value is less than the minimum allowed date of 1970-01-01");
            }

            _value = value;
            _hasOffset = true;
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="DATETIMETYPE"/> class using the specified year, month, day, hour, minute, second, and offset.
        /// </summary>
        /// <param name="year">The year (1970 - 2037)</param>
        /// <param name="month">The month of year (1 - 12)</param>
        /// <param name="day">The day of month (1 - number of days in <see cref="month"/>)</param>
        /// <param name="hour">The hour of day (0 - 23)</param>
        /// <param name="minute">The minute of the hour (0-59)</param>
        /// <param name="second">The second of the minute (0-59)</param>
        /// <param name="offset">The offset from UTC. If no offset is defined, QuickBooks will interpret the date as the local time of the QuickBooks host computer.</param>
        public DATETIMETYPE(int year, int month, int day, int hour = 0, int minute = 0, int second = 0, TimeSpan? offset = null)
        {
            if (year < 1970)
            {
                throw new ArgumentOutOfRangeException(nameof(year), year, "Year is less than minimum allowed value of 1970");
            }

            _value = new DateTimeOffset(year, month, day, hour, minute, second, offset ?? TimeSpan.Zero);

            if (_value > MaxValue._value)
            {
                throw new ArgumentOutOfRangeException(message: "The created date is greater than the QuickBooks epoch of 2038-01-19T03:14:07+00:00", innerException: null);
            }

            _hasOffset = offset != null;
        }


        /// <summary>
        /// Get's the time offset from Coordinated Universal Time (UTC), if available
        /// </summary>
        public TimeSpan? Offset => _hasOffset ? _value.Offset : (TimeSpan?)null;

        /// <summary>
        /// Gets the number of ticks for the date
        /// </summary>
        public long Ticks => _value.Ticks;

        /// <summary>
        /// Gets the year component of the date (1970-2038)
        /// </summary>
        public int Year => _value.Year;

        /// <summary>
        /// Gets the month component of the date (1-12)
        /// </summary>
        public int Month => _value.Month;

        /// <summary>
        /// Gets the day of month component of the date (1-31)
        /// </summary>
        public int Day => _value.Day;

        /// <summary>
        /// Gets the hour component of the date (0-23)
        /// </summary>
        public int Hour => _value.Hour;

        /// <summary>
        /// Gets the minute component of the date (0-59)
        /// </summary>
        public int Minute => _value.Minute;

        /// <summary>
        /// Gets the second component of the date (0-59)
        /// </summary>
        public int Second => _value.Second;


        /// <summary>
        /// Returns the date in "yyyy-MM-ddTHH:mm:ss[K]" format (The 'K' offset will only be appended if <see cref="Offset"/> is available)
        /// </summary>
        public override string ToString()
        {
            return _hasOffset
                ? _value.ToString("yyyy-MM-ddTHH:mm:ssK", CultureInfo.InvariantCulture)
                : _value.ToString("yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture);
        }


        /// <summary>
        /// Gets a <see cref="DateTime"/> value of this date.
        /// NOTE: In most cases this will have an <see cref="DateTimeKind.Unspecified"/> <see cref="DateTimeKind"/>.
        /// Use of <see cref="DateTime.ToUniversalTime"/> or <see cref="DateTime.ToLocalTime"/> on the returned value is not recommended. 
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
        /// Gets a <see cref="DateTimeOffset"/> value for this date, if <see cref="Offset"/> is available. Will throw a <see cref="InvalidOperationException"/> otherwise.
        /// Alternatively use <see cref="TryGetDateTimeOffset"/>, which will not throw.
        /// </summary>
        /// <exception cref="InvalidOperationException">Throws if <see cref="Offset"/> is null</exception>
        public DateTimeOffset GetDateTimeOffset()
        {
            if (!_hasOffset)
            {
                throw new InvalidOperationException("An offset is not available");
            }

            return _value;
        }

        /// <summary>
        /// Attempts to return a <see cref="DateTimeOffset"/> value for this date.
        /// </summary>
        /// <returns>False if <see cref="Offset"/> is not available, and an accurate <see cref="DateTimeOffset"/> cannot be returned</returns> 
        /// <remarks>
        /// Offsets are optional when communicating with QuickBooks, and in the case of responses, are often incorrect.
        /// Because of this, sometimes <see cref="DATETIMETYPE"/> needs to be treated as a <see cref="DateTime"/> and others as a <see cref="DateTimeOffset"/>.
        /// </remarks>
        public bool TryGetDateTimeOffset(out DateTimeOffset value)
        {
            if (!_hasOffset)
            {
                value = default(DateTimeOffset);
                return false;
            }

            value = _value;
            return true;
        }

        public override bool Equals(object obj)
        {
            if (obj is DATETIMETYPE other)
            {
                return _value == other._value && 
                       _hasOffset == other._hasOffset;
            }

            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return _value.GetHashCode();
        }

        public static bool operator ==(DATETIMETYPE a, DATETIMETYPE b)
        {
            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(a, b))
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

        public static bool operator !=(DATETIMETYPE a, DATETIMETYPE b)
        {
            return !(a == b);
        }

        public static bool operator <(DATETIMETYPE a, DATETIMETYPE b)
        {
            return a.CompareTo(b) < 0;
        }

        public static bool operator >(DATETIMETYPE a, DATETIMETYPE b)
        {
            return a.CompareTo(b) > 0;
        }

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

        public System.Xml.Schema.XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(System.Xml.XmlReader reader)
        {
            var str = reader.ReadElementContentAsString();
            
            if (string.IsNullOrWhiteSpace(str))
            {
                _value = MinValue._value;
                _hasOffset = false;
                return;
            }

            var timeZone = QbXmlResponse.qbXmlResponseOptionsStatic?.QuickBooksDesktopTimeZone;

            if (timeZone == null && str.Length > 19)
            {
                // QuickBooks has inaccurate offset values
                // The DateTime is otherwise accurate (follows DST), but the offset does not follow DST
                // To accomodate for this, simply ignore the offset portion of the string, but only when parsing from XML
                str = str.Substring(0, 19);
            }

            try
            {
                _value = ParseValue(str, out _hasOffset);

                if (timeZone != null && timeZone.SupportsDaylightSavingTime)
                {
                    // QuickBooks always returns the BaseUTC offset
                    // Verify this matches the returned value so we know the fix is configured correctly
                    if (timeZone.BaseUtcOffset == _value.Offset)
                    {
                        // A time zone was specified, so we can correct the values supplied by QB
                        var correctedOffset = timeZone.GetUtcOffset(_value.DateTime);
                        _value = new DateTimeOffset(_value.DateTime, correctedOffset);
                        _hasOffset = true;
                    }
                    else
                    {
                        _hasOffset = false;
                    }
                }
            }
            catch (Exception)
            {
                _value = MinValue._value;
                _hasOffset = false;
            }
        }

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
        /// <exception cref="ArgumentNullException">If <see cref="value"/> is null</exception>
        /// <exception cref="FormatException">If <see cref="value"/> is in an unsupported format</exception>
        public static DATETIMETYPE Parse(string value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            var date = ParseValue(value, out var hasOffset);

            return new DATETIMETYPE(date)
            {
                _hasOffset = hasOffset
            };
        }


        /// <summary>
        /// Parses from a string value. If the <see cref="value"/> cannot be parsed, the provided <see cref="defaultValue"/> will be returned.
        /// If an offset from Coordinated Universal Time (UTC) is missing, QuickBooks will interpret the date as the local time of the QuickBooks host computer.
        /// </summary>
        /// <param name="value">Date string in yyyy-mm-dd[Thh][:mm][:ss][K] format (though other standard formats will work as well). K is the offset from UTC (ie "Z", "-08:00", "+00:00", etc)</param>
        /// <param name="defaultValue">The default value to use if parsing is unsuccessful</param>
        /// <returns>A new instance of <see cref="DATETIMETYPE"/>, or <see cref="defaultValue"/></returns>
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
        /// The QuickBooks max value for a date & time is 2038-01-19T03:14:07+00:00
        /// </summary>
        public static readonly DATETIMETYPE MaxValue = new DATETIMETYPE
        {
            _value = new DateTimeOffset(2038, 1, 19, 3, 14, 7, 0, TimeSpan.Zero),
            _hasOffset = true,
        };

        /// <summary>
        /// Minimum date QuickBooks allows (1970-01-01)
        /// </summary>
        public static readonly DATETIMETYPE MinValue = new DATETIMETYPE
        {
            _value = new DateTimeOffset(1970, 1, 1, 0, 0, 0, 0, TimeSpan.Zero),
            _hasOffset = false,
        };
    }
}