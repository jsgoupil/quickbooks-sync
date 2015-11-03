using System;
using System.Globalization;
using System.Xml.Serialization;

namespace QbSync.QbXml.Objects
{
    public partial class DATETIMETYPE : ITypeWrapper, IComparable<DATETIMETYPE>, IXmlSerializable
    {
        private DateTime _value;
        protected TimeZoneInfo timeZoneInfo;

        public DATETIMETYPE()
            : this((TimeZoneInfo)null)
        {
            // Used for serialization
            if (QbXmlResponse.qbXmlResponseOptionsStatic != null)
            {
                this.timeZoneInfo = QbXmlResponse.qbXmlResponseOptionsStatic.TimeZoneBugFix;
            }
        }

        public DATETIMETYPE(TimeZoneInfo timeZoneInfo)
            : this(default(DateTime), timeZoneInfo)
        {
            this.timeZoneInfo = timeZoneInfo;
        }

        public DATETIMETYPE(string value)
            : this(value, null)
        {
        }

        public DATETIMETYPE(string value, TimeZoneInfo timeZoneInfo)
            : this(Parse(value), timeZoneInfo)
        {
        }

        public DATETIMETYPE(DateTime value)
            : this(value, null)
        {
        }

        public DATETIMETYPE(DateTime value, TimeZoneInfo timeZoneInfo)
        {
            Init(value, timeZoneInfo);
        }

        private void Init(DateTime value, TimeZoneInfo timeZoneInfo)
        {
            this.timeZoneInfo = timeZoneInfo;

            this.value = value;
        }

        protected DateTime value
        {
            get
            {
                return this._value;
            }

            set
            {
                this._value = value;

                // Mono Build does not like having IsDaylightSavingTime with an invalid date.
                // Let's make a check here.
                if (this.value != DateTime.MinValue)
                {
                    if (timeZoneInfo != null && timeZoneInfo.IsDaylightSavingTime(this.value))
                    {
                        // QuickBooks does not handle DST. They will send the date with no DST applied.
                        // In order to get the correct date, we modify the date if a timezone was provided and
                        // apply the daylight time saving manually if the date is currently in DST mode.
                        foreach (var adjustmentRule in timeZoneInfo.GetAdjustmentRules())
                        {
                            if (this._value.CompareTo(adjustmentRule.DateStart) > 0 && this._value.CompareTo(adjustmentRule.DateEnd) < 0)
                            {
                                this._value = this.value.Add(-adjustmentRule.DaylightDelta);
                                break;
                            }
                        }
                    }
                }
            }
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
                value = Parse(reader.ReadContentAsString());
                reader.ReadEndElement();
            }
        }

        public void WriteXml(System.Xml.XmlWriter writer)
        {
            writer.WriteString(ToString());
        }

        private static DateTime Parse(string value)
        {
            DateTime output;
            if (DateTime.TryParse(value, out output))
            {
                return output;
            }

            return DateTime.MinValue;
        }
    }
}