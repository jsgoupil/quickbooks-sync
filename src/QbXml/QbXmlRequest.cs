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
        public static readonly Version VERSION = new Version(16, 0);

        private readonly List<QBXMLMsgsRq> qbxmlMsgsRqList;
        private readonly Version version;

        /// <summary>
        /// Creates a QbXmlRequest.
        /// </summary>
        public QbXmlRequest()
            : this(VERSION)
        {
            qbxmlMsgsRqList = new List<QBXMLMsgsRq>();
        }

        /// <summary>
        /// Creates a QbXmlRequest with a specific version.
        /// </summary>
        /// <param name="version">The requested QBXML version.</param>
        public QbXmlRequest(Version version)
        {
            qbxmlMsgsRqList = new List<QBXMLMsgsRq>();
            this.version = version;
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
                ItemsElementName = Enumerable.Repeat(ItemsChoiceType106.QBXMLMsgsRq, qbxmlMsgsRqList.Count()).ToArray()
            };

            using var writer = new StringWriter();
            using XmlWriter xmlWriter = new QbXmlTextWriter(writer);
            xmlWriter.WriteProcessingInstruction("xml", "version=\"1.0\" encoding=\"utf-8\"");
            xmlWriter.WriteProcessingInstruction("qbxml", string.Format("version=\"{0}.{1}\"", version.Major, version.Minor));
            var ns = new XmlSerializerNamespaces();
            ns.Add("", "");
            QbXmlSerializer.Instance.XmlSerializer.Serialize(xmlWriter, qbXml, ns);

            xmlWriter.Flush();
            return writer.ToString();
        }
    }
}