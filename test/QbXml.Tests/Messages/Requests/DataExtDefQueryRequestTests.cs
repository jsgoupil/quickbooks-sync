using NUnit.Framework;
using QbSync.QbXml.Messages.Requests;
using QbSync.QbXml.Objects;
using QbSync.QbXml.Tests.Helpers;
using QbSync.QbXml.Type;
using System;
using System.Collections.Generic;
using System.Xml;

namespace QbSync.QbXml.Tests.QbXml
{
    [TestFixture]
    class DataExtDefQueryRequestTests
    {
        [Test]
        public void OwnerIDDataExtDefQueryRequestTest()
        {
            var request = new QbXmlRequest();
            var innerRequest = new DataExtDefQueryRequest();
            innerRequest.OwnerID = new List<GuidType>
            {
                Guid.NewGuid(),
                Guid.NewGuid()
            };
            innerRequest.IncludeRetElement = new List<string>
            {
                "ABC",
                "DEF"
            };
            request.AddToSingle(innerRequest);
            var xml = request.GetRequest();

            XmlDocument requestXmlDoc = new XmlDocument();
            requestXmlDoc.LoadXml(xml);

            Assert.AreEqual(1, requestXmlDoc.GetElementsByTagName("DataExtDefQueryRq").Count);

            var node = requestXmlDoc.SelectSingleNode("//DataExtDefQueryRq");
            Assert.AreEqual(2, node.SelectNodes("OwnerID").Count);

            var node3 = requestXmlDoc.SelectNodes("//IncludeRetElement");
            Assert.AreEqual(2, node3.Count);
            Assert.AreEqual("ABC", node3.Item(0).InnerText);
            Assert.AreEqual("DEF", node3.Item(1).InnerText);
            Assert.IsEmpty(QuickBooksTestHelper.GetXmlValidation(xml));
        }

        [Test]
        public void AssignToObjectDataExtDefQueryRequestTest()
        {
            var request = new QbXmlRequest();
            var innerRequest = new DataExtDefQueryRequest();
            innerRequest.AssignToObject = new List<AssignToObject>
            {
                AssignToObject.Account,
                AssignToObject.Charge
            };
            innerRequest.IncludeRetElement = new List<string>
            {
                "ABC",
                "DEF"
            };
            request.AddToSingle(innerRequest);
            var xml = request.GetRequest();

            XmlDocument requestXmlDoc = new XmlDocument();
            requestXmlDoc.LoadXml(xml);

            Assert.AreEqual(1, requestXmlDoc.GetElementsByTagName("DataExtDefQueryRq").Count);

            var node = requestXmlDoc.SelectSingleNode("//DataExtDefQueryRq");
            var node2 = node.SelectNodes("AssignToObject");
            Assert.AreEqual(2, node2.Count);
            Assert.AreEqual("Account", node2.Item(0).InnerText);
            Assert.AreEqual("Charge", node2.Item(1).InnerText);

            var node3 = requestXmlDoc.SelectNodes("//IncludeRetElement");
            Assert.AreEqual(2, node3.Count);
            Assert.AreEqual("ABC", node3.Item(0).InnerText);
            Assert.AreEqual("DEF", node3.Item(1).InnerText);
            Assert.IsEmpty(QuickBooksTestHelper.GetXmlValidation(xml));
        }
    }
}