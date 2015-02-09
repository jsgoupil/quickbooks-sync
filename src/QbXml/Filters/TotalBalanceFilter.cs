using QbSync.QbXml.Struct;
using QbSync.QbXml.Type;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using QbSync.QuickbooksDesktopSync.Extensions;

namespace QbSync.QbXml.Filters
{
    public class TotalBalanceFilter
    {
        public Operator Operator
        {
            get;
            set;
        }

        public AmtType Amount
        {
            get;
            set;
        }

        public void AppendXml(XmlElement parent)
        {
            parent.AppendChild(parent.OwnerDocument.CreateElementWithValue("Operator", Operator.ToString()));
            parent.AppendTag("Amount", Amount);
        }
    }
}
