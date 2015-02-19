using QbSync.QbXml.Extensions;
using QbSync.QbXml.Type;
using System.Collections.Generic;

namespace QbSync.QbXml.Objects
{
    public class InvoiceLineAdd : InvoiceLineAddMod, IXmlConvertible
    {
        public LinkToTxn LinkToTxn { get; set; }
        public IEnumerable<DataExtBase> DataExtRet { get; set; }

        public override void AppendXml(System.Xml.XmlElement parent)
        {
            base.AppendXml(parent);
            parent.AppendTagIfNotNull("LinkToTxn", LinkToTxn);
            parent.AppendTagsIfNotNull("DataExtRet", DataExtRet);
        }
    }
}