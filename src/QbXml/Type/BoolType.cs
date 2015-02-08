using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace QBSync.QbXml.Type
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
            // TODO
            return "";
        }

        public static implicit operator BoolType(bool value)
        {
            return new BoolType(value);
        }
    }
}
