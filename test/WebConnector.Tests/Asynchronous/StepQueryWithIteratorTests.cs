using Moq;
using Moq.Protected;
using NUnit.Framework;
using QbSync.QbXml;
using QbSync.QbXml.Objects;
using QbSync.WebConnector.Asynchronous.Messages;
using QbSync.WebConnector.Tests.Helpers;
using QbSync.WebConnector.Tests.Asynchronous.Helpers;
using System;
using System.Reflection;
using System.Xml;
using System.Threading.Tasks;

namespace QbSync.WebConnector.Asynchronous.Tests
{
    [TestFixture]
    class StepQueryWithIteratorTests
    {
        [Test]
        public async Task CustomerQueryWithNoInitialMessage()
        {
            var defaultMaxResult = 100;
            var authenticatedTicket = new AuthenticatedTicket
            {
                Ticket = Guid.NewGuid().ToString(),
                CurrentStep = "step4"
            };

            var stepQueryWithIteratorMock = new Mock<StepQueryWithIterator<CustomerQueryRqType, CustomerQueryRsType>>();
            stepQueryWithIteratorMock.CallBase = true;

            var xml = await stepQueryWithIteratorMock.Object.SendXMLAsync(authenticatedTicket);

            XmlDocument requestXmlDoc = new XmlDocument();
            requestXmlDoc.LoadXml(xml);

            var node = requestXmlDoc.SelectSingleNode("//CustomerQueryRq");

            Assert.IsNotNull(node);
            Assert.AreEqual("Start", node.Attributes.GetNamedItem("iterator").Value);
            Assert.IsNotNull(node.SelectSingleNode("MaxReturned"));
            Assert.AreEqual(defaultMaxResult.ToString(), node.SelectSingleNode("MaxReturned").InnerText);
        }

        [Test]
        public async Task CustomerQueryWithPreviousIterator()
        {
            var authenticatedTicket = new AuthenticatedTicket
            {
                Ticket = Guid.NewGuid().ToString(),
                CurrentStep = "step4"
            };
            var iteratorID = "123456";
            var iteratorKey = StepQueryWithIterator<CustomerQueryRqType, CustomerQueryRsType>.IteratorKey;
            var updateCustomerMock = new Mock<StepQueryWithIterator<CustomerQueryRqType, CustomerQueryRsType>>();
            updateCustomerMock
                .Protected()
                .Setup<Task<string>>("RetrieveMessageAsync", ItExpr.Is<AuthenticatedTicket>(s => s == authenticatedTicket), ItExpr.Is<string>(s => s == iteratorKey))
                .Returns(Task.FromResult(iteratorID));
            updateCustomerMock.CallBase = true;

            var xml = await updateCustomerMock.Object.SendXMLAsync(authenticatedTicket);

            XmlDocument requestXmlDoc = new XmlDocument();
            requestXmlDoc.LoadXml(xml);

            var node = requestXmlDoc.SelectSingleNode("//CustomerQueryRq");

            Assert.IsNotNull(node);
            Assert.AreEqual("Continue", node.Attributes.GetNamedItem("iterator").Value);
            Assert.AreEqual(iteratorID, node.Attributes.GetNamedItem("iteratorID").Value);
        }

        [Test]
        public async Task CustomerQueryWithShortCircuitExecuteRequest()
        {
            var authenticatedTicket = new AuthenticatedTicket
            {
                Ticket = Guid.NewGuid().ToString(),
                CurrentStep = "step4"
            };
            var updateCustomerMock = new Mock<StepQueryWithIterator<CustomerQueryRqType, CustomerQueryRsType>>();
            updateCustomerMock
                .Protected()
                .Setup<Task<bool>>("ExecuteRequestAsync", ItExpr.IsAny<AuthenticatedTicket>(), ItExpr.IsAny<CustomerQueryRqType>())
                .Returns(Task.FromResult(false));
            updateCustomerMock.CallBase = true;

            var xml = await updateCustomerMock.Object.SendXMLAsync(authenticatedTicket);

            Assert.IsNull(xml);
        }

