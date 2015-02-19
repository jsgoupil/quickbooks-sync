using NUnit.Framework;
using QbSync.QbXml.Messages;
using QbSync.QbXml.Messages.Responses;
using QbSync.QbXml.Objects;
using QbSync.QbXml.Struct;
using QbSync.QbXml.Tests.Helpers;
using System.Linq;

namespace QbSync.QbXml.Tests.QbXml
{
    [TestFixture]
    class InvoiceModResponseTests
    {
        [Test]
        public void BasicInvoiceModResponseTest()
        {
            var invoiceRet = "<InvoiceRet><TxnID>80000001-1422671082</TxnID><TimeCreated>2015-01-30T18:24:42-08:00</TimeCreated><TimeModified>2015-01-30T18:24:42-08:00</TimeModified><EditSequence>1422671082</EditSequence><CustomerRef><ListID>123456</ListID></CustomerRef><TxnDate>2015-02-19</TxnDate></InvoiceRet>";

            var invoiceModResponse = new InvoiceModResponse();
            var response = invoiceModResponse.ParseResponse(QuickBooksTestHelper.CreateQbXmlWithEnvelope(invoiceRet, "InvoiceModRs"));
            var invoice = response.Object;

            QBAssert.AreEqual("80000001-1422671082", invoice.TxnID);
            QBAssert.AreEqual("123456", invoice.CustomerRef.ListID);
        }
    }
}