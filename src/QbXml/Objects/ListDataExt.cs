using QbSync.QbXml.Type;
using System.Xml;
using QbSync.QbXml.Extensions;
using QbSync.QbXml.Struct;

namespace QbSync.QbXml.Objects
{
    public class ListDataExt
    {
        public ListDataExtType ListDataExtType { get; set; }
        public Ref ListObjRef { get; set; }

        public void AppendXml(XmlElement parent)
        {
            parent.AppendChild(parent.OwnerDocument.CreateElementWithValue("ListDataExtType", ListDataExtType.ToString()));

            var listObjRef = parent.OwnerDocument.CreateElement("ListObjRef");
            parent.AppendChild(listObjRef);

            ListObjRef.AppendXml(listObjRef);
        }
    }
}