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
    class DataExtDefQueryResponseTests
    {
        [Test]
        public void BasicDataDefQueryExtResponseTest()
        {
            var ret = "<DataExtDefRet><OwnerID>{7d543f23-f3b1-4dea-8ff4-37bd26d15e6c}</OwnerID><DataExtName>name</DataExtName><DataExtID>123</DataExtID><DataExtType>STR255TYPE</DataExtType><AssignToObject>Account</AssignToObject><AssignToObject>Charge</AssignToObject></DataExtDefRet>";

            var dataExtDefQueryResponse = new DataExtDefQueryResponse();
            var response = dataExtDefQueryResponse.ParseResponse(QuickBooksTestHelper.CreateQbXmlWithEnvelope(ret, "DataExtDefQueryRs"));
            var dataDefExtList = response.Object;

            Assert.AreEqual(1, dataDefExtList.Count());
            QBAssert.AreEqual("name", dataDefExtList[0].DataExtName);
            Assert.AreEqual(DataExtType.STR255TYPE, dataDefExtList[0].DataExtType);
            Assert.AreEqual(2, dataDefExtList[0].AssignToObject.Count());
            Assert.AreEqual(AssignToObject.Account, dataDefExtList[0].AssignToObject.First());
            Assert.AreEqual(AssignToObject.Charge, dataDefExtList[0].AssignToObject.Last());
        }
    }
}