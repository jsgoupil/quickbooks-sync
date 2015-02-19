using NUnit.Framework;
using QbSync.QbXml.Extensions;
using QbSync.QbXml.Messages.Requests;
using QbSync.QbXml.Objects;
using QbSync.QbXml.Struct;
using QbSync.QbXml.Tests.Helpers;
using System;
using System.Xml;

namespace QbSync.QbXml.Tests.QbXml
{
    [TestFixture]
    class InvoiceAddRequestTests
    {
        [Test]
        public void BasicInvoiceAddRequestTest()
        {
            var invoiceAddRequest = new InvoiceAddRequest();
            invoiceAddRequest.CustomerRef = new Ref
            {
                ListID = "12345"
            };
            invoiceAddRequest.IsPending = new Type.BoolType(true);

            var request = invoiceAddRequest.GetRequest();

            XmlDocument requestXmlDoc = new XmlDocument();
            requestXmlDoc.LoadXml(request);

            Assert.AreEqual(1, requestXmlDoc.GetElementsByTagName("InvoiceAddRq").Count);

            var node = requestXmlDoc.SelectSingleNode("//InvoiceAddRq/InvoiceAdd");
            QBAssert.AreEqual(invoiceAddRequest.CustomerRef.ListID, node.ReadNode("CustomerRef/ListID"));
            QBAssert.AreEqual(invoiceAddRequest.IsPending, node.ReadNode("IsPending"));
            Assert.IsEmpty(QuickBooksTestHelper.GetXmlValidation(request));
        }
    }
}