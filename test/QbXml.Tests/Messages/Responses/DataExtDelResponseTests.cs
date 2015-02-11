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
    class DataExtDelResponseTests
    {
        [Test]
        public void BasicDataExtDelResponseTest()
        {
            var ret = "<DataExtDelRet><OwnerID>{7d543f23-f3b1-4dea-8ff4-37bd26d15e6c}</OwnerID><DataExtName>name</DataExtName><TxnDataExtType>CreditCardCredit</TxnDataExtType><TxnID>1234</TxnID><TimeDeleted>2015-02-10</TimeDeleted></DataExtDelRet>";

            var dataExtDelResponse = new DataExtDelResponse();
            var response = dataExtDelResponse.ParseResponse(QuickBooksTestHelper.CreateQbXmlWithEnvelope(ret, "DataExtDelRs"));
            var dataExt = response.Object;

            QBAssert.AreEqual("name", dataExt.DataExtName);
            Assert.AreEqual(TxnDataExtType.CreditCardCredit, dataExt.TxnDataExtType);
            QBAssert.AreEqual("1234", dataExt.TxnID);
            QBAssert.AreEqual(DateTime.Parse("2015-02-10").ToString("s"), dataExt.TimeDeleted);
        }
    }
}