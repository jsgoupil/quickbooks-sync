using NUnit.Framework;
using QbSync.QbXml.Extensions;
using QbSync.QbXml.Objects;
using QbSync.QbXml.Tests.Helpers;
using System.Xml;

namespace QbSync.QbXml.Tests.QbXml
{
    [TestFixture]
    class InvoiceModRequestTests
    {
        [Test]
        public void BasicInvoiceModRequestTest()
        {
            var request = new QbXmlRequest();
            var innerRequest = new InvoiceModRqType();
            innerRequest.InvoiceMod = new InvoiceMod
            {
                TxnID = "123",
                EditSequence = "456",
                CustomerRef = new CustomerRef
                {
                    ListID = "12345"
                },
                IsPending = true
            };
            request.AddToSingle(innerRequest);
            var xml = request.GetRequest();

            XmlDocument requestXmlDoc = new XmlDocument();
            requestXmlDoc.LoadXml(xml);

            Assert.AreEqual(1, requestXmlDoc.GetElementsByTagName("InvoiceModRq").Count);

            var node = requestXmlDoc.SelectSingleNode("//InvoiceModRq/InvoiceMod");
            Assert.AreEqual(innerRequest.InvoiceMod.TxnID, node.ReadNode("TxnID"));
            Assert.AreEqual(innerRequest.InvoiceMod.EditSequence, node.ReadNode("EditSequence"));
            Assert.AreEqual(innerRequest.InvoiceMod.CustomerRef.ListID, node.ReadNode("CustomerRef/ListID"));
            Assert.AreEqual(innerRequest.InvoiceMod.IsPending.ToString(), node.ReadNode("IsPending"));
            Assert.IsEmpty(QuickBooksTestHelper.GetXmlValidation(xml));
        }
    }
}