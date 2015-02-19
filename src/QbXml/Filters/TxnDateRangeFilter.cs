using QbSync.QbXml.Extensions;
using QbSync.QbXml.Struct;
using QbSync.QbXml.Type;
using System;
using System.Xml;

namespace QbSync.QbXml.Filters
{
    public class TxnDateRangeFilter
    {
        public DateType FromTxnDate { get; set; }
        public DateType ToTxnDate { get; set; }
        public DateMacro? DateMacro { get; set; }

        public void AppendXml(XmlElement parent)
        {
            if (DateMacro.HasValue && (FromTxnDate != null || ToTxnDate != null))
            {
                throw new ArgumentException("You cannot set DateMacro if FromTxnDate or ToTxnDate is set.");
            }

            if (FromTxnDate != null)
            {
                parent.AppendTag("FromTxnDate", FromTxnDate);
            }

            if (ToTxnDate != null)
            {
                parent.AppendTag("ToTxnDate", ToTxnDate);
            }

            if (DateMacro.HasValue)
            {
                parent.AppendChild(parent.OwnerDocument.CreateElementWithValue("DateMacro", DateMacro.Value.ToString()));
            }
        }
    }
}
