using NUnit.Framework;
using QbSync.QbXml.Messages;
using QbSync.QbXml.Messages.Responses;
using QbSync.QbXml.Objects;
using QbSync.QbXml.Struct;
using QbSync.QbXml.Tests.Helpers;
using System;
using System.Linq;

namespace QbSync.QbXml.Tests.QbXml
{
    [TestFixture]
    class DataExtDelResponseErrorRecoveryTests
    {
        [Test]
        public void ErrorRecoveryBasicWithDataExtDel()
        {
            var ret = "<ErrorRecovery><ListID>123456</ListID><TxnNumber>67890</TxnNumber></ErrorRecovery>";

            var dataExtDelResponse = new DataExtDelResponse();
            var response = dataExtDelResponse.ParseResponse(QuickBooksTestHelper.CreateQbXmlWithEnvelope(ret, "DataExtDelRs")) as QbXmlMsgResponseWithErrorRecovery<DataExtDel>;
            var dataExt = response.Object;

            Assert.IsNull(dataExt);
            QBAssert.AreEqual("123456", response.ErrorRecovery.ListID);
            QBAssert.AreEqual("67890", response.ErrorRecovery.TxnNumber);
        }
    }
}