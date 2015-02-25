using NUnit.Framework;
using QbSync.QbXml.Objects;
using QbSync.QbXml.Tests.Helpers;
using System.Linq;

namespace QbSync.QbXml.Tests.QbXml
{
    [TestFixture]
    class DataExtDefQueryResponseTests
    {
        [Test]
        public void BasicDataExtDefQueryResponseTest()
        {
            var ret = "<DataExtDefRet><OwnerID>{7d543f23-f3b1-4dea-8ff4-37bd26d15e6c}</OwnerID><DataExtID>123</DataExtID><DataExtName>name</DataExtName><DataExtType>STR255TYPE</DataExtType><AssignToObject>Account</AssignToObject><AssignToObject>Charge</AssignToObject></DataExtDefRet>";

            var response = new QbXmlResponse();
            var rs = response.GetSingleItemFromResponse<DataExtDefQueryRsType>(QuickBooksTestHelper.CreateQbXmlWithEnvelope(ret, "DataExtDefQueryRs"));
            var dataExtDefList = rs.DataExtDefRet;

            Assert.AreEqual(1, dataExtDefList.Count());
            Assert.AreEqual("name", dataExtDefList[0].DataExtName);
            Assert.AreEqual(DataExtType.STR255TYPE, dataExtDefList[0].DataExtType);
            Assert.AreEqual(2, dataExtDefList[0].AssignToObject.Count());
            Assert.AreEqual(AssignToObject.Account, dataExtDefList[0].AssignToObject.First());
            Assert.AreEqual(AssignToObject.Charge, dataExtDefList[0].AssignToObject.Last());
        }
    }
}