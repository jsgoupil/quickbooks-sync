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
    class DataExtDefDelResponseTests
    {
        [Test]
        public void BasicDataExtDefDelResponseTest()
        {
            var ret = "<DataExtDefDelRet><OwnerID>{7d543f23-f3b1-4dea-8ff4-37bd26d15e6c}</OwnerID><DataExtName>name</DataExtName></DataExtDefDelRet>";

            var dataExtDefDelResponse = new DataExtDefDelResponse();
            var response = dataExtDefDelResponse.ParseResponse(QuickBooksTestHelper.CreateQbXmlWithEnvelope(ret, "DataExtDefDelRs"));
            var dataDefExt = response.Object;

            QBAssert.AreEqual("name", dataDefExt.DataExtName);
        }
    }
}