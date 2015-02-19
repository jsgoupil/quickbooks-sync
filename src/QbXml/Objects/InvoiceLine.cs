using QbSync.QbXml.Type;
using System.Collections.Generic;

namespace QbSync.QbXml.Objects
{
    public class InvoiceLine : InvoiceLineBase
    {
        public IdType TxnLineID { get; set; }
        public IEnumerable<DataExt> DataExtRet { get; set; }
    }
}