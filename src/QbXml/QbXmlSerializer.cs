using QbSync.QbXml.Objects;
using System.Xml.Serialization;

namespace QbSync.QbXml
{
    public sealed class QbXmlSerializer
    {
        private static volatile QbXmlSerializer instance;
        private static object syncRoot = new System.Object();

        public QbXmlSerializer()
        {
            Initialize();
        }

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