        [Test]
        public async Task CustomerQueryWithResponseWithNoIterators()
        {
            var authenticatedTicket = new AuthenticatedTicket
            {
                Ticket = Guid.NewGuid().ToString(),
                CurrentStep = "step4"
            };
            var xml = @"<?xml version=""1.0"" ?><QBXML><QBXMLMsgsRs><CustomerQueryRs requestID=""1"" statusCode=""0"" statusSeverity=""Info"" statusMessage=""Status OK"">" +
                "<CustomerRet>" +
                    "<ListID>110000-1232697602</ListID>" +
                    "<TimeCreated>2009-01-23T03:00:02-05:00</TimeCreated>" +
                    "<TimeModified>2009-01-23T03:00:02-05:00</TimeModified>" +
                    "<EditSequence>1232697602</EditSequence>" +
                    "<Name>10th Customer</Name>" +
                    "<FullName>ABC Customer</FullName>" +
                    "<IsActive>true</IsActive>" +
                    "<Sublevel>0</Sublevel>" +
                    "<Balance>0.00</Balance>" +
                    "<TotalBalance>0.00</TotalBalance>" +
                    "<JobStatus>None</JobStatus>" +
                "</CustomerRet>" +
                "</CustomerQueryRs></QBXMLMsgsRs></QBXML>";

            var stepQueryWithIteratorMock = new Mock<StepQueryWithIterator<CustomerQueryRqType, CustomerQueryRsType>>();
            stepQueryWithIteratorMock.CallBase = true;

            var ret = await stepQueryWithIteratorMock.Object.ReceiveXMLAsync(authenticatedTicket, xml, string.Empty, string.Empty);
            Assert.AreEqual(0, ret);
        }

