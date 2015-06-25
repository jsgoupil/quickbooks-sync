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
            Assert.IsEmpty(QuickBooksTestHelper.GetXmlValidation(xml));
        }

        [Test]
        public void SpecialCharactersMustBeEncoded()
        {
            var request = new QbXmlRequest();
            var innerRequest = new CustomerAddRqType();
            innerRequest.CustomerAdd = new CustomerAdd
            {
                Name = "Name",
                Notes = "<>'\"&é—"
            };
            request.AddToSingle(innerRequest);
            var xml = request.GetRequest();

            XmlDocument requestXmlDoc = new XmlDocument();
            requestXmlDoc.LoadXml(xml);

            Assert.IsTrue(xml.Contains("&lt;&gt;&#39;&quot;&amp;&#233;&#8212;"));
            Assert.IsEmpty(QuickBooksTestHelper.GetXmlValidation(xml));
        }

        [Test]
        public void LForCRCountsAsTwoCharacters()
        {
            var request = new QbXmlRequest();
            var innerRequest = new CustomerAddRqType();
            innerRequest.CustomerAdd = new CustomerAdd
            {
                Name = "Name",
                BillAddress = new BillAddress
                {
                    Addr1 = "ABCDEFGHIJKLMNOPQRSTUVWXYZ\nABCDEFGHIJKLMNOPQRSTUVWXYZ",
                    //                                                ^ is the 41st character but M should be the last character
                    Addr2 = "ABCDEFGHIJKLMNOPQRSTUVWXYZ\rABCDEFGHIJKLMNOPQRSTUVWXYZ",
                    //                                                ^ is the 41st character but M should be the last character
                    Addr3 = "ABCDEFGHIJKLMNOPQRSTUVWXYZ\r\nABCDEFGHIJKLMNOPQRSTUVWXYZ"
                    //                                                 ^ is the 41st
                }
            };
            request.AddToSingle(innerRequest);
            var xml = request.GetRequest();

            XmlDocument requestXmlDoc = new XmlDocument();
            requestXmlDoc.LoadXml(xml);

            // On Windows, \r or \n will be transformed into \r\n.
            Assert.AreEqual("ABCDEFGHIJKLMNOPQRSTUVWXYZ\r\nABCDEFGHIJKLM", innerRequest.CustomerAdd.BillAddress.Addr1);
            Assert.AreEqual("ABCDEFGHIJKLMNOPQRSTUVWXYZ\r\nABCDEFGHIJKLM", innerRequest.CustomerAdd.BillAddress.Addr2);
            Assert.AreEqual("ABCDEFGHIJKLMNOPQRSTUVWXYZ\r\nABCDEFGHIJKLM", innerRequest.CustomerAdd.BillAddress.Addr3);
        }
    }
}