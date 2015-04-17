using NUnit.Framework;
using QbSync.QbXml.Objects;
using QbSync.QbXml.Tests.Helpers;
using System;
using System.Linq;

namespace QbSync.QbXml.Tests.QbXml
{
    [TestFixture]
    class CustomerQueryResponseTests
    {
        [Test]
        public void BasicCustomerResponseTest()
        {
            var ret = "<CustomerRet><ListID>80000001-1422671082</ListID><TimeCreated>2015-01-30T18:24:42-08:00</TimeCreated><TimeModified>2015-01-30T18:24:42-08:00</TimeModified><EditSequence>1422671082</EditSequence><Name>Jean-S&#233;bastien Goupil</Name><FullName>Jean-S&#233;bastien Goupil</FullName><IsActive>true</IsActive></CustomerRet>";

            var response = new QbXmlResponse();
            var rs = response.GetSingleItemFromResponse<CustomerQueryRsType>(QuickBooksTestHelper.CreateQbXmlWithEnvelope(ret, "CustomerQueryRs"));
            var customers = rs.CustomerRet;
            var customer = customers[0];

            Assert.AreEqual(1, customers.Length);
            Assert.AreEqual("80000001-1422671082", customer.ListID);
            Assert.AreEqual("Jean-Sébastien Goupil", customer.Name);
        }

        [Test]
        public void BasicCustomerResponseTest_WithObject()
        {
            var ret = "<CustomerRet><ShipAddress><City>Seattle</City><State>WA</State></ShipAddress></CustomerRet>";

            var response = new QbXmlResponse();
            var rs = response.GetSingleItemFromResponse<CustomerQueryRsType>(QuickBooksTestHelper.CreateQbXmlWithEnvelope(ret, "CustomerQueryRs"));
            var customers = rs.CustomerRet;
            var customer = customers[0];

            Assert.AreEqual(1, customers.Length);
            Assert.IsNotNull(customer.ShipAddress);
            Assert.AreEqual("Seattle", customer.ShipAddress.City);
            Assert.AreEqual("WA", customer.ShipAddress.State);
        }

        [Test]
        public void BasicCustomerResponseTest_WithIEnumerable()
        {
            var ret = "<CustomerRet><AdditionalContactRef><ContactName>Name1</ContactName><ContactValue>Value1</ContactValue></AdditionalContactRef><AdditionalContactRef><ContactName>Name2</ContactName><ContactValue>Value2</ContactValue></AdditionalContactRef></CustomerRet>";

            var response = new QbXmlResponse();
            var rs = response.GetSingleItemFromResponse<CustomerQueryRsType>(QuickBooksTestHelper.CreateQbXmlWithEnvelope(ret, "CustomerQueryRs"));
            var customers = rs.CustomerRet;
            var customer = customers[0];

            Assert.AreEqual(1, customers.Length);
            Assert.AreEqual(2, customer.AdditionalContactRef.Count());
            Assert.AreEqual("Name1", customer.AdditionalContactRef.First().ContactName);
            Assert.AreEqual("Value1", customer.AdditionalContactRef.First().ContactValue);
            Assert.AreEqual("Name2", customer.AdditionalContactRef.Last().ContactName);
            Assert.AreEqual("Value2", customer.AdditionalContactRef.Last().ContactValue);
        }

        [Test]
        public void BasicCustomerResponseTest_WithEnum()
        {
            var ret = "<CustomerRet><JobStatus>InProgress</JobStatus></CustomerRet>";

            var response = new QbXmlResponse();
            var rs = response.GetSingleItemFromResponse<CustomerQueryRsType>(QuickBooksTestHelper.CreateQbXmlWithEnvelope(ret, "CustomerQueryRs"));
            var customers = rs.CustomerRet;
            var customer = customers[0];

            Assert.AreEqual(1, customers.Length);
            Assert.AreEqual(JobStatus.InProgress, customer.JobStatus);
        }

        [Test]
        public void TimeZoneBugFixTests()
        {
            var ret = "<CustomerRet><ListID>80000001-1422671082</ListID><TimeCreated>2015-04-03T10:06:17-08:00</TimeCreated><TimeModified>2015-04-03T10:06:17-08:00</TimeModified><EditSequence>1422671082</EditSequence><Name>Jean-S&#233;bastien Goupil</Name><FullName>Jean-S&#233;bastien Goupil</FullName><IsActive>true</IsActive></CustomerRet>";

            var response = new QbXmlResponse(new QbXmlResponseOptions
            {
                TimeZoneBugFix = QuickBooksTestHelper.GetPacificStandardTimeZoneInfo()
            });
            var rs = response.GetSingleItemFromResponse<CustomerQueryRsType>(QuickBooksTestHelper.CreateQbXmlWithEnvelope(ret, "CustomerQueryRs"));
            var customers = rs.CustomerRet;
            var customer = customers[0];

            Assert.AreEqual(17, customer.TimeModified.ToDateTime().ToUniversalTime().Hour);
        }

        [Test]
        public void PreferredDeliveryMethod_Mail_ShouldBeValid()
        {
            var ret = "<CustomerRet><ListID>80000001-1422671082</ListID><TimeCreated>2015-04-03T10:06:17-08:00</TimeCreated><TimeModified>2015-04-03T10:06:17-08:00</TimeModified><EditSequence>1422671082</EditSequence><Name>Jean-S&#233;bastien Goupil</Name><FullName>Jean-S&#233;bastien Goupil</FullName><IsActive>true</IsActive><PreferredDeliveryMethod>Mail</PreferredDeliveryMethod></CustomerRet>";

            var response = new QbXmlResponse();
            var rs = response.GetSingleItemFromResponse<CustomerQueryRsType>(QuickBooksTestHelper.CreateQbXmlWithEnvelope(ret, "CustomerQueryRs"));
            var customers = rs.CustomerRet;
            var customer = customers[0];

            Assert.AreEqual(1, customers.Length);
            Assert.AreEqual(PreferredDeliveryMethod.Mail, customer.PreferredDeliveryMethod);
        }
    }
}