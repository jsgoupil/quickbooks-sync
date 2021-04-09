using QbSync.QbXml.Objects;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;

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
        /// <param name="events">XmlDeserializationEvents that could be triggered while deserializing.</param>
        /// <returns>QBXML object.</returns>
        public QBXML ParseResponseRaw(string response, XmlDeserializationEvents? events = null)
        {
            var reader = new StringReader(response);
            var qbXml = events.HasValue
                ? QbXmlSerializer.Instance.XmlSerializer.Deserialize(XmlReader.Create(reader), events.Value) as QBXML
                : QbXmlSerializer.Instance.XmlSerializer.Deserialize(reader) as QBXML;
            return qbXml ?? throw new System.ArgumentException("We were not able to materialize the QBXML.", nameof(response));
        }

        /// <summary>
        /// Gets a single item from the XML response.
        /// </summary>
        /// <typeparam name="T">Object to get.</typeparam>
        /// <param name="response">XML.</param>
        /// <param name="events">XmlDeserializationEvents that could be triggered while deserializing.</param>
        /// <returns>Object instance.</returns>
        public T? GetSingleItemFromResponse<T>(string response, XmlDeserializationEvents? events = null)
            where T : class
        {
            return GetSingleItemFromResponse(response, typeof(T), events) as T;
        }

        /// <summary>
        /// Gets multiple items from the XML response.
        /// </summary>
        /// <typeparam name="T">Objects to get.</typeparam>
        /// <param name="response">XML.</param>
        /// <param name="events">XmlDeserializationEvents that could be triggered while deserializing.</param>
        /// <returns>Object instances.</returns>
        public IEnumerable<T> GetItemsFromResponse<T>(string response, XmlDeserializationEvents? events = null)
            where T : class
        {
            return GetItemsFromResponse(response, typeof(T), events).Cast<T>();
        }

        /// <summary>
        /// Gets a single item from the XML response.
        /// </summary>
        /// <param name="response">XML.</param>
        /// <param name="type">Object to get.</param>
        /// <param name="events">XmlDeserializationEvents that could be triggered while deserializing.</param>
        /// <returns>Object instance.</returns>
        public object? GetSingleItemFromResponse(string response, System.Type type, XmlDeserializationEvents? events = null)
        {
            return GetItemsFromResponse(response, type, events).FirstOrDefault();
        }

        /// <summary>
        /// Gets multiple items from the XML response.
        /// </summary>
        /// <param name="response">XML.</param>
        /// <param name="type">Objects to get.</param>
        /// <param name="events">XmlDeserializationEvents that could be triggered while deserializing.</param>
        /// <returns>Object instances.</returns>
        public IEnumerable<object> GetItemsFromResponse(string response, System.Type type, XmlDeserializationEvents? events = null)
        {
            var qbXml = ParseResponseRaw(response, events);
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