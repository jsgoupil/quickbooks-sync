using QbSync.QbXml.Objects;
using QbSync.QbXml.Type;
using System.Collections.Generic;
using System.Linq;

namespace QbSync.QbXml.Messages.Requests
{
    public class InvoiceAddRequest : InvoiceRequest<InvoiceAddRqType>
    {
        public GuidType ExternalGUID { get; set; }
        public IEnumerable<string> LinkToTxnID { get; set; }
        public IEnumerable<InvoiceLineAdd> InvoiceLineAdd { get; set; }
        public IEnumerable<InvoiceLineGroupAdd> InvoiceLineGroupAdd { get; set; }

        protected override void ProcessObj(InvoiceAddRqType obj)
        {
            base.ProcessObj(obj);

            obj.InvoiceAdd = new InvoiceAdd
            {
                CustomerRef = CustomerRef,
                ClassRef = ClassRef,
                ARAccountRef = ARAccountRef,
                TemplateRef = TemplateRef,
                TxnDate = TxnDate == null ? null : TxnDate.ToString(),
                RefNumber = RefNumber,
                BillAddress = BillAddress,
                ShipAddress = ShipAddress,
                IsPending = IsPending == null ? null : IsPending.ToString(),
                PONumber = PONumber,
                TermsRef = TermsRef,
                DueDate = DueDate == null ? null : DueDate.ToString(),
                SalesRepRef = SalesRepRef,
                FOB = FOB,
                ShipDate = ShipDate == null ? null : ShipDate.ToString(),
                ShipMethodRef = ShipMethodRef,
                ItemSalesTaxRef = ItemSalesTaxRef,
                Memo=  Memo,
                CustomerMsgRef = CustomerMsgRef,
                IsToBePrinted = IsToBePrinted == null ? null : IsToBePrinted.ToString(),
                IsToBeEmailed = IsToBeEmailed == null ? null : IsToBeEmailed.ToString(),
                IsTaxIncluded = IsTaxIncluded == null ? null : IsTaxIncluded.ToString(),
                CustomerSalesTaxCodeRef = CustomerSalesTaxCodeRef,
                Other = Other,
                ExchangeRate = ExchangeRate == null ? null : ExchangeRate.ToString(),
                SetCredit = SetCredit == null ? null : SetCredit.ToArray(),

                // Specific
                ExternalGUID = ExternalGUID == null ? null : ExternalGUID.ToString(),
                LinkToTxnID = LinkToTxnID == null ? null : LinkToTxnID.ToArray()
            };

            var items = new ItemWithoutName();
            if (InvoiceLineAdd != null)
            {
                foreach (var item in InvoiceLineAdd)
                {
                    items.Add(item);
                }
            }

            if (InvoiceLineGroupAdd != null)
            {
                foreach (var item in InvoiceLineGroupAdd)
                {
                    items.Add(item);
                }
            }

            obj.InvoiceAdd.Items = items.GetItems();

            if (IncludeRetElement != null)
            {
                obj.IncludeRetElement = IncludeRetElement.ToArray();
            }
        }
    }
}
