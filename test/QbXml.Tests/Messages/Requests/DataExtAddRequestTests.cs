using NUnit.Framework;
using QbSync.QbXml.Extensions;
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
            var innerRequest = new DataExtAddRqType();
            innerRequest.DataExtAdd = new DataExtAdd
            {
                OwnerID = new GUIDTYPE(Guid.NewGuid()),
                DataExtName = "name",
                DataExtValue = "value",
                OtherDataExtType = OtherDataExtType.Company
            };
            request.AddToSingle(innerRequest);
            var xml = request.GetRequest();

            XmlDocument requestXmlDoc = new XmlDocument();
            requestXmlDoc.LoadXml(xml);

            Assert.AreEqual(1, requestXmlDoc.GetElementsByTagName("DataExtAddRq").Count);

            var node = requestXmlDoc.SelectSingleNode("//DataExtAddRq/DataExtAdd");
            Assert.AreEqual(innerRequest.DataExtAdd.OwnerID.ToString(), node.ReadNode("OwnerID"));
            Assert.AreEqual(innerRequest.DataExtAdd.DataExtName, node.ReadNode("DataExtName"));
            Assert.AreEqual(innerRequest.DataExtAdd.DataExtValue, node.ReadNode("DataExtValue"));
            Assert.AreEqual("Company", node.ReadNode("OtherDataExtType"));
            Assert.IsEmpty(QuickBooksTestHelper.GetXmlValidation(xml));
        }

        [Test]
        public void ListDataExtAddRequestTest()
        {
            var request = new QbXmlRequest();
            var innerRequest = new DataExtAddRqType();
            innerRequest.DataExtAdd = new DataExtAdd
            {
                OwnerID = Guid.NewGuid().ToString(),
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

            Assert.AreEqual(1, requestXmlDoc.GetElementsByTagName("DataExtAddRq").Count);

            var node = requestXmlDoc.SelectSingleNode("//DataExtAddRq/DataExtAdd");
            Assert.AreEqual(innerRequest.DataExtAdd.OwnerID.ToString(), node.ReadNode("OwnerID"));
            Assert.AreEqual(innerRequest.DataExtAdd.DataExtName, node.ReadNode("DataExtName"));
            Assert.AreEqual(innerRequest.DataExtAdd.DataExtValue, node.ReadNode("DataExtValue"));
            Assert.AreEqual("Customer", node.ReadNode("ListDataExtType"));

            var node2 = node.SelectSingleNode("ListObjRef");
            Assert.AreEqual(innerRequest.DataExtAdd.ListObjRef.FullName, node2.ReadNode("FullName"));
            Assert.AreEqual(innerRequest.DataExtAdd.ListObjRef.ListID, node2.ReadNode("ListID"));
            Assert.IsEmpty(QuickBooksTestHelper.GetXmlValidation(xml));
        }

        [Test]
        public void TxnDataExtAddRequestTest()
        {
            var request = new QbXmlRequest();
            var innerRequest = new DataExtAddRqType();
            innerRequest.DataExtAdd = new DataExtAdd
            {
                OwnerID = Guid.NewGuid().ToString(),
                DataExtName = "name",
                DataExtValue = "value",
                TxnDataExtType = TxnDataExtType.Charge,
                TxnID = new DataExtAddTxnID
                {
                    Value = "123"
                },
                TxnLineID = "345"
            };
            request.AddToSingle(innerRequest);
            var xml = request.GetRequest();

            XmlDocument requestXmlDoc = new XmlDocument();
            requestXmlDoc.LoadXml(xml);

            Assert.AreEqual(1, requestXmlDoc.GetElementsByTagName("DataExtAddRq").Count);

            var node = requestXmlDoc.SelectSingleNode("//DataExtAddRq/DataExtAdd");
            Assert.AreEqual(innerRequest.DataExtAdd.OwnerID.ToString(), node.ReadNode("OwnerID"));
            Assert.AreEqual(innerRequest.DataExtAdd.DataExtName, node.ReadNode("DataExtName"));
            Assert.AreEqual(innerRequest.DataExtAdd.DataExtValue, node.ReadNode("DataExtValue"));
            Assert.AreEqual("Charge", node.ReadNode("TxnDataExtType"));
            Assert.AreEqual(innerRequest.DataExtAdd.TxnID.Value, node.ReadNode("TxnID"));
            Assert.AreEqual(innerRequest.DataExtAdd.TxnLineID, node.ReadNode("TxnLineID"));
            Assert.IsEmpty(QuickBooksTestHelper.GetXmlValidation(xml));
        }

        [Test]
        public void ListDataExtAddRequestValidAfterReorder()
        {
            var request = new QbXmlRequest();
            var innerRequest = new DataExtAddRqType();
            innerRequest.DataExtAdd = new DataExtAdd
            {
                OwnerID = Guid.NewGuid().ToString(),
                DataExtName = "name",
                DataExtValue = "value",
                ListObjRef = new ListObjRef
                {
                    FullName = "test",
                    ListID = "12345"
                },
                ListDataExtType = ListDataExtType.Customer
            };
            request.AddToSingle(innerRequest);
            var xml = request.GetRequest();

            XmlDocument requestXmlDoc = new XmlDocument();
            requestXmlDoc.LoadXml(xml);

            Assert.IsEmpty(QuickBooksTestHelper.GetXmlValidation(xml));
        }
    }
}