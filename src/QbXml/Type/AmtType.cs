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

        public decimal ToDecimal()
        {
            return value;
        }

        public static implicit operator AmtType(decimal value)
        {
            return new AmtType(value);
        }

        public static implicit operator decimal(AmtType type)
        {
            if (type != null)
            {
                return type.ToDecimal();
            }

            return default(decimal);
        }
    }
}
