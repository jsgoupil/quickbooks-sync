using QbSync.QbXml.Objects;
using System.Xml.Serialization;

namespace QbSync.QbXml
{
    /// <summary>
    /// Serializer.
    /// </summary>
    public sealed class QbXmlSerializer
    {
        private static volatile QbXmlSerializer? instance;
        private static readonly object syncRoot = new object();

        /// <summary>
        /// Creates a serializer.
        /// </summary>
        public QbXmlSerializer()
        {
            XmlSerializer = new XmlSerializer(typeof(QBXML));
        }

        /// <summary>
        /// Gets the serializer.
        /// </summary>
        public static QbXmlSerializer Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                        {
                            instance = new QbXmlSerializer();
                        }
                    }
                }

                return instance;
            }
        }

        /// <summary>
        /// Gets the XmlSerializer.
        /// </summary>
        public XmlSerializer XmlSerializer
        {
            get;
            private set;
        }
    }
}