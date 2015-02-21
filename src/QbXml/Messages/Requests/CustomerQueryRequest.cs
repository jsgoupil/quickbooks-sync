using QbSync.QbXml.Objects;
using QbSync.QbXml.Type;
using System.Collections.Generic;
using System.Linq;

namespace QbSync.QbXml.Messages.Requests
{
    public class CustomerQueryRequest : QbXmlIterator<CustomerQueryRqType>
    {
        public IEnumerable<string> ListID { get; set; }
        public IEnumerable<string> FullName { get; set; }
        public ActiveStatus? ActiveStatus { get; set; }
        public DateTimeType FromModifiedDate { get; set; }
        public DateTimeType ToModifiedDate { get; set; }
        public NameFilter NameFilter { get; set; }
        public NameRangeFilter NameRangeFilter { get; set; }
        public TotalBalanceFilter TotalBalanceFilter { get; set; }
        public CurrencyFilter CurrencyFilter { get; set; }
        public ClassFilter ClassFilter { get; set; }

        public IEnumerable<string> IncludeRetElement { get; set; }
        public IEnumerable<GuidType> OwnerID { get; set; }

        protected override void ProcessObj(CustomerQueryRqType obj)
        {
            base.ProcessObj(obj);

            var items = new ItemWithName<ItemsChoiceType32>();

            if (ListID != null)
            {
                foreach (var item in ListID)
                {
                    items.Add(ItemsChoiceType32.ListID, item);
                }
            }

            if (FullName != null)
            {
                foreach (var item in FullName)
                {
                    items.Add(ItemsChoiceType32.FullName, item);
                }
            }

            items.AddNotNull(ItemsChoiceType32.ActiveStatus, ActiveStatus);
            items.AddNotNull(ItemsChoiceType32.FromModifiedDate, FromModifiedDate);
            items.AddNotNull(ItemsChoiceType32.ToModifiedDate, ToModifiedDate);
            items.AddNotNull(ItemsChoiceType32.NameFilter, NameFilter);
            items.AddNotNull(ItemsChoiceType32.NameRangeFilter, NameRangeFilter);
            items.AddNotNull(ItemsChoiceType32.TotalBalanceFilter, TotalBalanceFilter);
            items.AddNotNull(ItemsChoiceType32.CurrencyFilter, CurrencyFilter);
            items.AddNotNull(ItemsChoiceType32.ClassFilter, ClassFilter);

            if (MaxReturned.HasValue)
            {
                items.Add(ItemsChoiceType32.MaxReturned, MaxReturned.Value.ToString());
            }

            if (Iterator.HasValue)
            {
                obj.iteratorSpecified = true;
                obj.iterator = IteratorMapper.Map<CustomerQueryRqTypeIterator>(Iterator.Value);
                obj.iteratorID = IteratorID;
            }

            obj.ItemsElementName = items.GetNames();
            obj.Items = items.GetItems();

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