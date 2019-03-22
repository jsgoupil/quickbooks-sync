using QbSync.QbXml.Objects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace QbSync.QbXml
{
    /// <summary>
    /// Base XML object which holds the response.
    /// </summary>
    public class QbXmlResponse
    {
        /// <summary>
        /// Creates a QbXmlResponse.
        /// </summary>
        public QbXmlResponse()
        {
        }

        /// <summary>
        /// Parses a string and returns a QBXML object.
        /// </summary>
        /// <param name="response">XML.</param>
        /// <returns>QBXML object.</returns>
        public QBXML ParseResponseRaw(string response)
        {
            var reader = new StringReader(response);
            var ret = QbXmlSerializer.Instance.XmlSerializer.Deserialize(reader) as QBXML;
            return ret;
        }

        /// <summary>
        /// Gets a single item from the XML response.
        /// </summary>
        /// <typeparam name="T">Object to get.</typeparam>
        /// <param name="response">XML.</param>
        /// <returns>Object instance.</returns>
        public T GetSingleItemFromResponse<T>(string response)
            where T : class
        {
            return GetSingleItemFromResponse(response, typeof(T)) as T;
        }

        /// <summary>
        /// Gets multiple items from the XML response.
        /// </summary>
        /// <typeparam name="T">Objects to get.</typeparam>
        /// <param name="response">XML.</param>
        /// <returns>Object instances.</returns>
        public IEnumerable<T> GetItemsFromResponse<T>(string response)
            where T : class
        {
            return GetItemsFromResponse(response, typeof(T)).Cast<T>();
        }

        /// <summary>
        /// Gets a single item from the XML response.
        /// </summary>
        /// <param name="response">XML.</param>
        /// <param name="type">Object to get.</param>
        /// <returns>Object instance.</returns>
        public object GetSingleItemFromResponse(string response, System.Type type)
        {
            return GetItemsFromResponse(response, type).FirstOrDefault();
        }

        /// <summary>
        /// Gets multiple items from the XML response.
        /// </summary>
        /// <param name="response">XML.</param>
        /// <param name="type">Objects to get</param>
        /// <returns>Object instances.</returns>
        public IEnumerable<object> GetItemsFromResponse(string response, System.Type type)
        {
            var qbXml = ParseResponseRaw(response);
            return SearchFor(qbXml, type);
        }

        private IEnumerable<object> SearchFor(QBXML qbXml, System.Type type)
        {
            foreach (var item in qbXml.Items)
            {
                if (item is QBXMLMsgsRs typedItem)
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