using QbSync.QbXml.Extensions;
using QbSync.QbXml.Type;
using System.Xml;

namespace QbSync.QbXml.Objects
{
    public class Address : AddressBlock, IXmlConvertible
    {
        public StrType City { get; set; }
        public StrType State { get; set; }
        public StrType PostalCode { get; set; }
        public StrType Country { get; set; }
        public StrType Note { get; set; }

        public override void AppendXml(XmlElement parent)
        {
            base.AppendXml(parent);
            parent.AppendTagIfNotNull("City", City);
            parent.AppendTagIfNotNull("State", State);
            parent.AppendTagIfNotNull("PostalCode", PostalCode);
            parent.AppendTagIfNotNull("Country", Country);
            parent.AppendTagIfNotNull("Note", Note);
        }
    }
}
