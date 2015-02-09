using System;
using System.Globalization;

namespace QbSync.QbXml.Type
{
    public class BoolType : IStringConvertible
    {
        private bool value;

        public BoolType(string value)
        {
            this.value = Boolean.Parse(value);
        }

        public BoolType(bool value)
        {
            this.value = value;
        }

        public override string ToString()
        {
            return value.ToString(CultureInfo.InvariantCulture).ToLowerInvariant();
        }

        public static implicit operator BoolType(bool value)
        {
            return new BoolType(value);
        }
    }
}
