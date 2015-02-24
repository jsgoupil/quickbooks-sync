using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;

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

        public static void AddOnly(this XmlAttributeOverrides xmlAttributeOverrides, System.Type type, string member, XmlAttributes attributes)
        {
            var customAttributes = type.GetProperty(member).GetCustomAttributes(typeof(XmlElementAttribute), false) as XmlElementAttribute[];

            var xmlAttributes = new XmlAttributes
            {
                XmlAnyAttribute = attributes.XmlAnyAttribute,
                XmlArray = attributes.XmlArray,
                XmlAttribute = attributes.XmlAttribute,
                XmlDefaultValue = attributes.XmlDefaultValue,
                XmlEnum = attributes.XmlEnum,
                XmlIgnore = attributes.XmlIgnore,
                Xmlns = attributes.Xmlns,
                XmlRoot = attributes.XmlRoot,
                XmlText = attributes.XmlText,
                XmlType = attributes.XmlType
            };

            var existingXmlElementAttributes = new List<XmlElementAttribute>(attributes.XmlElements.Count + customAttributes.Count());
            existingXmlElementAttributes.AddRange(attributes.XmlElements.Cast<XmlElementAttribute>());

            foreach (var customAttribute in customAttributes)
            {
                existingXmlElementAttributes.Add(customAttribute);
            }

            foreach (var xmlElementAttribute in existingXmlElementAttributes.Distinct(new XmlOverrider.XmlElementAttributeEqualityComparer()))
            {
                xmlAttributes.XmlElements.Add(xmlElementAttribute);
            }

            xmlAttributeOverrides.Add(type, member, xmlAttributes);
        }

        public static bool IsSubclassOf(this System.Type type, System.Type baseType)
        {
            if (type == null || baseType == null || type == baseType)
                return false;

            if (baseType.IsGenericType == false)
            {
                if (type.IsGenericType == false)
                    return type.IsSubclassOf(baseType);
            }
            else
            {
                baseType = baseType.GetGenericTypeDefinition();
            }

            type = type.BaseType;
            var objectType = typeof(object);
            while (type != objectType && type != null)
            {
                var curentType = type.IsGenericType ?
                    type.GetGenericTypeDefinition() : type;
                if (curentType == baseType)
                    return true;

                type = type.BaseType;
            }

            return false;
        }
    }

    static class XmlOverrider
    {
        public class XmlElementAttributeEqualityComparer : IEqualityComparer<XmlElementAttribute>
        {
            public bool Equals(XmlElementAttribute x, XmlElementAttribute y)
            {
                return x.ElementName == y.ElementName;
            }

            public int GetHashCode(XmlElementAttribute obj)
            {
                return obj.ElementName.GetHashCode();
            }
        }
    }
}
