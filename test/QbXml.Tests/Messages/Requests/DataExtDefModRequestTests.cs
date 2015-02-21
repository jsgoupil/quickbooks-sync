using NUnit.Framework;
using QbSync.QbXml.Extensions;
using QbSync.QbXml.Messages.Requests;
using QbSync.QbXml.Objects;
using QbSync.QbXml.Tests.Helpers;
using System;
using System.Collections.Generic;
using System.Xml;

namespace QbSync.QbXml.Tests.QbXml
{
    [TestFixture]
    class DataExtDefModRequestTests
    {
        [Test]
        public void BasicDataExtDefModRequestTest()
        {
            var request = new QbXmlRequest();
            var innerRequest = new DataExtDefModRequest();
            innerRequest.OwnerID = Guid.NewGuid();
            innerRequest.DataExtName = "name";
            innerRequest.DataExtNewName = "newname";
            innerRequest.AssignToObject = new List<AssignToObject>
            {
                AssignToObject.Account,
                AssignToObject.Charge
            };
            innerRequest.RemoveFromObject = new List<RemoveFromObject>
            {
                RemoveFromObject.Bill,
                RemoveFromObject.Check
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

            Assert.AreEqual(1, requestXmlDoc.GetElementsByTagName("DataExtDefModRq").Count);

            var node = requestXmlDoc.SelectSingleNode("//DataExtDefModRq/DataExtDefMod");
            Assert.AreEqual(innerRequest.OwnerID.ToString(), node.ReadNode("OwnerID"));
            Assert.AreEqual(innerRequest.DataExtName, node.ReadNode("DataExtName"));
            Assert.AreEqual(innerRequest.DataExtNewName, node.ReadNode("DataExtNewName"));

            var node2 = node.SelectNodes("AssignToObject");
            Assert.AreEqual(2, node2.Count);
            Assert.AreEqual("Account", node2.Item(0).InnerText);
            Assert.AreEqual("Charge", node2.Item(1).InnerText);

            var node3 = node.SelectNodes("RemoveFromObject");
            Assert.AreEqual(2, node3.Count);
            Assert.AreEqual("Bill", node3.Item(0).InnerText);
            Assert.AreEqual("Check", node3.Item(1).InnerText);

            var node4 = requestXmlDoc.SelectNodes("//IncludeRetElement");
            Assert.AreEqual(2, node4.Count);
            Assert.AreEqual("ABC", node4.Item(0).InnerText);
            Assert.AreEqual("DEF", node4.Item(1).InnerText);
            Assert.AreNotEqual("DataExtDefMod", node4.Item(0).ParentNode.Name);
            Assert.IsEmpty(QuickBooksTestHelper.GetXmlValidation(xml));
        }
    }
}