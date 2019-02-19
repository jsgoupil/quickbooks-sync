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

        private bool _ignoreOffset;
        private bool _canReadXml;
        private readonly bool _isLocal;

        //Private constructor as only xml deserialization should be using this
        private DATETIMETYPE()
        {
            _canReadXml = true;
        }

        
        [Obsolete("Use static DATETIMETYPE.Parse")]
        public DATETIMETYPE(string value)
        {
            _value = ParseValue(value, out _ignoreOffset);

            if (this > MaxValue)
            {
                throw new ArgumentOutOfRangeException(nameof(value), value, "The supplied value is greater than the QuickBooks epoch of 2038-01-19T03:14:07+00:00");
            }

            if (this < MinValue)
            {
                throw new ArgumentOutOfRangeException(nameof(value), value, "The supplied value is less than the minimum allowed date of 1970-01-01");
            }
        }

        [Obsolete("TimeZoneInfo is no longer used. Use static DATETIMETYPE.Parse")]
        public DATETIMETYPE(string value, TimeZoneInfo timeZoneInfo) 
            : this(value)
        {
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="DATETIMETYPE"/> class using the specified <see cref="DateTime"/>.
        /// IMPORTANT NOTE: The <see cref="DateTime.Kind"/> property of the <see cref="value"/> will be used to determine 
        /// if and what offset should be used when sending the value to QuickBooks. 
        /// <see cref="DateTimeKind"/>.<see cref="DateTimeKind.Utc"/> will use "+00:00".
        /// <see cref="DateTimeKind"/>.<see cref="DateTimeKind.Local"/> will use the offset as determined by the timezone of the machine this application is running on.
        /// <see cref="DateTimeKind"/>.<see cref="DateTimeKind.Unspecified"/> will use no offset, which QuickBooks will interpret to mean the local time of the QuickBooks host computer.
        /// </summary>
        /// <seealso cref="DateTimeKind"/>
        public DATETIMETYPE(DateTime value)
        {
            if (value.Kind == DateTimeKind.Utc)
            {
                _value = new DateTimeOffset(value, TimeSpan.Zero);
            }
            else if (value.Kind == DateTimeKind.Local)
            {
                _value = new DateTimeOffset(value);
                _isLocal = true;
            }
            else
            {
                _ignoreOffset = true;
                _value = new DateTimeOffset(value, TimeSpan.Zero);
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


        [Obsolete("TimeZoneInfo is no longer used. Use overload without timeZoneInfo")]
        public DATETIMETYPE(DateTime value, TimeZoneInfo timeZoneInfo)
            : this (value)
        {
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="DATETIMETYPE"/> class using the specified <see cref="DateTimeOffset"/>.
        /// <see cref="HasOffset"/> will always be true, and the supplied offset will be included with requests to QuickBooks.
        /// </summary>
        /// <param name="value">The <see cref="DateTimeOffset"/> value</param>
        public DATETIMETYPE(DateTimeOffset value)
        {
            _value = value;

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
        /// Initializes a new instance of the <see cref="DATETIMETYPE"/> class using the specified year, month, day, hour, minute, second, and optional offset.
        /// </summary>
        /// <param name="year">The year (1970 - 2037)</param>
        /// <param name="month">The month of year (1 - 12)</param>
        /// <param name="day">The day of month (1 - number of days in <see cref="month"/>)</param>
        /// <param name="hour">The hour of day (0 - 23)</param>
        /// <param name="minute">The minute of the hour (0-59)</param>
        /// <param name="second">The second of the minute (0-59)</param>
        /// <param name="offset">The optional offset from UTC. If no offset is defined, QuickBooks will interpret the date as the local time of the QuickBooks host computer.</param>
        public DATETIMETYPE(int year, int month, int day, int hour = 0, int minute = 0, int second = 0, TimeSpan? offset = null)
        {
            if (year > 2037 || year < 1970) throw new ArgumentOutOfRangeException(nameof(year), year, "Year must be between 1970 and 2037, inclusive");

            _value = new DateTimeOffset(year, month, day, hour, minute, second, offset ?? TimeSpan.Zero);
            _ignoreOffset = offset == null;
        }


        /// <summary>
        /// Returns the date in "yyyy-MM-ddTHH:mm:ss" format.
        /// An offset will be appended if available (+00:00 or -08:00 etc)
        /// </summary>
        public override string ToString()
        {
            return _ignoreOffset
                ? _value.ToString("yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture)
                : _value.ToString("yyyy-MM-ddTHH:mm:ssK", CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Gets a <see cref="DateTime"/> value of this date.
        /// </summary>
        public DateTime ToDateTime()
        {
            if (_ignoreOffset)
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
        /// If this date has a UTC offset specified. If this value was parsed from a QuickBooks response, the existence
        /// of an offset will likely depend on if a <see cref="QbXmlResponseOptions.TimeZoneBugFix"/> was configured
        /// for the <see cref="QbXmlResponseOptions"/>. If the <see cref="QbXmlResponseOptions.TimeZoneBugFix"/>, the offset
        /// can not be reliable interpreted, so it will be ignored. 
        /// </summary>
        public bool HasOffset()
        {
            return !_ignoreOffset;
        }

        /// <summary>
        /// Gets a <see cref="DateTimeOffset"/> value for this date (will throw if offset not specified).
        /// Use <see cref="HasOffset"/> to determine if this can be used, otherwise use <see cref="TryGetDateTimeOffset"/>, <see cref="ToDateTime"/>, or <see cref="ToString"/>.
        /// </summary>
        /// <exception cref="InvalidOperationException">Throws if an offset is not specified</exception>
        public DateTimeOffset GetDateTimeOffset()
        {
            if (_ignoreOffset)
            {
                throw new InvalidOperationException("An offset is not available");
            }

            return _value;
        }

        /// <summary>
        /// Attempts to return a <see cref="DateTimeOffset"/> value for this date.
        /// </summary>
        /// <returns>False if offset inforation is not available, and an accurate <see cref="DateTimeOffset"/> cannot be returned</returns> 
        /// <remarks>
        /// Offsets are optional when communicating with QuickBooks, and in the case of responses, are often incorrect.
        /// Because of this, sometimes <see cref="DATETIMETYPE"/> needs to be treated as a <see cref="DateTime"/> and others as a <see cref="DateTimeOffset"/>.
        /// </remarks>
        public bool TryGetDateTimeOffset(out DateTimeOffset value)
        {
            if (_ignoreOffset)
            {
                value = DateTimeOffset.MinValue;
                return false;
            }

            value = _value;
            return true;
        }

        public override bool Equals(object obj)
        {
            var objType = obj as DATETIMETYPE;
            if (objType != null)
            {
                return _value == objType._value && 
                       _ignoreOffset == objType._ignoreOffset;
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

        //TODO: Remove support for implicit casting from system types, as they may throw exceptions
        [Obsolete("Implicit operators will be no longer supported")]
        public static implicit operator DATETIMETYPE(DateTime value)
        {
            return new DATETIMETYPE(value);
        }

        //TODO: Remove support for implicit casting to system types. User should make a explicit choice to lose data to convert to DateTime
        [Obsolete("Implicit operators will be no longer supported")]
        public static implicit operator DateTime(DATETIMETYPE type)
        {
            if (type != null)
            {
                return type.ToDateTime();
            }

            return default(DateTime);
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
            if (other == null) return 1;

            if (_ignoreOffset || other._ignoreOffset)
            {
                return _value.DateTime.CompareTo(other._value.DateTime);
            }

            return _value.CompareTo(other._value);
        }

        public System.Xml.Schema.XmlSchema GetSchema()
        {
            return null;
        }

        [Obsolete]
        public void ReadXml(System.Xml.XmlReader reader)
        {
            if (!_canReadXml)
            {
                //This class needs to be immutable as possible
                //Only allow this when constructed using the empty constructor to only be used by deserialization
                throw new InvalidOperationException("This method not to be used by user code");
            }

            //Only allow ReadXML once to prevent user code from calling this method after deserialized
            _canReadXml = false;

            reader.MoveToContent();
            var isEmptyElement = reader.IsEmptyElement;
            reader.ReadStartElement();

            if (isEmptyElement)
            {
                _value = new DateTimeOffset(1970, 1, 1, 0, 0, 0, 0, TimeSpan.Zero);
                _ignoreOffset = true;
                return;
            }

            var str = reader.ReadContentAsString();
            var timeZone = QbXmlResponse.qbXmlResponseOptionsStatic?.TimeZoneBugFix;

            if (timeZone == null && str != null && str.Length > 19)
            {
                //QuickBooks has inaccurate offset values
                //The DateTime is otherwise accurate (follows DST), but the offset does not follow DST
                //To accomodate for this, simply ignore the offset portion of the string, but only when parsing from XML
                str = str.Substring(0, 19);
            }

            try
            {
                _value = ParseValue(str, out _ignoreOffset);

                if (timeZone != null && timeZone.SupportsDaylightSavingTime)
                {
                    //QuickBooks always returns the BaseUTC offset
                    //Verify this matches the returned value so we know the fix is configured correctly
                    if (timeZone.BaseUtcOffset == _value.Offset)
                    {
                        //A time zone was specified, so we can correct the values supplied by QB
                        var correctedOffset = timeZone.GetUtcOffset(_value.DateTime);
                        _value = new DateTimeOffset(_value.DateTime, correctedOffset);
                    }
                    else
                    {
                        _ignoreOffset = true;
                    }
                }
            }
            catch (Exception)
            {
                _value = new DateTimeOffset(1970, 1, 1, 0, 0, 0, 0, TimeSpan.Zero);
                _ignoreOffset = true;
            }
            finally
            {
               reader.ReadEndElement();
            }
        }

        public void WriteXml(System.Xml.XmlWriter writer)
        {
            writer.WriteString(ToString());
        }

        private static DateTimeOffset ParseValue(string value, out bool isMissingOffset)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            //Check if the offset is supplied at the end of supplied value
            isMissingOffset = !Regex.IsMatch(value, "(?:Z|[+-](?:2[0-3]|[01][0-9]):[0-5][0-9])$");

            //According to MSDN: If <Offset> is missing, its default value is the offset of the local time zone.
            //We assume universal below instead so offset is 0 when not supplied to the time does not get adjusted
            
            return DateTimeOffset.Parse(value, DateTimeFormatInfo.InvariantInfo, DateTimeStyles.AssumeUniversal);
        }


        /// <summary>
        /// Parses from a string value. If an offset is not provided in the string, one will not be inferred.
        /// When no offset is defined, QuickBooks will interpret the date as the local time of the QuickBooks host computer.
        /// </summary>
        /// <param name="value">Date string in either "yyyy-MM-ddTHH:mm:ss" or "yyyy-MM-ddTHH:mm:ssK" format (but others may work as well). K is the offset (ie "Z", "-08:00", "+00:00", etc)</param>
        /// <returns>A new instance of the <see cref="DATETIMETYPE"/> class</returns>
        /// <exception cref="ArgumentNullException">If <see cref="value"/> is null</exception>
        /// <exception cref="FormatException">If <see cref="value"/> is in an unsupported format</exception>
        public static DATETIMETYPE Parse(string value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            bool ignoreOffset;
            var date = ParseValue(value, out ignoreOffset);

            return new DATETIMETYPE(date)
            {
                _ignoreOffset = ignoreOffset
            };
        }


        /// <summary>
        /// The QuickBooks epoch is 2038-01-19T03:14:07+00:00. No date can be higher than this in QuickBooks
        /// </summary>
        public static readonly DATETIMETYPE MaxValue = new DATETIMETYPE
        {
            _value = new DateTimeOffset(2038, 1, 19, 3, 14, 7, 0, TimeSpan.Zero),
            _ignoreOffset = false,
            _canReadXml = false
        };

        /// <summary>
        /// Minimum date QuickBooks allows (1970-01-01)
        /// </summary>
        public static readonly DATETIMETYPE MinValue = new DATETIMETYPE
        {
            _value = new DateTimeOffset(1970, 1, 1, 0, 0, 0, 0, TimeSpan.Zero),
            _ignoreOffset = true,
            _canReadXml = false
        };

    }
}