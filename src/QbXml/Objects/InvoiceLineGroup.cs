using QbSync.QbXml.Type;
using System.Collections.Generic;

namespace QbSync.QbXml.Objects
{
    public class InvoiceLineGroup : InvoiceLineBaseLow
    {
        public IdType TxnLineID { get; set; }
        public Ref ItemGroupRef { get; set; }
        public BoolType IsPrintItemsInGroup { get; set; }
        public AmtType TotalAmount { get; set; }
        public IEnumerable<InvoiceLineBase> InvoiceLineRet { get; set; }
        public IEnumerable<DataExt> DataExtRet { get; set; }
    }
}