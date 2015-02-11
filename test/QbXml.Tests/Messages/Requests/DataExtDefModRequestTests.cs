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
            dataExtDefModRequest.DataExtType = DataExtType.STR255TYPE;
            dataExtDefModRequest.AssignToObject = new List<AssignToObject>
            {
                AssignToObject.Account,
                AssignToObject.Charge
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
            Assert.AreEqual(dataExtDefModRequest.DataExtType.ToString(), node.ReadNode("DataExtType"));

            var node2 = node.SelectNodes("AssignToObject");
            Assert.AreEqual(2, node2.Count);
            Assert.AreEqual("Account", node2.Item(0).InnerText);
            Assert.AreEqual("Charge", node2.Item(1).InnerText);

            var node3 = requestXmlDoc.SelectNodes("//IncludeRetElement");
            Assert.AreEqual(2, node3.Count);
            Assert.AreEqual("ABC", node3.Item(0).InnerText);
            Assert.AreEqual("DEF", node3.Item(1).InnerText);
            Assert.AreNotEqual("DataExtDefMod", node3.Item(0).ParentNode.Name);
        }
    }
}