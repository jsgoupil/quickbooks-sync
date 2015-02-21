using QbSync.QbXml.Messages.Requests;
using QbSync.QbXml.Objects;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace QbSync.QbXml
{
    public class QbXmlRequest
    {
        private System.Type qbXmlType;
        private List<QBXMLMsgsRq> qbxmlMsgsRqList;

        public QbXmlRequest()
        {
            qbXmlType = typeof(QBXML);
            qbxmlMsgsRqList = new List<QBXMLMsgsRq>();
        }

        public void Add(params QBXMLMsgsRq[] messages)
        {
            qbxmlMsgsRqList.AddRange(messages);
        }

        public void AddToSingle(params QbRequestWrapper[] requests) {

            var list = new List<object>(requests.Count());
            foreach (var request in requests)
            {
                list.Add(request.GetQbObject());
            }

            Add(new QBXMLMsgsRq
            {
                Items = list.ToArray()
            });
        }

        public string GetRequest()
        {
            var qbXml = new QBXML
            {
                Items = qbxmlMsgsRqList.ToArray(),
                ItemsElementName = Enumerable.Repeat<ItemsChoiceType99>(ItemsChoiceType99.QBXMLMsgsRq, qbxmlMsgsRqList.Count()).ToArray()
            };

            var xmlSerializer = new XmlSerializer(qbXmlType);
            using (var textWriter = new StringWriter())
            {
                xmlSerializer.Serialize(textWriter, qbXml);
                return textWriter.ToString();
            }
        }
    }
}