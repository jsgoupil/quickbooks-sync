using QbSync.QbXml.Extensions;
using QbSync.QbXml.Filters;
using QbSync.QbXml.Struct;
using QbSync.QbXml.Type;
using System;
using System.Collections.Generic;
using System.Xml;

namespace QbSync.QbXml.Messages.Requests
{
    public class CustomerQueryRequest : IteratorRequest
    {
        public CustomerQueryRequest()
            : base("CustomerQueryRq")
        {
            Filter = CustomerQueryRequestFilter.None;
        }

        public CustomerQueryRequestFilter Filter { get; set; }
        public IEnumerable<IdType> ListID { get; set; }
        public IEnumerable<StrType> FullName { get; set; }
        public ActiveStatus? ActiveStatus { get; set; }
        public DateTimeType FromModifiedDate { get; set; }
        public DateTimeType ToModifiedDate { get; set; }
        public NameFilter NameFilter { get; set; }
        public NameRangeFilter NameRangeFilter { get; set; }
        public TotalBalanceFilter TotalBalanceFilter { get; set; }
        public CurrencyFilter CurrencyFilter { get; set; }
        public ClassFilter ClassFilter { get; set; }
        public IEnumerable<StrType> IncludeRetElement { get; set; }
        public GuidType OwnerID { get; set; }

        protected override void BuildRequest(XmlElement parent)
        {
            base.BuildRequest(parent);

            var doc = parent.OwnerDocument;

            if (Filter == CustomerQueryRequestFilter.ListId)
            {
                if (ListID != null)
                {
                    parent.AppendTags("ListID", ListID);
                }
            }
            else if (Filter == CustomerQueryRequestFilter.FullName)
            {
                if (FullName != null)
                {
                    parent.AppendTags("FullName", FullName);
                }
            }
            else
            {
                if (MaxReturned != null)
                {
                    parent.AppendTag("MaxReturned", MaxReturned);
                }

                if (ActiveStatus.HasValue)
                {
                    parent.AppendChild(doc.CreateElementWithValue("ActiveStatus", ActiveStatus.Value.ToString()));
                }

                if (FromModifiedDate != null)
                {
                    parent.AppendTag("FromModifiedDate", FromModifiedDate);
                }

                if (ToModifiedDate != null)
                {
                    parent.AppendTag("ToModifiedDate", ToModifiedDate);
                }

                if (NameFilter != null && NameRangeFilter != null)
                {
                    throw new ArgumentException("You cannot set NameFilter and NameRangeFilter at the same time.");
                }

                if (NameFilter != null)
                {
                    parent.AppendTag("NameFilter", NameFilter);

                }
                else if (NameRangeFilter != null)
                {
                    parent.AppendTag("NameRangeFilter", NameRangeFilter);
                }

                if (TotalBalanceFilter != null)
                {
                    parent.AppendTag("TotalBalanceFilter", TotalBalanceFilter);
                }

                if (CurrencyFilter != null)
                {
                    parent.AppendTag("CurrencyFilter", CurrencyFilter);
                }

                if (ClassFilter != null)
                {
                    parent.AppendTag("ClassFilter", ClassFilter);
                }
            }

            if (IncludeRetElement != null)
            {
                parent.AppendTags("IncludeRetElement", IncludeRetElement);
            }

            if (OwnerID != null)
            {
                parent.AppendTag("OwnerID", OwnerID);
            }
        }
    }
}
