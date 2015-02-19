using NUnit.Framework;
using QbSync.QbXml.Messages;
using QbSync.QbXml.Messages.Responses;
using QbSync.QbXml.Objects;
using QbSync.QbXml.Tests.Helpers;
using System.Linq;

namespace QbSync.QbXml.Tests.QbXml
{
    [TestFixture]
    class InvoiceQueryResponseTests
    {
        [Test]
        public void BasicInvoiceResponseTest()
        {
            var invoiceRet = "<InvoiceRet><TxnID>80000001-1422671082</TxnID><TimeCreated>2015-01-30T18:24:42-08:00</TimeCreated><TimeModified>2015-01-30T18:24:42-08:00</TimeModified><EditSequence>1422671082</EditSequence><CustomerRef><ListID>123456</ListID></CustomerRef><TxnDate>2015-02-19</TxnDate></InvoiceRet>";

            var invoiceResponse = new InvoiceQueryResponse();
            var response = invoiceResponse.ParseResponse(QuickBooksTestHelper.CreateQbXmlWithEnvelope(invoiceRet, "InvoiceQueryRs"));
            var invoices = response.Object;
            var invoice = invoices[0];

            Assert.AreEqual(1, invoices.Length);
            QBAssert.AreEqual("80000001-1422671082", invoice.TxnID);
            QBAssert.AreEqual("123456", invoice.CustomerRef.ListID);
        }
    }
}