        [Test]
        public async Task CustomerQueryWithResponseWithIterators()
        {
            var authenticatedTicket = new AuthenticatedTicket
            {
                Ticket = Guid.NewGuid().ToString(),
                CurrentStep = "step4"
            };
            var iteratorID = "{eb05f701-e727-472f-8ade-6753c4f67a46}";
            var xml = @"<?xml version=""1.0"" ?><QBXML><QBXMLMsgsRs><CustomerQueryRs requestID=""1"" statusCode=""0"" statusSeverity=""Info"" statusMessage=""Status OK"" iteratorRemainingCount=""18"" iteratorID=""" + iteratorID + @""">" +
                "<CustomerRet>" +
                    "<ListID>110000-1232697602</ListID>" +
                    "<TimeCreated>2009-01-23T03:00:02-05:00</TimeCreated>" +
                    "<TimeModified>2009-01-23T03:00:02-05:00</TimeModified>" +
                    "<EditSequence>1232697602</EditSequence>" +
                    "<Name>10th Customer</Name>" +
                    "<FullName>ABC Customer</FullName>" +
                    "<IsActive>true</IsActive>" +
                    "<Sublevel>0</Sublevel>" +
                    "<Balance>0.00</Balance>" +
                    "<TotalBalance>0.00</TotalBalance>" +
                    "<JobStatus>None</JobStatus>" +
                "</CustomerRet>" +
                "</CustomerQueryRs></QBXMLMsgsRs></QBXML>";

            var stepQueryWithIteratorMock = new Mock<StepQueryWithIterator<CustomerQueryRqType, CustomerQueryRsType>>();
            stepQueryWithIteratorMock.CallBase = true;

            var ret = await stepQueryWithIteratorMock.Object.ReceiveXMLAsync(authenticatedTicket, xml, string.Empty, string.Empty);
            Assert.AreEqual(0, ret);
        }

        [Test]
        public async Task CustomerQueryWithTimeZoneFix()
        {
            var authenticatedTicket = new AuthenticatedTicket
            {
                Ticket = Guid.NewGuid().ToString(),
                CurrentStep = "step4"
            };
            var xml = @"<?xml version=""1.0"" ?><QBXML><QBXMLMsgsRs><CustomerQueryRs requestID=""1"" statusCode=""0"" statusSeverity=""Info"" statusMessage=""Status OK"">" +
                "<CustomerRet>" +
                    "<ListID>110000-1232697602</ListID>" +
                    "<TimeCreated>2015-04-03T10:06:17-08:00</TimeCreated>" +
                    "<TimeModified>2015-04-03T10:06:17-08:00</TimeModified>" +
                    "<EditSequence>1232697602</EditSequence>" +
                    "<Name>10th Customer</Name>" +
                    "<FullName>ABC Customer</FullName>" +
                    "<IsActive>true</IsActive>" +
                    "<Sublevel>0</Sublevel>" +
                    "<Balance>0.00</Balance>" +
                    "<TotalBalance>0.00</TotalBalance>" +
                    "<JobStatus>None</JobStatus>" +
                "</CustomerRet>" +
                "</CustomerQueryRs></QBXMLMsgsRs></QBXML>";

            var qbXmlResponseOptions = new QbXmlResponseOptions
            {
                TimeZoneBugFix = QuickBooksTestHelper.GetPacificStandardTimeZoneInfo()
            };

            var stepQueryWithIteratorMock = new Mock<StepQueryResponseBase<CustomerQueryRqType, CustomerQueryRsType>>();
            stepQueryWithIteratorMock
                .Protected()
                .Setup<Task>("ExecuteResponseAsync", ItExpr.IsAny<AuthenticatedTicket>(), ItExpr.IsAny<CustomerQueryRsType>())
                .Returns(Task.FromResult<object>(null));
            stepQueryWithIteratorMock.CallBase = true;
            await stepQueryWithIteratorMock.Object.SetOptionsAsync(qbXmlResponseOptions);

            var ret = await stepQueryWithIteratorMock.Object.ReceiveXMLAsync(authenticatedTicket, xml, string.Empty, string.Empty);
            Assert.AreEqual(0, ret);
            var expectedHour = 17;
            stepQueryWithIteratorMock
                .Protected()
                .Verify("ExecuteResponseAsync", Times.Once(), ItExpr.IsAny<AuthenticatedTicket>(), ItExpr.Is<CustomerQueryRsType>(m => m.CustomerRet[0].TimeModified.ToDateTime().ToUniversalTime().Hour == expectedHour));
        }

        [Test]
        public async Task CustomerQueryWithTimeZoneNoFix()
        {
            var authenticatedTicket = new AuthenticatedTicket
            {
                Ticket = Guid.NewGuid().ToString(),
                CurrentStep = "step4"
            };
            var xml = @"<?xml version=""1.0"" ?><QBXML><QBXMLMsgsRs><CustomerQueryRs requestID=""1"" statusCode=""0"" statusSeverity=""Info"" statusMessage=""Status OK"">" +
                "<CustomerRet>" +
                    "<ListID>110000-1232697602</ListID>" +
                    "<TimeCreated>2015-04-03T10:06:17-08:00</TimeCreated>" +
                    "<TimeModified>2015-04-03T10:06:17-08:00</TimeModified>" +
                    "<EditSequence>1232697602</EditSequence>" +
                    "<Name>10th Customer</Name>" +
                    "<FullName>ABC Customer</FullName>" +
                    "<IsActive>true</IsActive>" +
                    "<Sublevel>0</Sublevel>" +
                    "<Balance>0.00</Balance>" +
                    "<TotalBalance>0.00</TotalBalance>" +
                    "<JobStatus>None</JobStatus>" +
                "</CustomerRet>" +
                "</CustomerQueryRs></QBXMLMsgsRs></QBXML>";

            var qbXmlResponseOptions = new QbXmlResponseOptions
            {
            };

            var stepQueryWithIteratorMock = new Mock<StepQueryResponseBase<CustomerQueryRqType, CustomerQueryRsType>>();
            stepQueryWithIteratorMock
                .Protected()
                .Setup<Task>("ExecuteResponseAsync", ItExpr.IsAny<AuthenticatedTicket>(), ItExpr.IsAny<CustomerQueryRsType>())
                .Returns(Task.FromResult<object>(null));
            stepQueryWithIteratorMock.CallBase = true;
            await stepQueryWithIteratorMock.Object.SetOptionsAsync(qbXmlResponseOptions);

            var ret = await stepQueryWithIteratorMock.Object.ReceiveXMLAsync(authenticatedTicket, xml, string.Empty, string.Empty);
            Assert.AreEqual(0, ret);
            var expectedHour = 18;
            stepQueryWithIteratorMock
                .Protected()
                .Verify("ExecuteResponseAsync", Times.Once(), ItExpr.IsAny<AuthenticatedTicket>(), ItExpr.Is<CustomerQueryRsType>(m => m.CustomerRet[0].TimeModified.ToDateTime().ToUniversalTime().Hour == expectedHour));
        }

        [Test]
        public async Task CustomerQueryWithIteratorInvalidMaxReturned()
        {
            const int expectedMaxResult = 100;

            var defaultMaxResult = "INVALID_MAX_RETURN_VALUE";
            var authenticatedTicket = new AuthenticatedTicket
            {
                Ticket = Guid.NewGuid().ToString(),
                CurrentStep = "step4"
            };

            var stepQueryWithIteratorMock = new Mock<StepQueryWithIteratorMaxReturnedHarness>(defaultMaxResult);
            stepQueryWithIteratorMock.CallBase = true;

            var xml = await stepQueryWithIteratorMock.Object.SendXMLAsync(authenticatedTicket);

            XmlDocument requestXmlDoc = new XmlDocument();
            requestXmlDoc.LoadXml(xml);

            var node = requestXmlDoc.SelectSingleNode("//CustomerQueryRq");

            Assert.IsNotNull(node);
            Assert.AreEqual("Start", node.Attributes.GetNamedItem("iterator").Value);
            Assert.IsNotNull(node.SelectSingleNode("MaxReturned"));
            Assert.AreEqual(expectedMaxResult.ToString(), node.SelectSingleNode("MaxReturned").InnerText);
        }

        [Test]
        public async Task CustomerQueryWithIteratorZeroMaxReturned()
        {
            const int expectedMaxResult = 100;

            var defaultMaxResult = "0";
            var authenticatedTicket = new AuthenticatedTicket
            {
                Ticket = Guid.NewGuid().ToString(),
                CurrentStep = "step4"
            };

            var stepQueryWithIteratorMock = new Mock<StepQueryWithIteratorMaxReturnedHarness>(defaultMaxResult);
            stepQueryWithIteratorMock.CallBase = true;

            var xml = await stepQueryWithIteratorMock.Object.SendXMLAsync(authenticatedTicket);

            XmlDocument requestXmlDoc = new XmlDocument();
            requestXmlDoc.LoadXml(xml);

            var node = requestXmlDoc.SelectSingleNode("//CustomerQueryRq");

            Assert.IsNotNull(node);
            Assert.AreEqual("Start", node.Attributes.GetNamedItem("iterator").Value);
            Assert.IsNotNull(node.SelectSingleNode("MaxReturned"));
            Assert.AreEqual(expectedMaxResult.ToString(), node.SelectSingleNode("MaxReturned").InnerText);
        }

        [Test]
        public async Task CustomerQueryWithIteratorNegativeMaxReturned()
        {
            const int expectedMaxResult = 100;

            var defaultMaxResult = "-1";
            var authenticatedTicket = new AuthenticatedTicket
            {
                Ticket = Guid.NewGuid().ToString(),
                CurrentStep = "step4"
            };

            var stepQueryWithIteratorMock = new Mock<StepQueryWithIteratorMaxReturnedHarness>(defaultMaxResult);
            stepQueryWithIteratorMock.CallBase = true;

            var xml = await stepQueryWithIteratorMock.Object.SendXMLAsync(authenticatedTicket);

            XmlDocument requestXmlDoc = new XmlDocument();
            requestXmlDoc.LoadXml(xml);

            var node = requestXmlDoc.SelectSingleNode("//CustomerQueryRq");

            Assert.IsNotNull(node);
            Assert.AreEqual("Start", node.Attributes.GetNamedItem("iterator").Value);
            Assert.IsNotNull(node.SelectSingleNode("MaxReturned"));
            Assert.AreEqual(expectedMaxResult.ToString(), node.SelectSingleNode("MaxReturned").InnerText);
        }

        [Test]
        public async Task CustomerQueryWithIteratorValidMaxReturned()
        {
            string defaultMaxResult = "500";
            var authenticatedTicket = new AuthenticatedTicket
            {
                Ticket = Guid.NewGuid().ToString(),
                CurrentStep = "step4"
            };

            var stepQueryWithIteratorMock = new Mock<StepQueryWithIteratorMaxReturnedHarness>(defaultMaxResult);
            stepQueryWithIteratorMock.CallBase = true;

            var xml = await stepQueryWithIteratorMock.Object.SendXMLAsync(authenticatedTicket);

            XmlDocument requestXmlDoc = new XmlDocument();
            requestXmlDoc.LoadXml(xml);

            var node = requestXmlDoc.SelectSingleNode("//CustomerQueryRq");

            Assert.IsNotNull(node);
            Assert.AreEqual("Start", node.Attributes.GetNamedItem("iterator").Value);
            Assert.IsNotNull(node.SelectSingleNode("MaxReturned"));
            Assert.AreEqual(defaultMaxResult.ToString(), node.SelectSingleNode("MaxReturned").InnerText);
        }
    }
}
