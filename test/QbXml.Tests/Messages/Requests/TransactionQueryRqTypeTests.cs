using NUnit.Framework;
using QbSync.QbXml.Objects;

namespace QbSync.QbXml.Tests.QbXml
{
    [TestFixture]
    public class TransactionQueryRqTypeTests
    {
        [Test]
        public void TransactionTypeFilterArray()
        {
            var filter = new TxnTypeFilter[]
            {
                TxnTypeFilter.Invoice
            };

            var request = new TransactionQueryRqType
            {
                TransactionTypeFilter = filter
            };

            Assert.AreEqual(filter, request.TransactionTypeFilter);
        }
    }
}

