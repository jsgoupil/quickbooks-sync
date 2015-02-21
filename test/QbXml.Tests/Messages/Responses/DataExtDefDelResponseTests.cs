using NUnit.Framework;
using QbSync.QbXml.Objects;
using QbSync.QbXml.Tests.Helpers;

namespace QbSync.QbXml.Tests.QbXml
{
    [TestFixture]
    class DataExtDefDelResponseTests
    {
        [Test]
        public void BasicDataExtDefDelResponseTest()
        {
            var ret = "<DataExtDefDelRet><OwnerID>{7d543f23-f3b1-4dea-8ff4-37bd26d15e6c}</OwnerID><DataExtName>name</DataExtName></DataExtDefDelRet>";

            var response = new QbXmlResponse();
            var rs = response.GetSingleItemFromResponse<DataExtDefDelRsType>(QuickBooksTestHelper.CreateQbXmlWithEnvelope(ret, "DataExtDefDelRs"));
            var dataExtDef = rs.DataExtDefDelRet;

            Assert.AreEqual("name", dataExtDef.DataExtName);
        }
    }
}