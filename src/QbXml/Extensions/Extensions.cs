using QbSync.QbXml.Type;
using System;
using System.Collections.Generic;
using System.Xml;

namespace QbSync.QbXml.Extensions
{
    public static class Extensions
    {
        public static void ForEach<T>(this IEnumerable<T> enumeration, Action<T> action)
        {
            foreach (T item in enumeration)
            {
                action(item);
            }
        }

        /// <summary>
        /// Returns null if we can't find the node.
        /// </summary>
        /// <param name="node"></param>
        /// <param name="xPath"></param>
        /// <returns></returns>
        public static string ReadNode(this XmlNode node, string xpath)
        {
            var selectedNode = node.SelectSingleNode(xpath);

            if (selectedNode != null)
            {
                return selectedNode.InnerText;
            }

            return null;
        }

        public static string ReadAttribute(this XmlNode node, string attributeName)
        {
            var attributeCollection = node.Attributes;
            var attribute = attributeCollection.GetNamedItem(attributeName);
            if (attribute != null)
            {
                return attribute.Value;
            }

            return null;
        }

        public static XmlElement CreateElementWithValue(this XmlDocument xmlDocument, string tagName, string innerText)
        {
            var elem = xmlDocument.CreateElement(tagName);
            elem.InnerText = innerText;
            return elem;
        }

        public static void AppendTag(this XmlElement xmlElement, string tagName, IStringConvertible innerText)
        {
            xmlElement.AppendChild(xmlElement.OwnerDocument.CreateElementWithValue(tagName, innerText.ToString()));
        }

        public static void AppendTags(this XmlElement xmlElement, string tagName, IEnumerable<IStringConvertible> objects)
        {
            objects.ForEach(innerText =>
            {
                xmlElement.AppendChild(xmlElement.OwnerDocument.CreateElementWithValue(tagName, innerText.ToString()));
            });
        }
    }
}
