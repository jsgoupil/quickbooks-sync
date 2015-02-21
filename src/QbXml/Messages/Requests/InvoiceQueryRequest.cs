using QbSync.QbXml.Objects;
using System.Linq;
using QbSync.QbXml.Type;
using System.Collections.Generic;

namespace QbSync.QbXml.Messages.Requests
{
    public class InvoiceQueryRequest : QbXmlObject<InvoiceQueryRqType>
    {
        public IEnumerable<string> TxnID { get; set; }
        public IEnumerable<string> RefNumber { get; set; }
        public IEnumerable<string> RefNumberCaseSensitive { get; set; }

        public int? MaxReturned { get; set; }

        public ModifiedDateRangeFilter ModifiedDateRangeFilter { get; set; }
        public TxnDateRangeFilter TxnDateRangeFilter { get; set; }

        public EntityFilter EntityFilter { get; set; }
        public AccountFilter AccountFilter { get; set; }

        public RefNumberFilter RefNumberFilter { get; set; }
        public RefNumberRangeFilter RefNumberRangeFilter { get; set; }

        public CurrencyFilter CurrencyFilter { get; set; }

        public PaidStatus? PaidStatus { get; set; }

        public BoolType IncludeLineItems { get; set; }
        public BoolType IncludeLinkedTxns { get; set; }

        public IEnumerable<string> IncludeRetElement { get; set; }
        public IEnumerable<GuidType> OwnerID { get; set; }

        protected override void ProcessObj(InvoiceQueryRqType obj)
        {
            base.ProcessObj(obj);

            var items = new ItemWithName<ItemsChoiceType72>();
            if (TxnID != null)
            {
                foreach (var item in TxnID)
                {
                    items.AddNotNull(ItemsChoiceType72.TxnID, item);
                }
            }

            if (RefNumber != null)
            {
                foreach (var item in RefNumber)
                {
                    items.AddNotNull(ItemsChoiceType72.RefNumber, item);
                }
            }

            if (RefNumberCaseSensitive != null)
            {
                foreach (var item in RefNumberCaseSensitive)
                {
                    items.AddNotNull(ItemsChoiceType72.RefNumberCaseSensitive, item);
                }
            }

            items.AddNotNull(ItemsChoiceType72.AccountFilter, AccountFilter);
            items.AddNotNull(ItemsChoiceType72.CurrencyFilter, CurrencyFilter);
            items.AddNotNull(ItemsChoiceType72.EntityFilter, EntityFilter);
            items.AddNotNull(ItemsChoiceType72.MaxReturned, MaxReturned);
            items.AddNotNull(ItemsChoiceType72.ModifiedDateRangeFilter, ModifiedDateRangeFilter);
            items.AddNotNull(ItemsChoiceType72.PaidStatus, PaidStatus);
            items.AddNotNull(ItemsChoiceType72.RefNumberFilter, RefNumberFilter);
            items.AddNotNull(ItemsChoiceType72.RefNumberRangeFilter, RefNumberRangeFilter);
            items.AddNotNull(ItemsChoiceType72.TxnDateRangeFilter, TxnDateRangeFilter);

            obj.ItemsElementName = items.GetNames();
            obj.Items = items.GetItems();

            if (IncludeLineItems != null)
            {
                obj.IncludeLineItems = IncludeLineItems.ToString();
            }

            if (IncludeLinkedTxns != null)
            {
                obj.IncludeLinkedTxns = IncludeLinkedTxns.ToString();
            }

            if (IncludeRetElement != null)
            {
                obj.IncludeRetElement = IncludeRetElement.ToArray();
            }

            if (OwnerID != null)
            {
                obj.OwnerID = OwnerID
                    .Select(m => m.ToString()).ToArray();
            }
        }
    }
}
