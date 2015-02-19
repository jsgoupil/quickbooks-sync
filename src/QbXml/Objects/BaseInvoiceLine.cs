using QbSync.QbXml.Type;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QbSync.QbXml.Objects
{
    public class BaseInvoiceLine
    {
        public IdType TxnLineID { get; set; }
        public StrType Desc { get; set; }
        public QuanType Quantity { get; set; }
        public StrType UnitOfMeasure { get; set; }
        public Ref OverrideUOMSetRef { get; set; }
    }
}
