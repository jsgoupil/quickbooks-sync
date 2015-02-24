using NUnit.Framework;
using QbSync.QbXml.Objects;
using QbSync.QbXml.Tests.Helpers;
using System;

namespace QbSync.QbXml.Tests.QbXml
{
    [TestFixture]
    class DataExtDelResponseTests
    {
        [Test]
        public void BasicDataDelExtResponseTest()
        {
            var ret = "<DataExtDelRet><OwnerID>{7d543f23-f3b1-4dea-8ff4-37bd26d15e6c}</OwnerID><DataExtName>name</DataExtName><TxnDataExtType>CreditCardCredit</TxnDataExtType><TxnID>1234</TxnID><TimeDeleted>2015-02-10T00:00:00</TimeDeleted></DataExtDelRet>";

            var response = new QbXmlResponse();
            var rs = response.GetSingleItemFromResponse<DataExtDelRsType>(QuickBooksTestHelper.CreateQbXmlWithEnvelope(ret, "DataExtDelRs"));
            var dataExt = rs.DataExtDelRet;

            Assert.AreEqual("name", dataExt.DataExtName);
            Assert.AreEqual(TxnDataExtType.CreditCardCredit, dataExt.TxnDataExtType.Value);
            Assert.AreEqual("1234", dataExt.TxnID);
            Assert.AreEqual(new DATETIMETYPE(DateTime.Parse("2015-02-10")), dataExt.TimeDeleted);
        }
    }
}