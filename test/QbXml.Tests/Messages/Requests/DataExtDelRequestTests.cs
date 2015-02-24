using NUnit.Framework;
using QbSync.QbXml.Extensions;
using QbSync.QbXml.Objects;
using QbSync.QbXml.Tests.Helpers;
using System;
using System.Xml;

namespace QbSync.QbXml.Tests.QbXml
{
    [TestFixture]
    class DataExtDelRequestTests
    {
        [Test]
        public void BasicDataExtDelRequestTest()
        {
            var request = new QbXmlRequest();
            var innerRequest = new DataExtDelRqType();
            innerRequest.DataExtDel = new DataExtDel
            {
                OwnerID = Guid.NewGuid(),
                DataExtName = "name",
                OtherDataExtType = OtherDataExtType.Company
            };
            request.AddToSingle(innerRequest);
            var xml = request.GetRequest();

            XmlDocument requestXmlDoc = new XmlDocument();
            requestXmlDoc.LoadXml(xml);

            Assert.AreEqual(1, requestXmlDoc.GetElementsByTagName("DataExtDelRq").Count);

            var node = requestXmlDoc.SelectSingleNode("//DataExtDelRq/DataExtDel");
            Assert.AreEqual(innerRequest.DataExtDel.OwnerID.ToString(), node.ReadNode("OwnerID"));
            Assert.AreEqual(innerRequest.DataExtDel.DataExtName, node.ReadNode("DataExtName"));
            Assert.AreEqual("Company", node.ReadNode("OtherDataExtType"));
            Assert.IsEmpty(QuickBooksTestHelper.GetXmlValidation(xml));
        }

        [Test]
        public void ListDataExtDelRequestTest()
        {
            var request = new QbXmlRequest();
            var innerRequest = new DataExtDelRqType();
            innerRequest.DataExtDel = new DataExtDel
            {
                OwnerID = Guid.NewGuid(),
                DataExtName = "name",
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

            Assert.AreEqual(1, requestXmlDoc.GetElementsByTagName("DataExtDelRq").Count);

            var node = requestXmlDoc.SelectSingleNode("//DataExtDelRq/DataExtDel");
            Assert.AreEqual(innerRequest.DataExtDel.OwnerID.ToString(), node.ReadNode("OwnerID"));
            Assert.AreEqual(innerRequest.DataExtDel.DataExtName, node.ReadNode("DataExtName"));
            Assert.AreEqual("Customer", node.ReadNode("ListDataExtType"));

            var node2 = node.SelectSingleNode("ListObjRef");
            Assert.AreEqual(innerRequest.DataExtDel.ListObjRef.FullName, node2.ReadNode("FullName"));
            Assert.AreEqual(innerRequest.DataExtDel.ListObjRef.ListID, node2.ReadNode("ListID"));
            Assert.IsEmpty(QuickBooksTestHelper.GetXmlValidation(xml));
        }

        [Test]
        public void TxnDataExtDelRequestTest()
        {
            var request = new QbXmlRequest();
            var innerRequest = new DataExtDelRqType();
            innerRequest.DataExtDel = new DataExtDel
            {
                OwnerID = Guid.NewGuid(),
                DataExtName = "name",
                TxnDataExtType = TxnDataExtType.Charge,
                TxnID = "123",
                TxnLineID = "345"
            };
            request.AddToSingle(innerRequest);
            var xml = request.GetRequest();

            XmlDocument requestXmlDoc = new XmlDocument();
            requestXmlDoc.LoadXml(xml);

            Assert.AreEqual(1, requestXmlDoc.GetElementsByTagName("DataExtDelRq").Count);

            var node = requestXmlDoc.SelectSingleNode("//DataExtDelRq/DataExtDel");
            Assert.AreEqual(innerRequest.DataExtDel.OwnerID.ToString(), node.ReadNode("OwnerID"));
            Assert.AreEqual(innerRequest.DataExtDel.DataExtName, node.ReadNode("DataExtName"));
            Assert.AreEqual("Charge", node.ReadNode("TxnDataExtType"));
            Assert.AreEqual(innerRequest.DataExtDel.TxnID, node.ReadNode("TxnID"));
            Assert.AreEqual(innerRequest.DataExtDel.TxnLineID, node.ReadNode("TxnLineID"));
            Assert.IsEmpty(QuickBooksTestHelper.GetXmlValidation(xml));
        }
    }
}