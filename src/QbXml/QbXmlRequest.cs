using QbSync.QbXml.Objects;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace QbSync.QbXml
{
    public class QbXmlRequest
    {
        private List<QBXMLMsgsRq> qbxmlMsgsRqList;

        public QbXmlRequest()
        {
            qbxmlMsgsRqList = new List<QBXMLMsgsRq>();
        }

        public void Add(params QBXMLMsgsRq[] messages)
        {
            qbxmlMsgsRqList.AddRange(messages);
        }

        public void AddToSingle(params object[] requests)
        {
            var list = new List<object>(requests.Count());
            foreach (var request in requests)
            {
                list.Add(request);
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

            using (var textWriter = new StringWriter())
            {
                QbXmlSerializer.Instance.XmlSerializer.Serialize(textWriter, qbXml);
                return textWriter.ToString();
            }
        }
    }
}