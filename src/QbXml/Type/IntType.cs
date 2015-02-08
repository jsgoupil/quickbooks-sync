using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace QBSync.QbXml.Type
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

        public static implicit operator IntType(int value)
        {
            return new IntType(value);
        }
    }
}
