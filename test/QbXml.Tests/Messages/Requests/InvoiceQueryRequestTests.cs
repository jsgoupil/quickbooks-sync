using NUnit.Framework;
using QbSync.QbXml.Extensions;
using QbSync.QbXml.Filters;
using QbSync.QbXml.Messages;
using QbSync.QbXml.Messages.Requests;
using QbSync.QbXml.Struct;
using QbSync.QbXml.Tests.Helpers;
using QbSync.QbXml.Type;
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
            var invoiceQueryRequest = new InvoiceQueryRequest();
            var request = invoiceQueryRequest.GetRequest();

            XmlDocument requestXmlDoc = new XmlDocument();
            requestXmlDoc.LoadXml(request);

            Assert.AreEqual(1, requestXmlDoc.GetElementsByTagName("InvoiceQueryRq").Count);
            Assert.AreEqual(0, requestXmlDoc.GetElementsByTagName("ListID").Count);
            Assert.AreEqual(0, requestXmlDoc.GetElementsByTagName("FullName").Count);
            Assert.IsEmpty(QuickBooksTestHelper.GetXmlValidation(request));
        }

        [Test]
        public void InvoiceQueryRequest_FilterByTxnId_Test()
        {
            var list = new List<IdType> {
                "1234", "3456"
            };
            var invoiceQueryRequest = new InvoiceQueryRequest();
            invoiceQueryRequest.Filter = InvoiceQueryRequestFilter.TxnID;
            invoiceQueryRequest.TxnID = list;
            var request = invoiceQueryRequest.GetRequest();

            XmlDocument requestXmlDoc = new XmlDocument();
            requestXmlDoc.LoadXml(request);

            Assert.AreEqual(invoiceQueryRequest.TxnID.Count(), requestXmlDoc.GetElementsByTagName("TxnID").Count);
            QBAssert.AreEqual(list[0], requestXmlDoc.GetElementsByTagName("TxnID").Item(0).InnerText);
            QBAssert.AreEqual(list[1], requestXmlDoc.GetElementsByTagName("TxnID").Item(1).InnerText);
            Assert.IsEmpty(QuickBooksTestHelper.GetXmlValidation(request));
        }

        [Test]
        public void InvoiceQueryRequest_FilterByRefNumber_Test()
        {
            var list = new List<StrType> {
                "abc", "def"
            };
            var invoiceQueryRequest = new InvoiceQueryRequest();
            invoiceQueryRequest.Filter = InvoiceQueryRequestFilter.RefNumber;
            invoiceQueryRequest.RefNumber = list;
            var request = invoiceQueryRequest.GetRequest();

            XmlDocument requestXmlDoc = new XmlDocument();
            requestXmlDoc.LoadXml(request);

            Assert.AreEqual(invoiceQueryRequest.RefNumber.Count(), requestXmlDoc.GetElementsByTagName("RefNumber").Count);
            QBAssert.AreEqual(list[0], requestXmlDoc.GetElementsByTagName("RefNumber").Item(0).InnerText);
            QBAssert.AreEqual(list[1], requestXmlDoc.GetElementsByTagName("RefNumber").Item(1).InnerText);
            Assert.IsEmpty(QuickBooksTestHelper.GetXmlValidation(request));
        }

        [Test]
        public void InvoiceQueryRequest_ModifiedDateRange_Test()
        {
            var invoiceQueryRequest = new InvoiceQueryRequest();
            invoiceQueryRequest.ModifiedDateRangeFilter = new ModifiedDateRangeFilter
            {
                FromModifiedDate = new DateTimeType(new DateTime(2014, 1, 1, 1, 2, 3)),
                ToModifiedDate = new DateTimeType(new DateTime(2014, 1, 1, 1, 2, 3))
            };

            var request = invoiceQueryRequest.GetRequest();

            XmlDocument requestXmlDoc = new XmlDocument();
            requestXmlDoc.LoadXml(request);

            var nameRangeFilter = requestXmlDoc.GetElementsByTagName("ModifiedDateRangeFilter");
            Assert.AreEqual(1, nameRangeFilter.Count);
            Assert.AreEqual(invoiceQueryRequest.ModifiedDateRangeFilter.FromModifiedDate.ToString(), nameRangeFilter.Item(0).ReadNode("./FromModifiedDate"));
            Assert.AreEqual(invoiceQueryRequest.ModifiedDateRangeFilter.ToModifiedDate.ToString(), nameRangeFilter.Item(0).ReadNode("./ToModifiedDate"));
            Assert.IsEmpty(QuickBooksTestHelper.GetXmlValidation(request));
        }
    }
}