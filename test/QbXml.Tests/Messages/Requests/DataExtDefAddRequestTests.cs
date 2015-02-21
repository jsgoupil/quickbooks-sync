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
    class DataExtDefAddRequestTests
    {
        [Test]
        public void BasicDataExtDefAddRequestTest()
        {
            var request = new QbXmlRequest();
            var innerRequest = new DataExtDefAddRequest();
            innerRequest.OwnerID = Guid.NewGuid();
            innerRequest.DataExtName = "name";
            innerRequest.DataExtType = DataExtType.STR255TYPE;
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

            Assert.AreEqual(1, requestXmlDoc.GetElementsByTagName("DataExtDefAddRq").Count);

            var node = requestXmlDoc.SelectSingleNode("//DataExtDefAddRq/DataExtDefAdd");
            Assert.AreEqual(innerRequest.OwnerID.ToString(), node.ReadNode("OwnerID"));
            Assert.AreEqual(innerRequest.DataExtName, node.ReadNode("DataExtName"));
            Assert.AreEqual(innerRequest.DataExtType.ToString(), node.ReadNode("DataExtType"));

            var node2 = node.SelectNodes("AssignToObject");
            Assert.AreEqual(2, node2.Count);
            Assert.AreEqual("Account", node2.Item(0).InnerText);
            Assert.AreEqual("Charge", node2.Item(1).InnerText);

            var node3 = requestXmlDoc.SelectNodes("//IncludeRetElement");
            Assert.AreEqual(2, node3.Count);
            Assert.AreEqual("ABC", node3.Item(0).InnerText);
            Assert.AreEqual("DEF", node3.Item(1).InnerText);
            Assert.AreNotEqual("DataExtDefAdd", node3.Item(0).ParentNode.Name);
            Assert.IsEmpty(QuickBooksTestHelper.GetXmlValidation(xml));
        }
    }
}