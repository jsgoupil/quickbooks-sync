using NUnit.Framework;
using QbSync.QbXml.Extensions;
using QbSync.QbXml.Messages.Requests;
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
            var innerRequest = new DataExtModRequest();
            innerRequest.OwnerID = Guid.NewGuid();
            innerRequest.DataExtName = "name";
            innerRequest.DataExtValue = "value";
            innerRequest.OtherDataExtType = OtherDataExtType.Company;
            request.AddToSingle(innerRequest);
            var xml = request.GetRequest();

            XmlDocument requestXmlDoc = new XmlDocument();
            requestXmlDoc.LoadXml(xml);

            Assert.AreEqual(1, requestXmlDoc.GetElementsByTagName("DataExtModRq").Count);

            var node = requestXmlDoc.SelectSingleNode("//DataExtModRq/DataExtMod");
            Assert.AreEqual(innerRequest.OwnerID.ToString(), node.ReadNode("OwnerID"));
            Assert.AreEqual(innerRequest.DataExtName, node.ReadNode("DataExtName"));
            Assert.AreEqual(innerRequest.DataExtValue, node.ReadNode("DataExtValue"));
            Assert.AreEqual("Company", node.ReadNode("OtherDataExtType"));
            Assert.IsEmpty(QuickBooksTestHelper.GetXmlValidation(xml));
        }

        [Test]
        public void ListDataExtModRequestTest()
        {
            var request = new QbXmlRequest();
            var innerRequest = new DataExtModRequest();
            innerRequest.OwnerID = Guid.NewGuid();
            innerRequest.DataExtName = "name";
            innerRequest.DataExtValue = "value";
            innerRequest.ListDataExtType = ListDataExtType.Customer;
            innerRequest.ListObjRef = new ListObjRef
            {
                FullName = "test",
                ListID = "12345"
            };
            request.AddToSingle(innerRequest);
            var xml = request.GetRequest();

            XmlDocument requestXmlDoc = new XmlDocument();
            requestXmlDoc.LoadXml(xml);

            Assert.AreEqual(1, requestXmlDoc.GetElementsByTagName("DataExtModRq").Count);

            var node = requestXmlDoc.SelectSingleNode("//DataExtModRq/DataExtMod");
            Assert.AreEqual(innerRequest.OwnerID.ToString(), node.ReadNode("OwnerID"));
            Assert.AreEqual(innerRequest.DataExtName, node.ReadNode("DataExtName"));
            Assert.AreEqual(innerRequest.DataExtValue, node.ReadNode("DataExtValue"));
            Assert.AreEqual("Customer", node.ReadNode("ListDataExtType"));

            var node2 = node.SelectSingleNode("ListObjRef");
            Assert.AreEqual(innerRequest.ListObjRef.FullName, node2.ReadNode("FullName"));
            Assert.AreEqual(innerRequest.ListObjRef.ListID, node2.ReadNode("ListID"));
            Assert.IsEmpty(QuickBooksTestHelper.GetXmlValidation(xml));
        }

        [Test]
        public void TxnDataExtModRequestTest()
        {
            var request = new QbXmlRequest();
            var innerRequest = new DataExtModRequest();
            innerRequest.OwnerID = Guid.NewGuid();
            innerRequest.DataExtName = "name";
            innerRequest.DataExtValue = "value";
            innerRequest.TxnDataExtType = TxnDataExtType.Charge;
            innerRequest.TxnID = "123";
            innerRequest.TxnLineID = "345";
            request.AddToSingle(innerRequest);
            var xml = request.GetRequest();

            XmlDocument requestXmlDoc = new XmlDocument();
            requestXmlDoc.LoadXml(xml);

            Assert.AreEqual(1, requestXmlDoc.GetElementsByTagName("DataExtModRq").Count);

            var node = requestXmlDoc.SelectSingleNode("//DataExtModRq/DataExtMod");
            Assert.AreEqual(innerRequest.OwnerID.ToString(), node.ReadNode("OwnerID"));
            Assert.AreEqual(innerRequest.DataExtName, node.ReadNode("DataExtName"));
            Assert.AreEqual(innerRequest.DataExtValue, node.ReadNode("DataExtValue"));
            Assert.AreEqual("Charge", node.ReadNode("TxnDataExtType"));
            Assert.AreEqual(innerRequest.TxnID, node.ReadNode("TxnID"));
            Assert.AreEqual(innerRequest.TxnLineID, node.ReadNode("TxnLineID"));
            Assert.IsEmpty(QuickBooksTestHelper.GetXmlValidation(xml));
        }
    }
}