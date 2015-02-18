using NUnit.Framework;
using QbSync.QbXml.Extensions;
using QbSync.QbXml.Messages.Requests;
using QbSync.QbXml.Struct;
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
            var dataExtDefQueryRequest = new DataExtDefQueryRequest();
            dataExtDefQueryRequest.OwnerID = new List<GuidType>
            {
                Guid.NewGuid(),
                Guid.NewGuid()
            };
            dataExtDefQueryRequest.IncludeRetElement = new List<StrType>
            {
                new StrType("ABC"),
                new StrType("DEF")
            };

            var request = dataExtDefQueryRequest.GetRequest();

            XmlDocument requestXmlDoc = new XmlDocument();
            requestXmlDoc.LoadXml(request);

            Assert.AreEqual(1, requestXmlDoc.GetElementsByTagName("DataExtDefQueryRq").Count);

            var node = requestXmlDoc.SelectSingleNode("//DataExtDefQueryRq");
            Assert.AreEqual(2, node.SelectNodes("OwnerID").Count);

            var node3 = requestXmlDoc.SelectNodes("//IncludeRetElement");
            Assert.AreEqual(2, node3.Count);
            Assert.AreEqual("ABC", node3.Item(0).InnerText);
            Assert.AreEqual("DEF", node3.Item(1).InnerText);
            Assert.IsEmpty(QuickBooksTestHelper.GetXmlValidation(request));
        }

        [Test]
        public void AssignToObjectDataExtDefQueryRequestTest()
        {
            var dataExtDefQueryRequest = new DataExtDefQueryRequest();
            dataExtDefQueryRequest.AssignToObject = new List<AssignToObject>
            {
                AssignToObject.Account,
                AssignToObject.Charge
            };
            dataExtDefQueryRequest.IncludeRetElement = new List<StrType>
            {
                new StrType("ABC"),
                new StrType("DEF")
            };

            var request = dataExtDefQueryRequest.GetRequest();

            XmlDocument requestXmlDoc = new XmlDocument();
            requestXmlDoc.LoadXml(request);

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
            Assert.IsEmpty(QuickBooksTestHelper.GetXmlValidation(request));
        }

        [Test]
        [ExpectedException(ExpectedException = typeof(ArgumentException))]
        public void ExceptionDataExtDefQueryRequestTest()
        {
            var dataExtDefQueryRequest = new DataExtDefQueryRequest();
            dataExtDefQueryRequest.OwnerID = new List<GuidType>
            {
                Guid.NewGuid(),
                Guid.NewGuid()
            };
            dataExtDefQueryRequest.AssignToObject = new List<AssignToObject>
            {
                AssignToObject.Account,
                AssignToObject.Charge
            };

            dataExtDefQueryRequest.GetRequest();
        }
    }
}