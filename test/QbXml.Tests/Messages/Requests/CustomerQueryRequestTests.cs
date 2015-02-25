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
    class CustomerQueryRequestTests
    {
        [Test]
        public void BasicCustomerQueryRequestTest()
        {
            var request = new QbXmlRequest();
            var innerRequest = new CustomerQueryRqType();
            request.AddToSingle(innerRequest);
            var xml = request.GetRequest();

            XmlDocument requestXmlDoc = new XmlDocument();
            requestXmlDoc.LoadXml(xml);

            Assert.AreEqual(1, requestXmlDoc.GetElementsByTagName("CustomerQueryRq").Count);
            Assert.AreEqual(0, requestXmlDoc.GetElementsByTagName("ListID").Count);
            Assert.AreEqual(0, requestXmlDoc.GetElementsByTagName("FullName").Count);
            Assert.IsEmpty(QuickBooksTestHelper.GetXmlValidation(xml));

            // Make sure we have no xmlns attribute
            Assert.IsTrue(!xml.Contains("xmlns"), "The XML contains XMLNS. The Web Connector will reject the request.");
        }

        [Test]
        public void BasicCustomerQueryWithSomeFilters()
        {
            var request = new QbXmlRequest();
            var innerRequest = new CustomerQueryRqType();
            innerRequest.ActiveStatus = ActiveStatus.ActiveOnly;
            innerRequest.OwnerID = new GUIDTYPE[] { new GUIDTYPE(Guid.NewGuid()) };
            innerRequest.MaxReturned = "100";
            request.AddToSingle(innerRequest);
            var xml = request.GetRequest();

            XmlDocument requestXmlDoc = new XmlDocument();
            requestXmlDoc.LoadXml(xml);

            Assert.AreEqual(1, requestXmlDoc.GetElementsByTagName("CustomerQueryRq").Count);
            Assert.AreEqual(0, requestXmlDoc.GetElementsByTagName("ListID").Count);
            Assert.AreEqual(0, requestXmlDoc.GetElementsByTagName("FullName").Count);
            Assert.IsEmpty(QuickBooksTestHelper.GetXmlValidation(xml));
        }

        [Test]
        public void CustomerQueryRequest_FilterByListId_Test()
        {
            var list = new List<string> {
                "1234", "3456"
            };
            var request = new QbXmlRequest();
            var innerRequest = new CustomerQueryRqType();
            innerRequest.ListID = list;
            request.AddToSingle(innerRequest);
            var xml = request.GetRequest();

            XmlDocument requestXmlDoc = new XmlDocument();
            requestXmlDoc.LoadXml(xml);

            Assert.AreEqual(innerRequest.ListID.Count(), requestXmlDoc.GetElementsByTagName("ListID").Count);
            Assert.AreEqual(list[0], requestXmlDoc.GetElementsByTagName("ListID").Item(0).InnerText);
            Assert.AreEqual(list[1], requestXmlDoc.GetElementsByTagName("ListID").Item(1).InnerText);
            Assert.IsEmpty(QuickBooksTestHelper.GetXmlValidation(xml));
        }

        [Test]
        public void CustomerQueryRequest_FilterByFullname_Test()
        {
            var list = new List<string> {
                "abc", "def"
            };
            var request = new QbXmlRequest();
            var innerRequest = new CustomerQueryRqType();
            innerRequest.FullName = list;
            request.AddToSingle(innerRequest);
            var xml = request.GetRequest();

            XmlDocument requestXmlDoc = new XmlDocument();
            requestXmlDoc.LoadXml(xml);

            Assert.AreEqual(innerRequest.FullName.Count(), requestXmlDoc.GetElementsByTagName("FullName").Count);
            Assert.AreEqual(list[0], requestXmlDoc.GetElementsByTagName("FullName").Item(0).InnerText);
            Assert.AreEqual(list[1], requestXmlDoc.GetElementsByTagName("FullName").Item(1).InnerText);
            Assert.IsEmpty(QuickBooksTestHelper.GetXmlValidation(xml));
        }

        [Test]
        public void CustomerQueryRequest_FilterByNameRange_Test()
        {
            var request = new QbXmlRequest();
            var innerRequest = new CustomerQueryRqType();
            innerRequest.NameRangeFilter = new NameRangeFilter
            {
                FromName = "ab",
                ToName = "ac"
            };
            request.AddToSingle(innerRequest);
            var xml = request.GetRequest();

            XmlDocument requestXmlDoc = new XmlDocument();
            requestXmlDoc.LoadXml(xml);

            var nameRangeFilter = requestXmlDoc.GetElementsByTagName("NameRangeFilter");
            Assert.AreEqual(1, nameRangeFilter.Count);
            Assert.AreEqual(innerRequest.NameRangeFilter.FromName, nameRangeFilter.Item(0).ReadNode("./FromName"));
            Assert.AreEqual(innerRequest.NameRangeFilter.ToName, nameRangeFilter.Item(0).ReadNode("./ToName"));
            Assert.IsEmpty(QuickBooksTestHelper.GetXmlValidation(xml));
        }
    }
}