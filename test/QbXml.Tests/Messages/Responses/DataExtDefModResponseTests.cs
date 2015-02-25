using NUnit.Framework;
using QbSync.QbXml.Objects;
using QbSync.QbXml.Tests.Helpers;
using System.Linq;

namespace QbSync.QbXml.Tests.QbXml
{
    [TestFixture]
    class DataExtDefModResponseTests
    {
        [Test]
        public void BasicDataExtDefModResponseTest()
        {
            var ret = "<DataExtDefRet><OwnerID>{7d543f23-f3b1-4dea-8ff4-37bd26d15e6c}</OwnerID><DataExtID>123</DataExtID><DataExtName>name</DataExtName><DataExtType>STR255TYPE</DataExtType><AssignToObject>Account</AssignToObject><AssignToObject>Charge</AssignToObject></DataExtDefRet>";

            var response = new QbXmlResponse();
            var rs = response.GetSingleItemFromResponse<DataExtDefModRsType>(QuickBooksTestHelper.CreateQbXmlWithEnvelope(ret, "DataExtDefModRs"));
            var dataExtDef = rs.DataExtDefRet;

            Assert.AreEqual("name", dataExtDef.DataExtName);
            Assert.AreEqual(DataExtType.STR255TYPE, dataExtDef.DataExtType);
            Assert.AreEqual(2, dataExtDef.AssignToObject.Count());
            Assert.AreEqual(AssignToObject.Account, dataExtDef.AssignToObject.First());
            Assert.AreEqual(AssignToObject.Charge, dataExtDef.AssignToObject.Last());
        }
    }
}