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
    class DataExtDefAddResponseTests
    {
        [Test]
        public void BasicDataExtDefAddResponseTest()
        {
            var ret = "<DataExtDefRet><OwnerID>{7d543f23-f3b1-4dea-8ff4-37bd26d15e6c}</OwnerID><DataExtName>name</DataExtName><DataExtID>123</DataExtID><DataExtType>STR255TYPE</DataExtType><AssignToObject>Account</AssignToObject><AssignToObject>Charge</AssignToObject></DataExtDefRet>";

            var dataExtDefAddResponse = new DataExtDefAddResponse();
            var response = dataExtDefAddResponse.ParseResponse(QuickBooksTestHelper.CreateQbXmlWithEnvelope(ret, "DataExtDefAddRs"));
            var dataDefExt = response.Object;

            QBAssert.AreEqual("name", dataDefExt.DataExtName);
            Assert.AreEqual(DataExtType.STR255TYPE, dataDefExt.DataExtType);
            Assert.AreEqual(2, dataDefExt.AssignToObject.Count());
            Assert.AreEqual(AssignToObject.Account, dataDefExt.AssignToObject.First());
            Assert.AreEqual(AssignToObject.Charge, dataDefExt.AssignToObject.Last());
        }
    }
}