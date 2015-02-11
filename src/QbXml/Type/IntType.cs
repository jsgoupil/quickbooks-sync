using System;
using System.Globalization;

namespace QbSync.QbXml.Type
{
    public class IntType : IStringConvertible
    {
        private int value;

        public IntType(string value)
        {
            this.value = Int32.Parse(value);
        }

        public IntType(int value)
        {
            this.value = value;
        }

        public override string ToString()
        {
            return value.ToString(CultureInfo.InvariantCulture);
        }

        public int ToInt()
        {
            return value;
        }

        public static implicit operator IntType(int value)
        {
            return new IntType(value);
        }

        public static implicit operator int(IntType type)
        {
            if (type != null)
            {
                return type.ToInt();
            }

            return default(int);
        }
    }
}
