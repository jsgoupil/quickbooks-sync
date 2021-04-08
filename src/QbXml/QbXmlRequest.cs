using QbSync.QbXml.Objects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;

namespace QbSync.QbXml
{
    /// <summary>
    /// Base XML object which holds the request.
    /// </summary>
    public class QbXmlRequest
    {
        /// <summary>
        /// Version used by QBXML.
        /// </summary>
        public static readonly Version VERSION = new Version(13, 0);

        private readonly List<QBXMLMsgsRq> qbxmlMsgsRqList;

        /// <summary>
        /// Creates a QbXmlRequest.
        /// </summary>
        public QbXmlRequest()
        {
            qbxmlMsgsRqList = new List<QBXMLMsgsRq>();
        }

        /// <summary>
        /// Adds a message.
        /// </summary>
        /// <param name="messages">The message.</param>
        public void Add(params QBXMLMsgsRq[] messages)
        {
            qbxmlMsgsRqList.AddRange(messages);
        }

        /// <summary>
        /// Adds requests to a single message.
        /// </summary>
        /// <param name="requests">The requests.</param>
        public void AddToSingle(params object[] requests)
        {
            Add(new QBXMLMsgsRq
            {
                Items = requests
            });
        }
        
        /// <summary>
        /// Gets the XML request.
        /// </summary>
        /// <returns>XML.</returns>
        public string GetRequest()
        {
            var qbXml = new QBXML
            {
                Items = qbxmlMsgsRqList.ToArray(),
                ItemsElementName = Enumerable.Repeat(ItemsChoiceType99.QBXMLMsgsRq, qbxmlMsgsRqList.Count()).ToArray()
            };

            using var writer = new StringWriter();
            using XmlWriter xmlWriter = new QbXmlTextWriter(writer);
            xmlWriter.WriteProcessingInstruction("xml", "version=\"1.0\" encoding=\"utf-8\"");
            xmlWriter.WriteProcessingInstruction("qbxml", string.Format("version=\"{0}.{1}\"", VERSION.Major, VERSION.Minor));
            var ns = new XmlSerializerNamespaces();
            ns.Add("", "");
            QbXmlSerializer.Instance.XmlSerializer.Serialize(xmlWriter, qbXml, ns);

            xmlWriter.Flush();
            return writer.ToString();
        }
    }
}