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
    class DataExtAddRequestTests
    {
        [Test]
        public void BasicDataExtAddRequestTest()
        {
            var request = new QbXmlRequest();
            var innerRequest = new DataExtAddRequest();
            innerRequest.OwnerID = Guid.NewGuid();
            innerRequest.DataExtName = "name";
            innerRequest.DataExtValue = "value";
            innerRequest.OtherDataExtType = OtherDataExtType.Company;
            request.AddToSingle(innerRequest);
            var xml = request.GetRequest();

            XmlDocument requestXmlDoc = new XmlDocument();
            requestXmlDoc.LoadXml(xml);

            Assert.AreEqual(1, requestXmlDoc.GetElementsByTagName("DataExtAddRq").Count);

            var node = requestXmlDoc.SelectSingleNode("//DataExtAddRq/DataExtAdd");
            Assert.AreEqual(innerRequest.OwnerID.ToString(), node.ReadNode("OwnerID"));
            Assert.AreEqual(innerRequest.DataExtName, node.ReadNode("DataExtName"));
            Assert.AreEqual(innerRequest.DataExtValue, node.ReadNode("DataExtValue"));
            Assert.AreEqual("Company", node.ReadNode("OtherDataExtType"));
            Assert.IsEmpty(QuickBooksTestHelper.GetXmlValidation(xml));
        }

        [Test]
        public void ListDataExtAddRequestTest()
        {
            var request = new QbXmlRequest();
            var innerRequest = new DataExtAddRequest();
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

            Assert.AreEqual(1, requestXmlDoc.GetElementsByTagName("DataExtAddRq").Count);

            var node = requestXmlDoc.SelectSingleNode("//DataExtAddRq/DataExtAdd");
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
        public void TxnDataExtAddRequestTest()
        {
            var request = new QbXmlRequest();
            var innerRequest = new DataExtAddRequest();
            innerRequest.OwnerID = Guid.NewGuid();
            innerRequest.DataExtName = "name";
            innerRequest.DataExtValue = "value";
            innerRequest.TxnDataExtType = TxnDataExtType.Charge;
            innerRequest.TxnID = new DataExtAddTxnID
            {
                Value = "123"
            };
            innerRequest.TxnLineID = "345";
            request.AddToSingle(innerRequest);
            var xml = request.GetRequest();

            XmlDocument requestXmlDoc = new XmlDocument();
            requestXmlDoc.LoadXml(xml);

            Assert.AreEqual(1, requestXmlDoc.GetElementsByTagName("DataExtAddRq").Count);

            var node = requestXmlDoc.SelectSingleNode("//DataExtAddRq/DataExtAdd");
            Assert.AreEqual(innerRequest.OwnerID.ToString(), node.ReadNode("OwnerID"));
            Assert.AreEqual(innerRequest.DataExtName, node.ReadNode("DataExtName"));
            Assert.AreEqual(innerRequest.DataExtValue, node.ReadNode("DataExtValue"));
            Assert.AreEqual("Charge", node.ReadNode("TxnDataExtType"));
            Assert.AreEqual(innerRequest.TxnID.Value, node.ReadNode("TxnID"));
            Assert.AreEqual(innerRequest.TxnLineID, node.ReadNode("TxnLineID"));
            Assert.IsEmpty(QuickBooksTestHelper.GetXmlValidation(xml));
        }
    }
}