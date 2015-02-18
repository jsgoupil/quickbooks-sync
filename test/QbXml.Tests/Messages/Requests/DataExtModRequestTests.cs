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
    class DataExtModRequestTests
    {
        [Test]
        public void BasicDataExtModRequestTest()
        {
            var dataExtModRequest = new DataExtModRequest();
            dataExtModRequest.OwnerID = Guid.NewGuid();
            dataExtModRequest.DataExtName = "name";
            dataExtModRequest.DataExtValue = "value";
            dataExtModRequest.OtherDataExtType = OtherDataExtType.Company;

            var request = dataExtModRequest.GetRequest();

            XmlDocument requestXmlDoc = new XmlDocument();
            requestXmlDoc.LoadXml(request);

            Assert.AreEqual(1, requestXmlDoc.GetElementsByTagName("DataExtModRq").Count);

            var node = requestXmlDoc.SelectSingleNode("//DataExtModRq/DataExtMod");
            QBAssert.AreEqual(dataExtModRequest.OwnerID, node.ReadNode("OwnerID"));
            QBAssert.AreEqual(dataExtModRequest.DataExtName, node.ReadNode("DataExtName"));
            QBAssert.AreEqual(dataExtModRequest.DataExtValue, node.ReadNode("DataExtValue"));
            Assert.AreEqual("Company", node.ReadNode("OtherDataExtType"));
            Assert.IsEmpty(QuickBooksTestHelper.GetXmlValidation(request));
        }

        [Test]
        public void ListDataExtModRequestTest()
        {
            var dataExtModRequest = new DataExtModRequest();
            dataExtModRequest.Filter = DataExtFilter.List;
            dataExtModRequest.OwnerID = Guid.NewGuid();
            dataExtModRequest.DataExtName = "name";
            dataExtModRequest.DataExtValue = "value";
            dataExtModRequest.ListDataExt = new Objects.ListDataExt
            {
                ListDataExtType = ListDataExtType.Customer,
                ListObjRef = new Objects.Ref
                {
                    FullName = "test",
                    ListID = "12345"
                }
            };

            var request = dataExtModRequest.GetRequest();

            XmlDocument requestXmlDoc = new XmlDocument();
            requestXmlDoc.LoadXml(request);

            Assert.AreEqual(1, requestXmlDoc.GetElementsByTagName("DataExtModRq").Count);

            var node = requestXmlDoc.SelectSingleNode("//DataExtModRq/DataExtMod");
            QBAssert.AreEqual(dataExtModRequest.OwnerID, node.ReadNode("OwnerID"));
            QBAssert.AreEqual(dataExtModRequest.DataExtName, node.ReadNode("DataExtName"));
            QBAssert.AreEqual(dataExtModRequest.DataExtValue, node.ReadNode("DataExtValue"));
            Assert.AreEqual("Customer", node.ReadNode("ListDataExtType"));

            var node2 = node.SelectSingleNode("ListObjRef");
            QBAssert.AreEqual(dataExtModRequest.ListDataExt.ListObjRef.FullName, node2.ReadNode("FullName"));
            QBAssert.AreEqual(dataExtModRequest.ListDataExt.ListObjRef.ListID, node2.ReadNode("ListID"));
            Assert.IsEmpty(QuickBooksTestHelper.GetXmlValidation(request));
        }

        [Test]
        public void TxnDataExtModRequestTest()
        {
            var dataExtModRequest = new DataExtModRequest();
            dataExtModRequest.Filter = DataExtFilter.Txn;
            dataExtModRequest.OwnerID = Guid.NewGuid();
            dataExtModRequest.DataExtName = "name";
            dataExtModRequest.DataExtValue = "value";
            dataExtModRequest.TxnDataExt = new Objects.TxnDataExt
            {
                TxnDataExtType = Struct.TxnDataExtType.Charge,
                TxnID = "123",
                TxnLineID = "345"
            };

            var request = dataExtModRequest.GetRequest();

            XmlDocument requestXmlDoc = new XmlDocument();
            requestXmlDoc.LoadXml(request);

            Assert.AreEqual(1, requestXmlDoc.GetElementsByTagName("DataExtModRq").Count);

            var node = requestXmlDoc.SelectSingleNode("//DataExtModRq/DataExtMod");
            QBAssert.AreEqual(dataExtModRequest.OwnerID, node.ReadNode("OwnerID"));
            QBAssert.AreEqual(dataExtModRequest.DataExtName, node.ReadNode("DataExtName"));
            QBAssert.AreEqual(dataExtModRequest.DataExtValue, node.ReadNode("DataExtValue"));
            Assert.AreEqual("Charge", node.ReadNode("TxnDataExtType"));
            QBAssert.AreEqual(dataExtModRequest.TxnDataExt.TxnID, node.ReadNode("TxnID"));
            QBAssert.AreEqual(dataExtModRequest.TxnDataExt.TxnLineID, node.ReadNode("TxnLineID"));
            Assert.IsEmpty(QuickBooksTestHelper.GetXmlValidation(request));
        }
    }
}