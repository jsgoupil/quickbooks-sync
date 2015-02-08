using QBSync.QbXml.Type;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QBSync.QbXml.Objects
{
    public class CreditCard
    {
        public StrType CreditCardNumber { get; set; }
        public IntType ExpirationMonth { get; set; }
    }
}
