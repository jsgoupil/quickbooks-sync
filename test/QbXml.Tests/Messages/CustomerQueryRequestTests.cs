using NUnit.Framework;
using QbSync.QbXml.Extensions;
using QbSync.QbXml.Filters;
using QbSync.QbXml.Messages;
using QbSync.QbXml.Struct;
using QbSync.QbXml.Tests.Helpers;
using QbSync.QbXml.Type;
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
            var customerQueryRequest = new CustomerQueryRequest();
            var request = customerQueryRequest.GetRequest();

            XmlDocument requestXmlDoc = new XmlDocument();
            requestXmlDoc.LoadXml(request);

            Assert.AreEqual(1, requestXmlDoc.GetElementsByTagName("CustomerQueryRq").Count);
            Assert.AreEqual(0, requestXmlDoc.GetElementsByTagName("ListID").Count);
            Assert.AreEqual(0, requestXmlDoc.GetElementsByTagName("FullName").Count);
        }

        [Test]
        public void CustomerQueryRequest_FilterByListId_Test()
        {
            var list = new List<IdType> {
                "1234", "3456"
            };
            var customerQueryRequest = new CustomerQueryRequest();
            customerQueryRequest.Filter = CustomerQueryRequestFilter.ListId;
            customerQueryRequest.ListID = list;
            var request = customerQueryRequest.GetRequest();

            XmlDocument requestXmlDoc = new XmlDocument();
            requestXmlDoc.LoadXml(request);

            Assert.AreEqual(customerQueryRequest.ListID.Count(), requestXmlDoc.GetElementsByTagName("ListID").Count);
            QBAssert.AreEqual(list[0], requestXmlDoc.GetElementsByTagName("ListID").Item(0).InnerText);
            QBAssert.AreEqual(list[1], requestXmlDoc.GetElementsByTagName("ListID").Item(1).InnerText);
        }

        [Test]
        public void CustomerQueryRequest_FilterByFullname_Test()
        {
            var list = new List<StrType> {
                "abc", "def"
            };
            var customerQueryRequest = new CustomerQueryRequest();
            customerQueryRequest.Filter = CustomerQueryRequestFilter.FullName;
            customerQueryRequest.FullName = list;
            var request = customerQueryRequest.GetRequest();

            XmlDocument requestXmlDoc = new XmlDocument();
            requestXmlDoc.LoadXml(request);

            Assert.AreEqual(customerQueryRequest.FullName.Count(), requestXmlDoc.GetElementsByTagName("FullName").Count);
            QBAssert.AreEqual(list[0], requestXmlDoc.GetElementsByTagName("FullName").Item(0).InnerText);
            QBAssert.AreEqual(list[1], requestXmlDoc.GetElementsByTagName("FullName").Item(1).InnerText);
        }

        [Test]
        public void CustomerQueryRequest_FilterByNameRange_Test()
        {
            var customerQueryRequest = new CustomerQueryRequest();
            customerQueryRequest.NameRangeFilter = new NameRangeFilter
            {
                FromName = "ab",
                ToName = "ac"
            };

            var request = customerQueryRequest.GetRequest();

            XmlDocument requestXmlDoc = new XmlDocument();
            requestXmlDoc.LoadXml(request);

            var nameRangeFilter = requestXmlDoc.GetElementsByTagName("NameRangeFilter");
            Assert.AreEqual(1, nameRangeFilter.Count);
            QBAssert.AreEqual(customerQueryRequest.NameRangeFilter.FromName, nameRangeFilter.Item(0).ReadNode("./FromName"));
            QBAssert.AreEqual(customerQueryRequest.NameRangeFilter.ToName, nameRangeFilter.Item(0).ReadNode("./ToName"));
        }
    }
}