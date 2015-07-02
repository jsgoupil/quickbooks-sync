using QbSync.QbXml.Objects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace QbSync.QbXml
{
    public class QbXmlRequest
    {
        public static readonly Version VERSION = new Version(13, 0);
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

            using (MemoryStream memoryStream = new MemoryStream())
            using (XmlWriter xmlWriter = new QbXmlTextWriter(memoryStream, Encoding.UTF8))
            {
                xmlWriter.WriteProcessingInstruction("xml", "version=\"1.0\" encoding=\"utf-8\"");
                xmlWriter.WriteProcessingInstruction("qbxml", string.Format("version=\"{0}.{1}\"", VERSION.Major, VERSION.Minor));
                var ns = new XmlSerializerNamespaces();
                ns.Add("", "");
                QbXmlSerializer.Instance.XmlSerializer.Serialize(xmlWriter, qbXml, ns);

                xmlWriter.Flush();
                memoryStream.Position = 0;
                var streamReader = new StreamReader(memoryStream);
                return streamReader.ReadToEnd();
            }
        }
    }
}