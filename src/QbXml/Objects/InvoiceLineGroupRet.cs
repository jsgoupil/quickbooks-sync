using QbSync.QbXml.Type;
using System.Collections.Generic;

namespace QbSync.QbXml.Objects
{
    public class InvoiceLineGroupRet : BaseInvoiceLine
    {
        public Ref ItemGroupRef { get; set; }
        public BoolType IsPrintItemsInGroup { get; set; }
        public AmtType TotalAmount { get; set; }
        public IEnumerable<InvoiceLineRet> InvoiceLineRet { get; set; }
        public IEnumerable<DataExtRet> DataExtRet { get; set; }
    }
}