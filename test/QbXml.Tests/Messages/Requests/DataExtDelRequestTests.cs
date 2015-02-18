using NUnit.Framework;
using QbSync.QbXml.Extensions;
using QbSync.QbXml.Messages.Requests;
using QbSync.QbXml.Struct;
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
            var dataExtDelRequest = new DataExtDelRequest();
            dataExtDelRequest.OwnerID = Guid.NewGuid();
            dataExtDelRequest.DataExtName = "name";
            dataExtDelRequest.OtherDataExtType = OtherDataExtType.Company;

            var request = dataExtDelRequest.GetRequest();

            XmlDocument requestXmlDoc = new XmlDocument();
            requestXmlDoc.LoadXml(request);

            Assert.AreEqual(1, requestXmlDoc.GetElementsByTagName("DataExtDelRq").Count);

            var node = requestXmlDoc.SelectSingleNode("//DataExtDelRq/DataExtDel");
            QBAssert.AreEqual(dataExtDelRequest.OwnerID, node.ReadNode("OwnerID"));
            QBAssert.AreEqual(dataExtDelRequest.DataExtName, node.ReadNode("DataExtName"));
            Assert.AreEqual("Company", node.ReadNode("OtherDataExtType"));
            Assert.IsEmpty(QuickBooksTestHelper.GetXmlValidation(request));
        }

        [Test]
        public void ListDataExtDelRequestTest()
        {
            var dataExtDelRequest = new DataExtDelRequest();
            dataExtDelRequest.Filter = DataExtFilter.List;
            dataExtDelRequest.OwnerID = Guid.NewGuid();
            dataExtDelRequest.DataExtName = "name";
            dataExtDelRequest.ListDataExt = new Objects.ListDataExt
            {
                ListDataExtType = ListDataExtType.Customer,
                ListObjRef = new Objects.Ref
                {
                    FullName = "test",
                    ListID = "12345"
                }
            };

            var request = dataExtDelRequest.GetRequest();

            XmlDocument requestXmlDoc = new XmlDocument();
            requestXmlDoc.LoadXml(request);

            Assert.AreEqual(1, requestXmlDoc.GetElementsByTagName("DataExtDelRq").Count);

            var node = requestXmlDoc.SelectSingleNode("//DataExtDelRq/DataExtDel");
            QBAssert.AreEqual(dataExtDelRequest.OwnerID, node.ReadNode("OwnerID"));
            QBAssert.AreEqual(dataExtDelRequest.DataExtName, node.ReadNode("DataExtName"));
            Assert.AreEqual("Customer", node.ReadNode("ListDataExtType"));

            var node2 = node.SelectSingleNode("ListObjRef");
            QBAssert.AreEqual(dataExtDelRequest.ListDataExt.ListObjRef.FullName, node2.ReadNode("FullName"));
            QBAssert.AreEqual(dataExtDelRequest.ListDataExt.ListObjRef.ListID, node2.ReadNode("ListID"));
            Assert.IsEmpty(QuickBooksTestHelper.GetXmlValidation(request));
        }

        [Test]
        public void TxnDataExtDelRequestTest()
        {
            var dataExtDelRequest = new DataExtDelRequest();
            dataExtDelRequest.Filter = DataExtFilter.Txn;
            dataExtDelRequest.OwnerID = Guid.NewGuid();
            dataExtDelRequest.DataExtName = "name";
            dataExtDelRequest.TxnDataExt = new Objects.TxnDataExt
            {
                TxnDataExtType = Struct.TxnDataExtType.Charge,
                TxnID = "123",
                TxnLineID = "345"
            };

            var request = dataExtDelRequest.GetRequest();

            XmlDocument requestXmlDoc = new XmlDocument();
            requestXmlDoc.LoadXml(request);

            Assert.AreEqual(1, requestXmlDoc.GetElementsByTagName("DataExtDelRq").Count);

            var node = requestXmlDoc.SelectSingleNode("//DataExtDelRq/DataExtDel");
            QBAssert.AreEqual(dataExtDelRequest.OwnerID, node.ReadNode("OwnerID"));
            QBAssert.AreEqual(dataExtDelRequest.DataExtName, node.ReadNode("DataExtName"));
            Assert.AreEqual("Charge", node.ReadNode("TxnDataExtType"));
            QBAssert.AreEqual(dataExtDelRequest.TxnDataExt.TxnID, node.ReadNode("TxnID"));
            QBAssert.AreEqual(dataExtDelRequest.TxnDataExt.TxnLineID, node.ReadNode("TxnLineID"));
            Assert.IsEmpty(QuickBooksTestHelper.GetXmlValidation(request));
        }
    }
}