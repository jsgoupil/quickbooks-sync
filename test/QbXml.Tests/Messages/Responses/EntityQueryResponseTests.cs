using NUnit.Framework;
using QbSync.QbXml.Objects;
using QbSync.QbXml.Tests.Helpers;
using System.Linq;

namespace QbSync.QbXml.Tests.QbXml
{
    [TestFixture]
    class EntityQueryResponseTests
    {
        [Test]
        public void BasicEntityQueryResponseTest()
        {
            var ret = "<EmployeeRet><Name>SomeName</Name></EmployeeRet><EmployeeRet><Name>Other</Name></EmployeeRet>";

            var response = new QbXmlResponse();
            var rs = response.GetSingleItemFromResponse<EntityQueryRsType>(QuickBooksTestHelper.CreateQbXmlWithEnvelope(ret, "EntityQueryRs"));
            var employees = rs.EmployeeRet;

            Assert.AreEqual(2, employees.Count());
            Assert.AreEqual("SomeName", employees.First().Name);
            Assert.AreEqual("Other", employees.Last().Name);
        }
    }
}