using QbSync.QbXml.Type;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using QbSync.QbXml.Extensions;

namespace QbSync.QbXml.Filters
{
    public class BaseFilterWithChildren : BaseFilter
    {
        public IdType ListIDWithChildren { get; set; }
        public StrType FullNameWithChildren { get; set; }

        public void AppendXml(XmlElement parent)
        {
            base.AppendXml(parent);

            if (ListIDWithChildren != null || FullNameWithChildren != null)
            {
                throw new ArgumentException("You cannot set ListIDWithChildren and FullNameWithChildren at the same time.");
            }

            if (ListIDWithChildren != null)
            {
                parent.AppendTag("ListIDWithChildren", ListIDWithChildren);
            }

            if (FullNameWithChildren != null)
            {
                parent.AppendTag("FullNameWithChildren", FullNameWithChildren);
            }
        }
    }
}
