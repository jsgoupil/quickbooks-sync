using Moq;
using NUnit.Framework;
using QbSync.QbXml.Objects;
using QbSync.WebConnector.Core;
using QbSync.WebConnector.Impl;
using QbSync.WebConnector.Tests.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QbSync.WebConnector.Tests.Impl
{
    [TestFixture]
    class GroupStepQueryResponseBaseTests
    {
        private string customerAddRs = @"<CustomerAddRs requestID=""1"" statusCode=""0"" statusSeverity=""Info"" statusMessage=""Status OK"">" +
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
                "</CustomerAddRs>";
        private string customerQueryRs = @"<CustomerQueryRs requestID=""1"" statusCode=""0"" statusSeverity=""Info"" statusMessage=""Status OK"">" +
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
                "</CustomerQueryRs>";

        private string customerResponseError = @"<CustomerAddRs statusCode=""3090"" statusSeverity=""Error"" statusMessage=""There was an error when storing &quot;Incorrect : Name&quot; in the &quot;customer name&quot; field.QuickBooks error message: Names in this list cannot contain a colon.The colon is a special character used to indicate a parent/child relationship."" />";
        private string customerResponseAborted = @"<CustomerQueryRs statusCode=""3231"" statusSeverity=""Error"" statusMessage=""The request has not been processed."" />";

        [Test]
        public async Task GroupStepQueryResponseBaseTests_DoubleResponse()
        {
            var authenticatedTicket = new AuthenticatedTicket
            {
                Ticket = Guid.NewGuid().ToString(),
                CurrentStep = "step4"
            };

            var xml = $@"<?xml version=""1.0"" ?><QBXML><QBXMLMsgsRs>{customerAddRs}{customerQueryRs}</QBXMLMsgsRs></QBXML>";

            var groupStepQueryResponseBaseMock = new Mock<GroupStepQueryResponseBase>();
            groupStepQueryResponseBaseMock
                .Setup(m => m.ExecuteResponseAsync(It.Is<IAuthenticatedTicket>(n => n == authenticatedTicket), It.IsAny<IEnumerable<IQbResponse>>()))
                .Returns(Task.CompletedTask)
                .Callback<IAuthenticatedTicket, IEnumerable<IQbResponse>>((ticket, responses) =>
                {
                    Assert.AreEqual(2, responses.Count());
                    Assert.IsInstanceOf(typeof(CustomerAddRsType), responses.First());
                    Assert.IsInstanceOf(typeof(CustomerQueryRsType), responses.Skip(1).First());
                });
            groupStepQueryResponseBaseMock.CallBase = true;

            var ret = await groupStepQueryResponseBaseMock.Object.ReceiveXMLAsync(authenticatedTicket, xml, string.Empty, string.Empty);
            Assert.AreEqual(0, ret);
        }

        [Test]
        public async Task GroupStepQueryResponseBaseTests_RequestedAborted()
        {
            var authenticatedTicket = new AuthenticatedTicket
            {
                Ticket = Guid.NewGuid().ToString(),
                CurrentStep = "step4"
            };

            var xml = $@"<?xml version=""1.0"" ?><QBXML><QBXMLMsgsRs>{customerResponseError}{customerResponseAborted}</QBXMLMsgsRs></QBXML>";

            var groupStepQueryResponseBaseMock = new Mock<GroupStepQueryResponseBase>();
            groupStepQueryResponseBaseMock
                .Setup(m => m.ExecuteResponseAsync(It.Is<IAuthenticatedTicket>(n => n == authenticatedTicket), It.IsAny<IEnumerable<IQbResponse>>()))
                .Returns(Task.CompletedTask)
                .Callback<IAuthenticatedTicket, IEnumerable<IQbResponse>>((ticket, responses) =>
                {
                    Assert.AreEqual(2, responses.Count());
                    var firstResponse = responses.First();
                    Assert.IsInstanceOf(typeof(CustomerAddRsType), firstResponse);
                    Assert.AreEqual("3090", firstResponse.statusCode);

                    var secondResponse = responses.Skip(1).First();
                    Assert.IsInstanceOf(typeof(CustomerQueryRsType), secondResponse);
                    Assert.AreEqual("3231", secondResponse.statusCode);
                });
            groupStepQueryResponseBaseMock.CallBase = true;

            var ret = await groupStepQueryResponseBaseMock.Object.ReceiveXMLAsync(authenticatedTicket, xml, string.Empty, string.Empty);
            Assert.AreEqual(0, ret);
        }
    }
}
