using QbSync.QbXml.Objects;
using QbSync.QbXml.Type;
using System.Linq;
using System.Collections.Generic;

namespace QbSync.QbXml.Messages.Requests
{
    public class InvoiceModRequest : InvoiceRequest<InvoiceModRqType>
    {
        public BoolType IsFinanceCharge { get; set; }

        public string TxnID { get; set; }
        public string EditSequence { get; set; }
        public IEnumerable<InvoiceLineMod> InvoiceLineMod { get; set; }
        public IEnumerable<InvoiceLineGroupMod> InvoiceLineGroupMod { get; set; }

        protected override void ProcessObj(InvoiceModRqType obj)
        {
            base.ProcessObj(obj);

            obj.InvoiceMod = new InvoiceMod
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
                Memo = Memo,
                CustomerMsgRef = CustomerMsgRef,
                IsToBePrinted = IsToBePrinted == null ? null : IsToBePrinted.ToString(),
                IsToBeEmailed = IsToBeEmailed == null ? null : IsToBeEmailed.ToString(),
                IsTaxIncluded = IsTaxIncluded == null ? null : IsTaxIncluded.ToString(),
                CustomerSalesTaxCodeRef = CustomerSalesTaxCodeRef,
                Other = Other,
                ExchangeRate = ExchangeRate == null ? null : ExchangeRate.ToString(),
                SetCredit = SetCredit == null ? null : SetCredit.ToArray(),

                // Specific
                TxnID = TxnID,
                EditSequence = EditSequence
            };

            var items = new ItemWithoutName();
            if (InvoiceLineMod != null)
            {
                foreach (var item in InvoiceLineMod)
                {
                    items.Add(item);
                }
            }

            if (InvoiceLineGroupMod != null)
            {
                foreach (var item in InvoiceLineGroupMod)
                {
                    items.Add(item);
                }
            }

            obj.InvoiceMod.Items = items.GetItems();

            if (IncludeRetElement != null)
            {
                obj.IncludeRetElement = IncludeRetElement.ToArray();
            }
        }
    }
}
