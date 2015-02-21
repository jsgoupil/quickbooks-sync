using QbSync.QbXml.Objects;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using System.Linq;
using QbSync.QbXml.Messages.Responses;
using QbSync.QbXml.Wrappers;
using QbSync.QbXml.Extensions;

namespace QbSync.QbXml
{
    public class QbXmlResponse
    {
        static XmlSerializer serializer;
        static object locker = new object();

        public QbXmlResponse()
        {
            InitializeOverrideList();
        }

        public QBXML ParseResponseRaw(string response)
        {
            var reader = new StringReader(response);
            return serializer.Deserialize(reader) as QBXML;
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

        private void InitializeOverrideList()
        {
            lock (locker)
            {
                if (serializer == null)
                {
                    var typeOverrides = new List<TypeOverride>
                    {
                        new TypeOverride
                        {
                             OverrideType = typeof(QBXMLMsgsRs),
                             OverrideMember = "Items",
                             Attributes = new XmlAttributes
                             {
                                 XmlElements =
                                 {
                                     new XmlElementAttribute
                                     {
                                         ElementName = "CustomerQueryRs",
                                         Type = typeof(CustomerQueryRsTypeWrapper)
                                     }
                                 }
                             }
                         },
                         new TypeOverride
                         {
                             OverrideType = typeof(DataExtDelRsType),
                             OverrideMember = "DataExtDelRet",
                             Attributes = new XmlAttributes
                             {
                                 XmlElements =
                                 {
                                     new XmlElementAttribute
                                     {
                                         ElementName = "DataExtDelRet",
                                         Type = typeof(DataExtDelRetWrapper)
                                     }
                                 }
                             }
                         }
                    };

                    var xmlAttributeOverride = new XmlAttributeOverrides();
                    foreach (var typeOverride in typeOverrides)
                    {
                        xmlAttributeOverride.AddOnly(typeOverride.OverrideType, typeOverride.OverrideMember, typeOverride.Attributes);
                    }

                    serializer = new XmlSerializer(typeof(QBXML), xmlAttributeOverride);
                }
            }
        }
    }

    class TypeOverride
    {
        internal System.Type OverrideType { get; set; }
        internal string OverrideMember { get; set; }
        internal XmlAttributes Attributes { get; set; }
    }
}