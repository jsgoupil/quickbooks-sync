using NUnit.Framework;
using QbSync.QbXml.Extensions;
using QbSync.QbXml.Objects;
using QbSync.QbXml.Tests.Helpers;
using System;
using System.Xml;

namespace QbSync.QbXml.Tests.QbXml
{
    [TestFixture]
    class DataExtModRequestTests
    {
        [Test]
        public void BasicDataExtModRequestTest()
        {
            var request = new QbXmlRequest();
            var innerRequest = new DataExtModRqType();
            innerRequest.DataExtMod = new DataExtMod
            {
                OwnerID = Guid.NewGuid(),
                DataExtName = "name",
                DataExtValue = "value",
                OtherDataExtType = OtherDataExtType.Company
            };
            request.AddToSingle(innerRequest);
            var xml = request.GetRequest();

            XmlDocument requestXmlDoc = new XmlDocument();
            requestXmlDoc.LoadXml(xml);

            Assert.AreEqual(1, requestXmlDoc.GetElementsByTagName("DataExtModRq").Count);

            var node = requestXmlDoc.SelectSingleNode("//DataExtModRq/DataExtMod");
            Assert.AreEqual(innerRequest.DataExtMod.OwnerID.ToString(), node.ReadNode("OwnerID"));
            Assert.AreEqual(innerRequest.DataExtMod.DataExtName, node.ReadNode("DataExtName"));
            Assert.AreEqual(innerRequest.DataExtMod.DataExtValue, node.ReadNode("DataExtValue"));
            Assert.AreEqual("Company", node.ReadNode("OtherDataExtType"));
            Assert.IsEmpty(QuickBooksTestHelper.GetXmlValidation(xml));
        }

        [Test]
        public void ListDataExtModRequestTest()
        {
            var request = new QbXmlRequest();
            var innerRequest = new DataExtModRqType();
            innerRequest.DataExtMod = new DataExtMod
            {
                OwnerID = Guid.NewGuid(),
                DataExtName = "name",
                DataExtValue = "value",
                ListDataExtType = ListDataExtType.Customer,
                ListObjRef = new ListObjRef
                {
                    FullName = "test",
                    ListID = "12345"
                }
            };
            request.AddToSingle(innerRequest);
            var xml = request.GetRequest();

            XmlDocument requestXmlDoc = new XmlDocument();
            requestXmlDoc.LoadXml(xml);

            Assert.AreEqual(1, requestXmlDoc.GetElementsByTagName("DataExtModRq").Count);

            var node = requestXmlDoc.SelectSingleNode("//DataExtModRq/DataExtMod");
            Assert.AreEqual(innerRequest.DataExtMod.OwnerID.ToString(), node.ReadNode("OwnerID"));
            Assert.AreEqual(innerRequest.DataExtMod.DataExtName, node.ReadNode("DataExtName"));
            Assert.AreEqual(innerRequest.DataExtMod.DataExtValue, node.ReadNode("DataExtValue"));
            Assert.AreEqual("Customer", node.ReadNode("ListDataExtType"));

            var node2 = node.SelectSingleNode("ListObjRef");
            Assert.AreEqual(innerRequest.DataExtMod.ListObjRef.FullName, node2.ReadNode("FullName"));
            Assert.AreEqual(innerRequest.DataExtMod.ListObjRef.ListID, node2.ReadNode("ListID"));
            Assert.IsEmpty(QuickBooksTestHelper.GetXmlValidation(xml));
        }

        [Test]
        public void TxnDataExtModRequestTest()
        {
            var request = new QbXmlRequest();
            var innerRequest = new DataExtModRqType();
            innerRequest.DataExtMod = new DataExtMod
            {
                OwnerID = Guid.NewGuid(),
                DataExtName = "name",
                DataExtValue = "value",
                TxnDataExtType = TxnDataExtType.Charge,
                TxnID = "123",
                TxnLineID = "345"
            };
            request.AddToSingle(innerRequest);
            var xml = request.GetRequest();

            XmlDocument requestXmlDoc = new XmlDocument();
            requestXmlDoc.LoadXml(xml);

            Assert.AreEqual(1, requestXmlDoc.GetElementsByTagName("DataExtModRq").Count);

            var node = requestXmlDoc.SelectSingleNode("//DataExtModRq/DataExtMod");
            Assert.AreEqual(innerRequest.DataExtMod.OwnerID.ToString(), node.ReadNode("OwnerID"));
            Assert.AreEqual(innerRequest.DataExtMod.DataExtName, node.ReadNode("DataExtName"));
            Assert.AreEqual(innerRequest.DataExtMod.DataExtValue, node.ReadNode("DataExtValue"));
            Assert.AreEqual("Charge", node.ReadNode("TxnDataExtType"));
            Assert.AreEqual(innerRequest.DataExtMod.TxnID, node.ReadNode("TxnID"));
            Assert.AreEqual(innerRequest.DataExtMod.TxnLineID, node.ReadNode("TxnLineID"));
            Assert.IsEmpty(QuickBooksTestHelper.GetXmlValidation(xml));
        }
    }
}