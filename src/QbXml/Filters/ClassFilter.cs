using QbSync.QbXml.Extensions;
using QbSync.QbXml.Type;
using System;
using System.Collections.Generic;
using System.Xml;

namespace QbSync.QbXml.Filters
{
    public class ClassFilter
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

        public IdType ListIdWithChildren
        {
            get;
            set;
        }

        public StrType FullNameWithChildren
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

            if (ListIdWithChildren != null)
            {
                parent.AppendTag("ListIdWithChildren", ListIdWithChildren);
            }

            if (FullNameWithChildren != null)
            {
                parent.AppendTag("FullNameWithChildren", FullNameWithChildren);
            }
        }

        private void CheckFilters()
        {
            if (ListId != null && FullName != null && ListIdWithChildren != null && FullNameWithChildren != null)
            {
                throw new ArgumentException("You cannot set ListId, FullName, ListIdWithChildren, or FullNameWithChildren at the same time.");
            }
        }
    }
}
