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
    class InvoiceAddResponseTests
    {
        [Test]
        public void BasicInvoiceAddResponseTest()
        {
            var invoiceRet = "<InvoiceRet><TxnID>80000001-1422671082</TxnID><TimeCreated>2015-01-30T18:24:42-08:00</TimeCreated><TimeModified>2015-01-30T18:24:42-08:00</TimeModified><EditSequence>1422671082</EditSequence><CustomerRef><ListID>123456</ListID></CustomerRef><TxnDate>2015-02-19</TxnDate></InvoiceRet>";

            var invoiceAddResponse = new InvoiceAddResponse();
            var response = invoiceAddResponse.ParseResponse(QuickBooksTestHelper.CreateQbXmlWithEnvelope(invoiceRet, "InvoiceAddRs"));
            var invoice = response.Object;

            QBAssert.AreEqual("80000001-1422671082", invoice.TxnID);
            QBAssert.AreEqual("123456", invoice.CustomerRef.ListID);
        }
    }
}