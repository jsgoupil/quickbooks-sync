using QbSync.QbXml.Objects;
using System.Xml.Serialization;

namespace QbSync.QbXml
{
    /// <summary>
    /// Serializer.
    /// </summary>
    public sealed class QbXmlSerializer
    {
        private static volatile QbXmlSerializer instance;
        private static object syncRoot = new System.Object();

        /// <summary>
        /// Creates a serializer.
        /// </summary>
        public QbXmlSerializer()
        {
            Initialize();
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

        private void Initialize()
        {
            XmlSerializer = new XmlSerializer(typeof(QBXML));
        }
    }
}