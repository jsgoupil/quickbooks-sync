using QbSync.QbXml.Extensions;
using QbSync.QbXml.Objects;
using QbSync.QbXml.Struct;
using QbSync.QbXml.Type;
using System.Xml;

namespace QbSync.QbXml.Messages.Requests
{
    public abstract class DataExtRequest : QbXmlRequest
    {
        public DataExtRequest(string rootElementName)
            : base(rootElementName)
        {
            Filter = DataExtFilter.Other;
        }

        public GuidType OwnerID { get; set; }
        public StrType DataExtName { get; set; }
        public DataExtFilter Filter { get; set; }

        public ListDataExt ListDataExt { get; set; }
        public TxnDataExt TxnDataExt { get; set; }
        public OtherDataExtType? OtherDataExtType { get; set; }

        protected override void BuildRequest(XmlElement parent)
        {
            base.BuildRequest(parent);

            parent.AppendTag("OwnerID", OwnerID);
            parent.AppendTag("DataExtName", DataExtName);

            if (Filter == DataExtFilter.List)
            {
                ListDataExt.AppendXml(parent);
            }
            else if (Filter == DataExtFilter.Txn)
            {
                TxnDataExt.AppendXml(parent);
            }
            else if (Filter == DataExtFilter.Other && OtherDataExtType.HasValue)
            {
                parent.AppendChild(parent.OwnerDocument.CreateElementWithValue("OtherDataExtType", OtherDataExtType.ToString()));
            }
        }
    }
}
