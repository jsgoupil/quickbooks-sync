using NUnit.Framework;
using QbSync.QbXml.Extensions;
using QbSync.QbXml.Messages.Requests;
using QbSync.QbXml.Objects;
using QbSync.QbXml.Tests.Helpers;
using QbSync.QbXml.Type;
using System.Xml;

namespace QbSync.QbXml.Tests.QbXml
{
    [TestFixture]
    class InvoiceAddRequestTests
    {
        [Test]
        public void BasicInvoiceAddRequestTest()
        {
            var request = new QbXmlRequest();
            var innerRequest = new InvoiceAddRequest();
            innerRequest.CustomerRef = new CustomerRef
            {
                ListID = "12345"
            };
            innerRequest.IsPending = new BoolType(true);
            request.AddToSingle(innerRequest);
            var xml = request.GetRequest();

            XmlDocument requestXmlDoc = new XmlDocument();
            requestXmlDoc.LoadXml(xml);

            Assert.AreEqual(1, requestXmlDoc.GetElementsByTagName("InvoiceAddRq").Count);

            var node = requestXmlDoc.SelectSingleNode("//InvoiceAddRq/InvoiceAdd");
            Assert.AreEqual(innerRequest.CustomerRef.ListID, node.ReadNode("CustomerRef/ListID"));
            Assert.AreEqual(innerRequest.IsPending.ToString(), node.ReadNode("IsPending"));
            Assert.IsEmpty(QuickBooksTestHelper.GetXmlValidation(xml));
        }
    }
}