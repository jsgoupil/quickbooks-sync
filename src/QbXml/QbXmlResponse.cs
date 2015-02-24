using QbSync.QbXml.Objects;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace QbSync.QbXml
{
    public class QbXmlResponse
    {
        public QBXML ParseResponseRaw(string response)
        {
            var reader = new StringReader(response);
            return QbXmlSerializer.Instance.XmlSerializer.Deserialize(reader) as QBXML;
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
                        if (innerItem.GetType() == type || innerItem.GetType().IsSubclassOf(type))
                        {
                            yield return innerItem;
                        }
                    }
                }
            }
        }
    }
}