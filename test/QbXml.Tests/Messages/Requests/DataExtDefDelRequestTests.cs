using NUnit.Framework;
using QbSync.QbXml.Extensions;
using QbSync.QbXml.Objects;
using QbSync.QbXml.Tests.Helpers;
using System;
using System.Xml;

namespace QbSync.QbXml.Tests.QbXml
{
    [TestFixture]
    class DataExtDefDelRequestTests
    {
        [Test]
        public void BasicDataExtDefDelRequestTest()
        {
            var request = new QbXmlRequest();
            var innerRequest = new DataExtDefDelRqType();
            innerRequest.OwnerID = Guid.NewGuid();
            innerRequest.DataExtName = "name";
            request.AddToSingle(innerRequest);
            var xml = request.GetRequest();

            XmlDocument requestXmlDoc = new XmlDocument();
            requestXmlDoc.LoadXml(xml);

            Assert.AreEqual(1, requestXmlDoc.GetElementsByTagName("DataExtDefDelRq").Count);

            var node = requestXmlDoc.SelectSingleNode("//DataExtDefDelRq");
            Assert.AreEqual(innerRequest.OwnerID.ToString(), node.ReadNode("OwnerID"));
            Assert.AreEqual(innerRequest.DataExtName, node.ReadNode("DataExtName"));
            Assert.IsEmpty(QuickBooksTestHelper.GetXmlValidation(xml));
        }
    }
}