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
    class DataExtDefModRequestTests
    {
        [Test]
        public void BasicDataExtDefModRequestTest()
        {
            var dataExtDefModRequest = new DataExtDefModRequest();
            dataExtDefModRequest.OwnerID = Guid.NewGuid();
            dataExtDefModRequest.DataExtName = "name";
            dataExtDefModRequest.DataExtNewName = "newname";
            dataExtDefModRequest.AssignToObject = new List<AssignToObject>
            {
                AssignToObject.Account,
                AssignToObject.Charge
            };
            dataExtDefModRequest.RemoveFromObject = new List<AssignToObject>
            {
                AssignToObject.Bill,
                AssignToObject.Check
            };
            dataExtDefModRequest.IncludeRetElement = new List<StrType>
            {
                new StrType("ABC"),
                new StrType("DEF")
            };

            var request = dataExtDefModRequest.GetRequest();

            XmlDocument requestXmlDoc = new XmlDocument();
            requestXmlDoc.LoadXml(request);

            Assert.AreEqual(1, requestXmlDoc.GetElementsByTagName("DataExtDefModRq").Count);

            var node = requestXmlDoc.SelectSingleNode("//DataExtDefModRq/DataExtDefMod");
            QBAssert.AreEqual(dataExtDefModRequest.OwnerID, node.ReadNode("OwnerID"));
            QBAssert.AreEqual(dataExtDefModRequest.DataExtName, node.ReadNode("DataExtName"));
            QBAssert.AreEqual(dataExtDefModRequest.DataExtNewName, node.ReadNode("DataExtNewName"));

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
            Assert.IsEmpty(QuickBooksTestHelper.GetXmlValidation(request));
        }
    }
}