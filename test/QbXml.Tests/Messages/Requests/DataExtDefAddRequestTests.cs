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
    class DataExtDefAddRequestTests
    {
        [Test]
        public void BasicDataExtDefAddRequestTest()
        {
            var dataExtDefAddRequest = new DataExtDefAddRequest();
            dataExtDefAddRequest.OwnerID = Guid.NewGuid();
            dataExtDefAddRequest.DataExtName = "name";
            dataExtDefAddRequest.DataExtType = DataExtType.STR255TYPE;
            dataExtDefAddRequest.AssignToObject = new List<AssignToObject>
            {
                AssignToObject.Account,
                AssignToObject.Charge
            };
            dataExtDefAddRequest.IncludeRetElement = new List<StrType>
            {
                new StrType("ABC"),
                new StrType("DEF")
            };

            var request = dataExtDefAddRequest.GetRequest();

            XmlDocument requestXmlDoc = new XmlDocument();
            requestXmlDoc.LoadXml(request);

            Assert.AreEqual(1, requestXmlDoc.GetElementsByTagName("DataExtDefAddRq").Count);

            var node = requestXmlDoc.SelectSingleNode("//DataExtDefAddRq/DataExtDefAdd");
            QBAssert.AreEqual(dataExtDefAddRequest.OwnerID, node.ReadNode("OwnerID"));
            QBAssert.AreEqual(dataExtDefAddRequest.DataExtName, node.ReadNode("DataExtName"));
            Assert.AreEqual(dataExtDefAddRequest.DataExtType.ToString(), node.ReadNode("DataExtType"));

            var node2 = node.SelectNodes("AssignToObject");
            Assert.AreEqual(2, node2.Count);
            Assert.AreEqual("Account", node2.Item(0).InnerText);
            Assert.AreEqual("Charge", node2.Item(1).InnerText);

            var node3 = requestXmlDoc.SelectNodes("//IncludeRetElement");
            Assert.AreEqual(2, node3.Count);
            Assert.AreEqual("ABC", node3.Item(0).InnerText);
            Assert.AreEqual("DEF", node3.Item(1).InnerText);
            Assert.AreNotEqual("DataExtDefAdd", node3.Item(0).ParentNode.Name);
        }
    }
}