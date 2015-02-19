using QbSync.QbXml.Extensions;
using QbSync.QbXml.Filters;
using QbSync.QbXml.Objects;
using QbSync.QbXml.Struct;
using QbSync.QbXml.Type;
using System;
using System.Collections.Generic;
using System.Xml;

namespace QbSync.QbXml.Messages.Requests
{
    public class InvoiceQueryRequest : IteratorRequest
    {
        public InvoiceQueryRequest()
            : base("InvoiceQueryRq")
        {
            Filter = InvoiceQueryRequestFilter.None;
        }

        public InvoiceQueryRequestFilter Filter { get; set; }

        public IEnumerable<IdType> TxnID { get; set; }
        public IEnumerable<StrType> RefNumber { get; set; }
        public IEnumerable<StrType> RefNumberCaseSensitive { get; set; }

        public ModifiedDateRangeFilter ModifiedDateRangeFilter { get; set; }
        public TxnDateRangeFilter TxnDateRangeFilter { get; set; }

        public BaseFilterWithChildren EntityFilter { get; set; }
        public BaseFilterWithChildren AccountFilter { get; set; }

        public RefNumberFilter RefNumberFilter { get; set; }
        public RefNumberRangeFilter RefNumberRangeFilter { get; set; }

        public BaseFilter CurrencyFilter { get; set; }

        public PaidStatus? PaidStatus { get; set; }

        public BoolType IncludeLineItems { get; set; }
        public BoolType IncludeLinkedTxns { get; set; }

        public IEnumerable<StrType> IncludeRetElement { get; set; }
        public GuidType OwnerID { get; set; }

        protected override void BuildRequest(XmlElement parent)
        {
            base.BuildRequest(parent);

            var doc = parent.OwnerDocument;

            if (Filter == InvoiceQueryRequestFilter.TxnID)
            {
                if (TxnID != null)
                {
                    parent.AppendTags("TxnID", TxnID);
                }
            }
            else if (Filter == InvoiceQueryRequestFilter.RefNumber)
            {
                if (RefNumber != null)
                {
                    parent.AppendTags("RefNumber", RefNumber);
                }
            }
            else if (Filter == InvoiceQueryRequestFilter.RefNumberCaseSensitive)
            {
                if (RefNumberCaseSensitive != null)
                {
                    parent.AppendTags("RefNumberCaseSensitive", RefNumberCaseSensitive);
                }
            }
            else
            {
                if (MaxReturned != null)
                {
                    parent.AppendTag("MaxReturned", MaxReturned);
                }

                if (ModifiedDateRangeFilter != null && TxnDateRangeFilter != null)
                {
                    throw new ArgumentException("You cannot set ModifiedDateRangeFilter and TxnDateRangeFilter at the same time.");
                }

                if (ModifiedDateRangeFilter != null)
                {
                    var modifiedDateRangeFilter = doc.CreateElement("ModifiedDateRangeFilter");
                    parent.AppendChild(modifiedDateRangeFilter);

                    ModifiedDateRangeFilter.AppendXml(modifiedDateRangeFilter);
                }
                else if (TxnDateRangeFilter != null)
                {
                    var txnDateRangeFilter = doc.CreateElement("TxnDateRangeFilter");
                    parent.AppendChild(txnDateRangeFilter);

                    TxnDateRangeFilter.AppendXml(txnDateRangeFilter);
                }

                if (EntityFilter != null)
                {
                    var entityFilter = doc.CreateElement("EntityFilter");
                    parent.AppendChild(entityFilter);

                    EntityFilter.AppendXml(entityFilter);
                }

                if (AccountFilter != null)
                {
                    var accountFilter = doc.CreateElement("AccountFilter");
                    parent.AppendChild(accountFilter);

                    AccountFilter.AppendXml(accountFilter);
                }

                if (RefNumberFilter != null && RefNumberRangeFilter != null)
                {
                    throw new ArgumentException("You cannot set RefNumberFilter and RefNumberRangeFilter at the same time.");
                }

                if (RefNumberFilter != null)
                {
                    var refNumberFilter = doc.CreateElement("RefNumberFilter");
                    parent.AppendChild(refNumberFilter);

                    RefNumberFilter.AppendXml(refNumberFilter);
                }
                else if (RefNumberRangeFilter != null)
                {
                    var refNumberRangeFilter = doc.CreateElement("RefNumberRangeFilter");
                    parent.AppendChild(refNumberRangeFilter);

                    RefNumberRangeFilter.AppendXml(refNumberRangeFilter);
                }

                if (CurrencyFilter != null)
                {
                    var currencyFilter = doc.CreateElement("CurrencyFilter");
                    parent.AppendChild(currencyFilter);

                    CurrencyFilter.AppendXml(currencyFilter);
                }

                if (PaidStatus.HasValue)
                {
                    parent.AppendChild(parent.OwnerDocument.CreateElementWithValue("PaidStatus", PaidStatus.Value.ToString()));
                }
            }

            if (IncludeLineItems != null)
            {
                parent.AppendTag("IncludeLineItems", IncludeLineItems);
            }

            if (IncludeLinkedTxns != null)
            {
                parent.AppendTag("IncludeLinkedTxns", IncludeLinkedTxns);
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
