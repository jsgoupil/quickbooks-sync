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
    class CustomeAddRequestTests
    {
        [Test]
        public void BasicCustomerAddRequestTest()
        {
            var request = new QbXmlRequest();
            var innerRequest = new CustomerAddRqType();
            innerRequest.CustomerAdd = new CustomerAdd
            {
                Name = "Some Name"
            };
            request.AddToSingle(innerRequest);
            var xml = request.GetRequest();

            XmlDocument requestXmlDoc = new XmlDocument();
            requestXmlDoc.LoadXml(xml);

            Assert.AreEqual(1, requestXmlDoc.GetElementsByTagName("CustomerAddRq").Count);
            Assert.AreEqual(innerRequest.CustomerAdd.Name, requestXmlDoc.SelectSingleNode("//CustomerAdd/Name").InnerText);
            Assert.IsEmpty(QuickBooksTestHelper.GetXmlValidation(xml));
        }

        [Test]
        public void BasicCustomerAddRequest_TooLong_Test()
        {
            var request = new QbXmlRequest();
            var innerRequest = new CustomerAddRqType();
            innerRequest.CustomerAdd = new CustomerAdd
            {
                Name = "ABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZ" // 52 characters > 41
            };
            request.AddToSingle(innerRequest);
            var xml = request.GetRequest();

            XmlDocument requestXmlDoc = new XmlDocument();
            requestXmlDoc.LoadXml(xml);

            Assert.AreEqual(1, requestXmlDoc.GetElementsByTagName("CustomerAddRq").Count);
            Assert.AreEqual("ABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZ".Substring(0, 41), innerRequest.CustomerAdd.Name);
            Assert.AreEqual(innerRequest.CustomerAdd.Name, requestXmlDoc.SelectSingleNode("//CustomerAdd/Name").InnerText);
            Assert.IsEmpty(QuickBooksTestHelper.GetXmlValidation(xml));
        }

        [Test]
        public void UTF8CharactersMustBeEncoded()
        {
            var request = new QbXmlRequest();
            var innerRequest = new CustomerAddRqType();
            innerRequest.CustomerAdd = new CustomerAdd
            {
                Name = "Name",
                Notes = "Note—1é"
            };
            request.AddToSingle(innerRequest);
            var xml = request.GetRequest();

            XmlDocument requestXmlDoc = new XmlDocument();
            requestXmlDoc.LoadXml(xml);

            Assert.IsTrue(xml.Contains("Note&#8212;1&#233;"));
        }
    }
}