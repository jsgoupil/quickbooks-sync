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
    public class NameFilter
    {
        public MatchCriterion Criterion
        {
            get;
            set;
        }

        public StrType Name
        {
            get;
            set;
        }

        public void AppendXml(XmlElement parent)
        {
            parent.AppendChild(parent.OwnerDocument.CreateElementWithValue("Criterion", Criterion.ToString()));
            parent.AppendTag("Name", Name);
        }
    }
}
