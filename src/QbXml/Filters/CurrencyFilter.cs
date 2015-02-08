using QBSync.QbXml.Struct;
using QBSync.QbXml.Type;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using QBSync.QuickbooksDesktopSync.Extensions;

namespace QBSync.QbXml.Filters
{
    public class CurrencyFilter
    {
        public IEnumerable<IdType> ListId
        {
            get;
            set;
        }

        public IEnumerable<StrType> FullName
        {
            get;
            set;
        }

        public void AppendXml(XmlElement parent)
        {
            CheckFilters();

            if (ListId != null)
            {
                parent.AppendTags("ListId", ListId);
            }

            if (FullName != null)
            {
                parent.AppendTags("FullName", FullName);
            }
        }

        private void CheckFilters()
        {
            if (ListId != null && FullName != null)
            {
                throw new ArgumentException("You cannot set ListId or FullName at the same time.");
            }
        }
    }
}
