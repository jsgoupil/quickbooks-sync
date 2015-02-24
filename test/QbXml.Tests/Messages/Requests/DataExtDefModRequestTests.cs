using NUnit.Framework;
using QbSync.QbXml.Extensions;
using QbSync.QbXml.Objects;
using QbSync.QbXml.Tests.Helpers;
using System;
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
            var innerRequest = new DataExtDefModRqType();
            innerRequest.DataExtDefMod = new DataExtDefMod
            {
                OwnerID = Guid.NewGuid(),
                DataExtName = "name",
                DataExtNewName = "newname",
                AssignToObject = new AssignToObject[]
                {
                    AssignToObject.Account,
                    AssignToObject.Charge
                },
                RemoveFromObject = new RemoveFromObject[] 
                {
                    RemoveFromObject.Bill,
                    RemoveFromObject.Check
                }
            };

            innerRequest.IncludeRetElement = new string[]
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
            Assert.AreEqual(innerRequest.DataExtDefMod.OwnerID.ToString(), node.ReadNode("OwnerID"));
            Assert.AreEqual(innerRequest.DataExtDefMod.DataExtName, node.ReadNode("DataExtName"));
            Assert.AreEqual(innerRequest.DataExtDefMod.DataExtNewName, node.ReadNode("DataExtNewName"));

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