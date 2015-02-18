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
    class DataExtAddRequestTests
    {
        [Test]
        public void BasicDataExtAddRequestTest()
        {
            var dataExtAddRequest = new DataExtAddRequest();
            dataExtAddRequest.OwnerID = Guid.NewGuid();
            dataExtAddRequest.DataExtName = "name";
            dataExtAddRequest.DataExtValue = "value";
            dataExtAddRequest.OtherDataExtType = OtherDataExtType.Company;

            var request = dataExtAddRequest.GetRequest();

            XmlDocument requestXmlDoc = new XmlDocument();
            requestXmlDoc.LoadXml(request);

            Assert.AreEqual(1, requestXmlDoc.GetElementsByTagName("DataExtAddRq").Count);

            var node = requestXmlDoc.SelectSingleNode("//DataExtAddRq/DataExtAdd");
            QBAssert.AreEqual(dataExtAddRequest.OwnerID, node.ReadNode("OwnerID"));
            QBAssert.AreEqual(dataExtAddRequest.DataExtName, node.ReadNode("DataExtName"));
            QBAssert.AreEqual(dataExtAddRequest.DataExtValue, node.ReadNode("DataExtValue"));
            Assert.AreEqual("Company", node.ReadNode("OtherDataExtType"));
            Assert.IsEmpty(QuickBooksTestHelper.GetXmlValidation(request));
        }

        [Test]
        public void ListDataExtAddRequestTest()
        {
            var dataExtAddRequest = new DataExtAddRequest();
            dataExtAddRequest.Filter = DataExtFilter.List;
            dataExtAddRequest.OwnerID = Guid.NewGuid();
            dataExtAddRequest.DataExtName = "name";
            dataExtAddRequest.DataExtValue = "value";
            dataExtAddRequest.ListDataExt = new Objects.ListDataExt
            {
                ListDataExtType = ListDataExtType.Customer,
                ListObjRef = new Objects.Ref
                {
                    FullName = "test",
                    ListID = "12345"
                }
            };

            var request = dataExtAddRequest.GetRequest();

            XmlDocument requestXmlDoc = new XmlDocument();
            requestXmlDoc.LoadXml(request);

            Assert.AreEqual(1, requestXmlDoc.GetElementsByTagName("DataExtAddRq").Count);

            var node = requestXmlDoc.SelectSingleNode("//DataExtAddRq/DataExtAdd");
            QBAssert.AreEqual(dataExtAddRequest.OwnerID, node.ReadNode("OwnerID"));
            QBAssert.AreEqual(dataExtAddRequest.DataExtName, node.ReadNode("DataExtName"));
            QBAssert.AreEqual(dataExtAddRequest.DataExtValue, node.ReadNode("DataExtValue"));
            Assert.AreEqual("Customer", node.ReadNode("ListDataExtType"));

            var node2 = node.SelectSingleNode("ListObjRef");
            QBAssert.AreEqual(dataExtAddRequest.ListDataExt.ListObjRef.FullName, node2.ReadNode("FullName"));
            QBAssert.AreEqual(dataExtAddRequest.ListDataExt.ListObjRef.ListID, node2.ReadNode("ListID"));
            Assert.IsEmpty(QuickBooksTestHelper.GetXmlValidation(request));
        }

        [Test]
        public void TxnDataExtAddRequestTest()
        {
            var dataExtAddRequest = new DataExtAddRequest();
            dataExtAddRequest.Filter = DataExtFilter.Txn;
            dataExtAddRequest.OwnerID = Guid.NewGuid();
            dataExtAddRequest.DataExtName = "name";
            dataExtAddRequest.DataExtValue = "value";
            dataExtAddRequest.TxnDataExt = new Objects.TxnDataExt
            {
                TxnDataExtType = Struct.TxnDataExtType.Charge,
                TxnID = "123",
                TxnLineID = "345"
            };

            var request = dataExtAddRequest.GetRequest();

            XmlDocument requestXmlDoc = new XmlDocument();
            requestXmlDoc.LoadXml(request);

            Assert.AreEqual(1, requestXmlDoc.GetElementsByTagName("DataExtAddRq").Count);

            var node = requestXmlDoc.SelectSingleNode("//DataExtAddRq/DataExtAdd");
            QBAssert.AreEqual(dataExtAddRequest.OwnerID, node.ReadNode("OwnerID"));
            QBAssert.AreEqual(dataExtAddRequest.DataExtName, node.ReadNode("DataExtName"));
            QBAssert.AreEqual(dataExtAddRequest.DataExtValue, node.ReadNode("DataExtValue"));
            Assert.AreEqual("Charge", node.ReadNode("TxnDataExtType"));
            QBAssert.AreEqual(dataExtAddRequest.TxnDataExt.TxnID, node.ReadNode("TxnID"));
            QBAssert.AreEqual(dataExtAddRequest.TxnDataExt.TxnLineID, node.ReadNode("TxnLineID"));
            Assert.IsEmpty(QuickBooksTestHelper.GetXmlValidation(request));
        }
    }
}