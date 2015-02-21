using NUnit.Framework;
using QbSync.QbXml.Objects;
using QbSync.QbXml.Tests.Helpers;
using QbSync.QbXml.Wrappers;

namespace QbSync.QbXml.Tests.QbXml
{
    [TestFixture]
    class DataExtDelResponseErrorRecoveryTests
    {
        [Test]
        public void ErrorRecoveryBasicWithDataExtDel()
        {
            var ret = "<ErrorRecovery><ListID>123456</ListID><TxnNumber>67890</TxnNumber></ErrorRecovery>";

            var response = new QbXmlResponse();
            var rs = response.GetSingleItemFromResponse<DataExtDelRsType>(QuickBooksTestHelper.CreateQbXmlWithEnvelope(ret, "DataExtDelRs"));
            var dataExt = rs.DataExtDelRet;

            var wrapper = new ErrorRecoveryWrapper(rs.ErrorRecovery);
            Assert.IsNull(dataExt);
            Assert.AreEqual("123456", wrapper.ListID);
            Assert.AreEqual("67890", wrapper.TxnNumber);
        }
    }
}