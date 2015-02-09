using QbSync.QbXml.Struct;
using QbSync.QbXml.Type;
using QbSync.QuickbooksDesktopSync.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml;

namespace QbSync.QbXml
{
    public class QbXmlResponse<T>
    {
        protected string rootElementName;

        public QbXmlResponse(string rootElementName)
        {
            this.rootElementName = rootElementName;
        }

        public QbXmlMsgResponse<T> ParseResponse(string response)
        {
            // Parse the response XML string into an XmlDocument
            XmlDocument responseXmlDoc = new XmlDocument();
            responseXmlDoc.LoadXml(response);

            // Get the response for our request
            XmlNodeList rs = responseXmlDoc.GetElementsByTagName(rootElementName);

            if (rs.Count == 1) // Should always be true since we only did one request in this sample
            {
                XmlNode responseNode = rs.Item(0);

                // Check the status code, info, and severity
                XmlAttributeCollection rsAttributes = responseNode.Attributes;
                var qbXmlResponse = new QbXmlMsgResponse<T>
                {
                    RequestId = Convert.ToInt32(rsAttributes.GetNamedItem("requestID").Value),
                    StatusCode = Convert.ToInt32(rsAttributes.GetNamedItem("statusCode").Value),
                    StatusSeverity = (StatusSeverity)Enum.Parse(typeof(StatusSeverity), rsAttributes.GetNamedItem("statusSeverity").Value),
                    StatusMessage = rsAttributes.GetNamedItem("statusMessage").Value,
                };

                ProcessResponse(responseNode, qbXmlResponse);

                return qbXmlResponse;
            }

            return null;
        }

        protected virtual void ProcessResponse(XmlNode responseNode, QbXmlMsgResponse<T> qbXmlResponse)
        {
        }

        protected static IEnumerable<object> WalkTypes(System.Type type, XmlNodeList xmlNodeList)
        {
            var listType = typeof(List<>);
            var concreteType = listType.MakeGenericType(type);
            var instance = Activator.CreateInstance(concreteType, xmlNodeList.Count);
            for (int i = 0; i < xmlNodeList.Count; i++)
            {
                var value = WalkType(type, xmlNodeList.Item(i));
                concreteType.GetMethod("Add").Invoke(instance, new object[] { value });
            }

            return (IEnumerable<object>)instance;
        }

        protected static object WalkType(System.Type type, XmlNode xmlNode)
        {
            var propertyInfos = type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy);
            var instance = Activator.CreateInstance(type);

            foreach (var propertyInfo in propertyInfos)
            {
                var node = xmlNode.SelectSingleNode(propertyInfo.Name);
                if (node != null)
                {
                    if (propertyInfo.PropertyType.IsEnum)
                    {
                        var enumValue = Enum.Parse(propertyInfo.PropertyType, xmlNode.ReadNode(propertyInfo.Name));
                        propertyInfo.SetValue(instance, enumValue, null);
                    }
                    else if (propertyInfo.PropertyType.GetInterfaces().Contains(typeof(IStringConvertible)))
                    {
                        var str = xmlNode.ReadNode(propertyInfo.Name);
                        var baseInstance = Activator.CreateInstance(propertyInfo.PropertyType, str);
                        propertyInfo.SetValue(instance, baseInstance, null);
                    }
                    else if (typeof(IEnumerable<object>).IsAssignableFrom(propertyInfo.PropertyType))
                    {
                        var list = WalkTypes(propertyInfo.PropertyType.GetGenericArguments()[0], xmlNode.SelectNodes(propertyInfo.Name));
                        propertyInfo.SetValue(instance, list, null);
                    }
                    else
                    {
                        Console.WriteLine("TEST");
                    }
                }
            }

            return instance;
        }
    }
}
