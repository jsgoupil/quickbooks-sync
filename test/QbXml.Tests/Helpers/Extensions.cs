using System.Xml;

namespace QbSync.QbXml.Tests.Helpers
{
    internal static class Extensions
    {
        /// <summary>
        /// Returns null if we can't find the node.
        /// </summary>
        /// <param name="node"></param>
        /// <param name="xpath"></param>
        /// <returns></returns>
        internal static string ReadNode(this XmlNode node, string xpath)
        {
            var selectedNode = node.SelectSingleNode(xpath);

            if (selectedNode != null)
            {
                return selectedNode.InnerText;
            }

            return null;
        }
    }
}
