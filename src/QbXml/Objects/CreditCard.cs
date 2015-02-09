using QbSync.QbXml.Type;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QbSync.QbXml.Objects
{
    public class CreditCard
    {
        public StrType CreditCardNumber { get; set; }
        public IntType ExpirationMonth { get; set; }
    }
}
