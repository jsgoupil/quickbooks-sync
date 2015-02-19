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
    class InvoiceModRequestTests
    {
        [Test]
        public void BasicInvoiceModRequestTest()
        {
            var invoiceModRequest = new InvoiceModRequest();
            invoiceModRequest.TxnID = "56789";
            invoiceModRequest.EditSequence = "111";
            invoiceModRequest.CustomerRef = new Ref
            {
                ListID = "12345"
            };
            invoiceModRequest.IsPending = new Type.BoolType(true);

            var request = invoiceModRequest.GetRequest();

            XmlDocument requestXmlDoc = new XmlDocument();
            requestXmlDoc.LoadXml(request);

            Assert.AreEqual(1, requestXmlDoc.GetElementsByTagName("InvoiceModRq").Count);

            var node = requestXmlDoc.SelectSingleNode("//InvoiceModRq/InvoiceMod");
            QBAssert.AreEqual(invoiceModRequest.TxnID, node.ReadNode("TxnID"));
            QBAssert.AreEqual(invoiceModRequest.EditSequence, node.ReadNode("EditSequence"));
            QBAssert.AreEqual(invoiceModRequest.CustomerRef.ListID, node.ReadNode("CustomerRef/ListID"));
            QBAssert.AreEqual(invoiceModRequest.IsPending, node.ReadNode("IsPending"));
            Assert.IsEmpty(QuickBooksTestHelper.GetXmlValidation(request));
        }
    }
}