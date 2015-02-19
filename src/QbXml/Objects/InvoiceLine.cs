using QbSync.QbXml.Type;
using System.Collections.Generic;

namespace QbSync.QbXml.Objects
{
    public class InvoiceLine : BaseInvoiceLine
    {
        public Ref ItemRef { get; set; }
        public PriceType Rate { get; set; }
        public PercentType RatePercent { get; set; }
        public Ref ClassRef { get; set; }
        public AmtType Amount { get; set; }
        public Ref InventorySiteRef { get; set; }
        public Ref InventorySiteLocationRef { get; set; }
        public StrType SerialNumber { get; set; }
        public StrType LotNumber { get; set; }
        public DateType ServiceDate { get; set; }
        public Ref SalesTaxCodeRef { get; set; }
        public StrType Other1 { get; set; }
        public StrType Other2 { get; set; }
        public IEnumerable<DataExt> DataExtRet { get; set; }
    }
}