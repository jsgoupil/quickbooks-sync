using NUnit.Framework;
using QbSync.QbXml.Extensions;
using QbSync.QbXml.Objects;
using QbSync.QbXml.Tests.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace QbSync.QbXml.Tests.QbXml
{
    [TestFixture]
    class InvoiceQueryRequestTests
    {
        [Test]
        public void BasicInvoiceQueryRequestTest()
        {
            var request = new QbXmlRequest();
            var innerRequest = new InvoiceQueryRqType();
            request.AddToSingle(innerRequest);
            var xml = request.GetRequest();

            XmlDocument requestXmlDoc = new XmlDocument();
            requestXmlDoc.LoadXml(xml);

            Assert.AreEqual(1, requestXmlDoc.GetElementsByTagName("InvoiceQueryRq").Count);
            Assert.AreEqual(0, requestXmlDoc.GetElementsByTagName("ListID").Count);
            Assert.AreEqual(0, requestXmlDoc.GetElementsByTagName("FullName").Count);
            Assert.IsEmpty(QuickBooksTestHelper.GetXmlValidation(xml));
        }

        [Test]
        public void InvoiceQueryRequest_FilterByTxnId_Test()
        {
            var request = new QbXmlRequest();
            var innerRequest = new InvoiceQueryRqType();
            innerRequest.TxnID = new List<string> {
                "1234", "3456"
            };
            request.AddToSingle(innerRequest);
            var xml = request.GetRequest();

            XmlDocument requestXmlDoc = new XmlDocument();
            requestXmlDoc.LoadXml(xml);

            Assert.AreEqual(innerRequest.TxnID.Count(), requestXmlDoc.GetElementsByTagName("TxnID").Count);
            Assert.AreEqual(innerRequest.TxnID.Last(), requestXmlDoc.GetElementsByTagName("TxnID").Item(1).InnerText);
            Assert.AreEqual(innerRequest.TxnID.First(), requestXmlDoc.GetElementsByTagName("TxnID").Item(0).InnerText);
            Assert.IsEmpty(QuickBooksTestHelper.GetXmlValidation(xml));
        }

        [Test]
        public void InvoiceQueryRequest_FilterByRefNumber_Test()
        {
            var request = new QbXmlRequest();
            var innerRequest = new InvoiceQueryRqType();
            innerRequest.RefNumber = new List<string> {
                "abc", "def"
            };
            request.AddToSingle(innerRequest);
            var xml = request.GetRequest();

            XmlDocument requestXmlDoc = new XmlDocument();
            requestXmlDoc.LoadXml(xml);

            Assert.AreEqual(innerRequest.RefNumber.Count(), requestXmlDoc.GetElementsByTagName("RefNumber").Count);
            Assert.AreEqual(innerRequest.RefNumber.First(), requestXmlDoc.GetElementsByTagName("RefNumber").Item(0).InnerText);
            Assert.AreEqual(innerRequest.RefNumber.Last(), requestXmlDoc.GetElementsByTagName("RefNumber").Item(1).InnerText);
            Assert.IsEmpty(QuickBooksTestHelper.GetXmlValidation(xml));
        }

        [Test]
        public void InvoiceQueryRequest_ModifiedDateRange_Test()
        {
            var request = new QbXmlRequest();
            var innerRequest = new InvoiceQueryRqType();
            innerRequest.ModifiedDateRangeFilter = new ModifiedDateRangeFilter
            {
                FromModifiedDate = new DATETIMETYPE(new DateTime(2014, 1, 1, 1, 2, 3)),
                ToModifiedDate = new DATETIMETYPE(new DateTime(2014, 1, 1, 1, 2, 3))
            };
            request.AddToSingle(innerRequest);
            var xml = request.GetRequest();

            XmlDocument requestXmlDoc = new XmlDocument();
            requestXmlDoc.LoadXml(xml);

            var nameRangeFilter = requestXmlDoc.GetElementsByTagName("ModifiedDateRangeFilter");
            Assert.AreEqual(1, nameRangeFilter.Count);
            Assert.AreEqual(innerRequest.ModifiedDateRangeFilter.FromModifiedDate.ToString(), nameRangeFilter.Item(0).ReadNode("./FromModifiedDate"));
            Assert.AreEqual(innerRequest.ModifiedDateRangeFilter.ToModifiedDate.ToString(), nameRangeFilter.Item(0).ReadNode("./ToModifiedDate"));
            Assert.IsEmpty(QuickBooksTestHelper.GetXmlValidation(xml));
        }
    }
}