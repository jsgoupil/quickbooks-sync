using System;
using System.Globalization;

namespace QbSync.QbXml.Type
{
    public class AmtType : IStringConvertible
    {
        private decimal value;

        public AmtType(string value)
        {
            this.value = Decimal.Parse(value);
        }

        public AmtType(decimal value)
        {
            this.value = value;
        }

        public override string ToString()
        {
            return value.ToString("F", CultureInfo.InvariantCulture);
        }

        public static implicit operator AmtType(decimal value)
        {
            return new AmtType(value);
        }
    }
}
