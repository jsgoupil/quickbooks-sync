using QbSync.QbXml.Extensions;
using QbSync.QbXml.Type;
using System.Collections.Generic;
using System.Xml;

namespace QbSync.QbXml.Objects
{
    public class InvoiceLineGroupAdd : InvoiceLineGroupAddMod, IXmlConvertible
    {
        public Ref InventorySiteRef { get; set; }
        public Ref InventorySiteLocationRef { get; set; }
        public IEnumerable<DataExtBase> DataExt { get; set; }

        public override void AppendXml(XmlElement parent)
        {
            base.AppendXml(parent);
            parent.AppendTagIfNotNull("InventorySiteRef", InventorySiteRef);
            parent.AppendTagIfNotNull("InventorySiteLocationRef", InventorySiteLocationRef);
            parent.AppendTagsIfNotNull("DataExt", DataExt);
        }
    }
}
