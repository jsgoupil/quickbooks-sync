using NUnit.Framework;
using QbSync.QbXml.Objects;
using QbSync.QbXml.Tests.Helpers;

namespace QbSync.QbXml.Tests.QbXml
{
    [TestFixture]
    class DataExtAddResponseTests
    {
        [Test]
        public void BasicDataExtAddResponseTest()
        {
            var ret = "<DataExtRet><OwnerID>{7d543f23-f3b1-4dea-8ff4-37bd26d15e6c}</OwnerID><DataExtName>name</DataExtName><DataExtType>STR255TYPE</DataExtType><DataExtValue>value</DataExtValue></DataExtRet>";

            var response = new QbXmlResponse();
            var rs = response.GetSingleItemFromResponse<DataExtAddRsType>(QuickBooksTestHelper.CreateQbXmlWithEnvelope(ret, "DataExtAddRs"));
            var dataExt = rs.DataExtRet;

            Assert.AreEqual("name", dataExt.DataExtName);
            Assert.AreEqual("value", dataExt.DataExtValue);
            Assert.AreEqual(DataExtType.STR255TYPE, dataExt.DataExtType);
        }
    }
}