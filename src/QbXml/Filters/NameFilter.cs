using QbSync.QbXml.Extensions;
using QbSync.QbXml.Struct;
using QbSync.QbXml.Type;
using System.Xml;

namespace QbSync.QbXml.Filters
{
    public class NameFilter : IXmlConvertible
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

        public virtual void AppendXml(XmlElement parent)
        {
            parent.AppendChild(parent.OwnerDocument.CreateElementWithValue("Criterion", Criterion.ToString()));
            parent.AppendTag("Name", Name);
        }
    }
}
