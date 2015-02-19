using QbSync.QbXml.Type;
using System.Collections.Generic;

namespace QbSync.QbXml.Objects
{
    public class InvoiceLineGroup : BaseInvoiceLine
    {
        public Ref ItemGroupRef { get; set; }
        public BoolType IsPrintItemsInGroup { get; set; }
        public AmtType TotalAmount { get; set; }
        public IEnumerable<InvoiceLine> InvoiceLineRet { get; set; }
        public IEnumerable<DataExt> DataExtRet { get; set; }
    }
}