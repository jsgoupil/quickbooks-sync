using QbSync.QbXml.Objects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace QbSync.QbXml
{
    public class QbXmlResponse
    {
        protected QbXmlResponseOptions qbXmlResponseOptions;

        [ThreadStatic]
        internal static QbXmlResponseOptions qbXmlResponseOptionsStatic;

        public QbXmlResponse()
            : this(null)
        {
        }

        public QbXmlResponse(QbXmlResponseOptions qbXmlResponseOptions)
        {
            this.qbXmlResponseOptions = qbXmlResponseOptions;
        }

        public QBXML ParseResponseRaw(string response)
        {
            var reader = new StringReader(response);
            qbXmlResponseOptionsStatic = qbXmlResponseOptions;
            var ret = QbXmlSerializer.Instance.XmlSerializer.Deserialize(reader) as QBXML;
            qbXmlResponseOptionsStatic = null;
            return ret;
        }

        public T GetSingleItemFromResponse<T>(string response)
            where T : class
        {
            return GetSingleItemFromResponse(response, typeof(T)) as T;
        }

        public IEnumerable<T> GetItemsFromResponse<T>(string response)
            where T : class
        {
            return GetItemsFromResponse(response, typeof(T)).Cast<T>();
        }

        public object GetSingleItemFromResponse(string response, System.Type type)
        {
            return GetItemsFromResponse(response, type).FirstOrDefault();
        }

        public IEnumerable<object> GetItemsFromResponse(string response, System.Type type)
        {
            var qbXml = ParseResponseRaw(response);
            return SearchFor(qbXml, type);
        }

        private IEnumerable<object> SearchFor(QBXML qbXml, System.Type type)
        {
            foreach (var item in qbXml.Items)
            {
                var typedItem = item as QBXMLMsgsRs;
                if (typedItem != null)
                {
                    foreach (var innerItem in typedItem.Items)
                    {
                        if (innerItem.GetType() == type || type.IsAssignableFrom(innerItem.GetType()))
                        {
                            yield return innerItem;
                        }
                    }
                }
            }
        }
    }
}