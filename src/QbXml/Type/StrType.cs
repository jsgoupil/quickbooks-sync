using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QbSync.QbXml.Type
{
    public class StrType : IStringConvertible
    {
        private string value;

        public StrType(string value)
        {
            this.value = value;
        }

        public override string ToString()
        {
            // TODO QBSync to we need to encode?
            return value;
        }

        public static implicit operator StrType(string value)
        {
            if (value == null)
            {
                return null;
            }

            return new StrType(value);
        }
    }
}
