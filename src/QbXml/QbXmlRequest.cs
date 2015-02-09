using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace QbSync.QbXml
{
    public abstract class QbXmlRequest
    {
        protected string rootElementName;

        public QbXmlRequest(string rootElementName)
        {
            this.rootElementName = rootElementName;
        }

        public string GetRequest()
        {
            // Create the XML document to hold our request
            XmlDocument requestXmlDoc = new XmlDocument();

            // Add the prolog processing instructions
            requestXmlDoc.AppendChild(requestXmlDoc.CreateXmlDeclaration("1.0", null, null));
            requestXmlDoc.AppendChild(requestXmlDoc.CreateProcessingInstruction("qbxml", "version=\"13.0\""));

            // Create the outer request envelope tag
            XmlElement outer = requestXmlDoc.CreateElement("QBXML");
            requestXmlDoc.AppendChild(outer);

            // Create the inner request envelope & any needed attributes
            XmlElement inner = requestXmlDoc.CreateElement("QBXMLMsgsRq");
            outer.AppendChild(inner);
            inner.SetAttribute("onError", "stopOnError");

            // Create CustomerQueryRq aggregate and fill in field values for it
            XmlElement req = requestXmlDoc.CreateElement(rootElementName);
            inner.AppendChild(req);

            BuildRequest(requestXmlDoc, req);

            return requestXmlDoc.OuterXml;
        }

        protected virtual void BuildRequest(XmlDocument doc, XmlElement parent)
        {
        }
    }
}
