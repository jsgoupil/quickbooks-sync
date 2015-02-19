using QbSync.QbXml.Extensions;
using QbSync.QbXml.Type;
using System;
using System.Collections.Generic;
using System.Xml;

namespace QbSync.QbXml.Filters
{
    public class BaseFilter
    {
        public IEnumerable<IdType> ListID { get; set; }
        public IEnumerable<StrType> FullName { get; set; }

        public virtual void AppendXml(XmlElement parent)
        {
            if (ListID != null || FullName != null)
            {
                throw new ArgumentException("You cannot set ListID and FullName at the same time.");
            }

            if (ListID != null)
            {
                parent.AppendTags("ListID", ListID);
            }

            if (FullName != null)
            {
                parent.AppendTags("FullName", FullName);
            }
        }
    }
}
