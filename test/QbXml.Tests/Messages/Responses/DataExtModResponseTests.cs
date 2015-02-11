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
    class DataExtModResponseTests
    {
        [Test]
        public void BasicDataModExtResponseTest()
        {
            var ret = "<DataExtRet><OwnerID>{7d543f23-f3b1-4dea-8ff4-37bd26d15e6c}</OwnerID><DataExtName>name</DataExtName><DataExtValue>value</DataExtValue><DataExtType>STR255TYPE</DataExtType></DataExtRet>";

            var dataExtModResponse = new DataExtModResponse();
            var response = dataExtModResponse.ParseResponse(QuickBooksTestHelper.CreateQbXmlWithEnvelope(ret, "DataExtModRs"));
            var dataExt = response.Object;

            QBAssert.AreEqual("name", dataExt.DataExtName);
            QBAssert.AreEqual("value", dataExt.DataExtValue);
            Assert.AreEqual(DataExtType.STR255TYPE, dataExt.DataExtType);
        }
    }
}