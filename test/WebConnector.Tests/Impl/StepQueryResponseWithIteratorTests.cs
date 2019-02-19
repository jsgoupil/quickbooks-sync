using Moq;
using Moq.Protected;
using NUnit.Framework;
using QbSync.QbXml;
using QbSync.QbXml.Objects;
using QbSync.WebConnector.Impl;
using QbSync.WebConnector.Tests.Helpers;
using QbSync.WebConnector.Tests.Models;
using System;
using System.Threading.Tasks;

namespace QbSync.WebConnector.Tests.Impl
{
    [TestFixture]
    class StepQueryResponseWithIteratorTests
    {
        [Test]
        public async Task StepQueryResponseWithIterator_CustomerQueryWithResponseWithNoIterators()
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

            var stepQueryResponseWithIteratorMock = new Mock<StepQueryResponseWithIterator<CustomerQueryRsType>>();
            stepQueryResponseWithIteratorMock.CallBase = true;

            var ret = await stepQueryResponseWithIteratorMock.Object.ReceiveXMLAsync(authenticatedTicket, xml, string.Empty, string.Empty);
            Assert.AreEqual(0, ret);
        }

        [Test]
        public async Task StepQueryResponseWithIterator_CustomerQueryWithResponseWithIterators()
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

            var stepQueryResponseWithIteratorMock = new Mock<StepQueryResponseWithIterator<CustomerQueryRsType>>();
            stepQueryResponseWithIteratorMock.CallBase = true;

            var ret = await stepQueryResponseWithIteratorMock.Object.ReceiveXMLAsync(authenticatedTicket, xml, string.Empty, string.Empty);
            Assert.AreEqual(0, ret);
        }

        [Test]
        public async Task StepQueryResponseWithIterator_CustomerQueryWithTimeZoneFix()
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

            var stepQueryResponseWithIteratorMock = new Mock<StepQueryResponseBase<CustomerQueryRsType>>();
            stepQueryResponseWithIteratorMock
                .Protected()
                .Setup<Task>("ExecuteResponseAsync", ItExpr.IsAny<AuthenticatedTicket>(), ItExpr.IsAny<CustomerQueryRsType>())
                .Returns(Task.FromResult<object>(null));
            stepQueryResponseWithIteratorMock.CallBase = true;
            await stepQueryResponseWithIteratorMock.Object.SetOptionsAsync(qbXmlResponseOptions);

            var ret = await stepQueryResponseWithIteratorMock.Object.ReceiveXMLAsync(authenticatedTicket, xml, string.Empty, string.Empty);
            Assert.AreEqual(0, ret);

            // The time zone fix should leave the time as is, and correct the offset according to the provided zone
            var expectedHour = 10;
            var expectedOffset = TimeSpan.FromHours(-7);

            stepQueryResponseWithIteratorMock
                .Protected()
                .Verify("ExecuteResponseAsync", Times.Once(), ItExpr.IsAny<AuthenticatedTicket>(), ItExpr.Is<CustomerQueryRsType>(m => m.CustomerRet[0].TimeModified.GetDateTimeOffset().Hour == expectedHour && m.CustomerRet[0].TimeModified.GetDateTimeOffset().Offset == expectedOffset));
        }

        [Test]
        public async Task StepQueryResponseWithIterator_CustomerQueryWithTimeZoneNoFix()
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

            var stepQueryResponseWithIteratorMock = new Mock<StepQueryResponseBase<CustomerQueryRsType>>();
            stepQueryResponseWithIteratorMock
                .Protected()
                .Setup<Task>("ExecuteResponseAsync", ItExpr.IsAny<AuthenticatedTicket>(), ItExpr.IsAny<CustomerQueryRsType>())
                .Returns(Task.FromResult<object>(null));
            stepQueryResponseWithIteratorMock.CallBase = true;
            await stepQueryResponseWithIteratorMock.Object.SetOptionsAsync(qbXmlResponseOptions);

            var ret = await stepQueryResponseWithIteratorMock.Object.ReceiveXMLAsync(authenticatedTicket, xml, string.Empty, string.Empty);
            Assert.AreEqual(0, ret);


            // When there is no time zone fix, the time should be left alone, and the offset should be ignored

            var expectedHour = 10;
            stepQueryResponseWithIteratorMock
                .Protected()
                .Verify("ExecuteResponseAsync", Times.Once(), ItExpr.IsAny<AuthenticatedTicket>(), ItExpr.Is<CustomerQueryRsType>(m => m.CustomerRet[0].TimeModified.ToDateTime().Hour == expectedHour));

            stepQueryResponseWithIteratorMock
                .Protected()
                .Verify("ExecuteResponseAsync", Times.Once(), ItExpr.IsAny<AuthenticatedTicket>(), ItExpr.Is<CustomerQueryRsType>(m => m.CustomerRet[0].TimeModified.HasOffset() == false));
        }
    }
}
