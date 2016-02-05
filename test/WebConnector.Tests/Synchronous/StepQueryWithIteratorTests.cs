using Moq;
using Moq.Protected;
using NUnit.Framework;
using QbSync.QbXml;
using QbSync.QbXml.Objects;
using QbSync.WebConnector.Synchronous.Messages;
using QbSync.WebConnector.Tests.Helpers;
using QbSync.WebConnector.Tests.Synchronous.Helpers;
using System;
using System.Reflection;
using System.Xml;

namespace QbSync.WebConnector.Synchronous.Tests
{
    [TestFixture]
    class StepQueryWithIteratorTests
    {
        [Test]
        public void CustomerQueryWithNoInitialMessage()
        {
            var defaultMaxResult = 100;
            var authenticatedTicket = new AuthenticatedTicket
            {
                Ticket = Guid.NewGuid().ToString(),
                CurrentStep = "step4"
            };

            var stepQueryWithIteratorMock = new Mock<StepQueryWithIterator<CustomerQueryRqType, CustomerQueryRsType>>();
            stepQueryWithIteratorMock.CallBase = true;

            var xml = stepQueryWithIteratorMock.Object.SendXML(authenticatedTicket);

            XmlDocument requestXmlDoc = new XmlDocument();
            requestXmlDoc.LoadXml(xml);

            var node = requestXmlDoc.SelectSingleNode("//CustomerQueryRq");

            Assert.IsNotNull(node);
            Assert.AreEqual("Start", node.Attributes.GetNamedItem("iterator").Value);
            Assert.IsNotNull(node.SelectSingleNode("MaxReturned"));
            Assert.AreEqual(defaultMaxResult.ToString(), node.SelectSingleNode("MaxReturned").InnerText);
        }

        [Test]
        public void CustomerQueryWithPreviousIterator()
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
                .Setup<string>("RetrieveMessage", ItExpr.Is<AuthenticatedTicket>(s => s == authenticatedTicket), ItExpr.Is<string>(s => s == iteratorKey))
                .Returns(iteratorID);
            updateCustomerMock.CallBase = true;

            var xml = updateCustomerMock.Object.SendXML(authenticatedTicket);

            XmlDocument requestXmlDoc = new XmlDocument();
            requestXmlDoc.LoadXml(xml);

            var node = requestXmlDoc.SelectSingleNode("//CustomerQueryRq");

            Assert.IsNotNull(node);
            Assert.AreEqual("Continue", node.Attributes.GetNamedItem("iterator").Value);
            Assert.AreEqual(iteratorID, node.Attributes.GetNamedItem("iteratorID").Value);
        }

        [Test]
        public void CustomerQueryWithShortCircuitExecuteRequest()
        {
            var authenticatedTicket = new AuthenticatedTicket
            {
                Ticket = Guid.NewGuid().ToString(),
                CurrentStep = "step4"
            };
            var updateCustomerMock = new Mock<StepQueryWithIterator<CustomerQueryRqType, CustomerQueryRsType>>();
            updateCustomerMock
                .Protected()
                .Setup<bool>("ExecuteRequest", ItExpr.IsAny<AuthenticatedTicket>(), ItExpr.IsAny<CustomerQueryRqType>())
                .Returns(false);
            updateCustomerMock.CallBase = true;

            var xml = updateCustomerMock.Object.SendXML(authenticatedTicket);

            Assert.IsNull(xml);
        }

        [Test]
        public void CustomerQueryWithResponseWithNoIterators()
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

            var ret = stepQueryWithIteratorMock.Object.ReceiveXML(authenticatedTicket, xml, string.Empty, string.Empty);
            Assert.AreEqual(0, ret);
        }

        [Test]
        public void CustomerQueryWithResponseWithIterators()
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

            var ret = stepQueryWithIteratorMock.Object.ReceiveXML(authenticatedTicket, xml, string.Empty, string.Empty);
            Assert.AreEqual(0, ret);
        }

        [Test]
        public void CustomerQueryWithTimeZoneFix()
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
                .Setup("ExecuteResponse", ItExpr.IsAny<AuthenticatedTicket>(), ItExpr.IsAny<CustomerQueryRsType>());
            stepQueryWithIteratorMock.CallBase = true;
            stepQueryWithIteratorMock.Object.SetOptions(qbXmlResponseOptions);

            var ret = stepQueryWithIteratorMock.Object.ReceiveXML(authenticatedTicket, xml, string.Empty, string.Empty);
            Assert.AreEqual(0, ret);
            var expectedHour = 17;
            stepQueryWithIteratorMock
                .Protected()
                .Verify("ExecuteResponse", Times.Once(), ItExpr.IsAny<AuthenticatedTicket>(), ItExpr.Is<CustomerQueryRsType>(m => m.CustomerRet[0].TimeModified.ToDateTime().ToUniversalTime().Hour == expectedHour));
        }

        [Test]
        public void CustomerQueryWithTimeZoneNoFix()
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
                .Setup("ExecuteResponse", ItExpr.IsAny<AuthenticatedTicket>(), ItExpr.IsAny<CustomerQueryRsType>());
            stepQueryWithIteratorMock.CallBase = true;
            stepQueryWithIteratorMock.Object.SetOptions(qbXmlResponseOptions);

            var ret = stepQueryWithIteratorMock.Object.ReceiveXML(authenticatedTicket, xml, string.Empty, string.Empty);
            Assert.AreEqual(0, ret);
            var expectedHour = 18;
            stepQueryWithIteratorMock
                .Protected()
                .Verify("ExecuteResponse", Times.Once(), ItExpr.IsAny<AuthenticatedTicket>(), ItExpr.Is<CustomerQueryRsType>(m => m.CustomerRet[0].TimeModified.ToDateTime().ToUniversalTime().Hour == expectedHour));
        }

        [Test]
        public void CustomerQueryWithIteratorInvalidMaxReturned()
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

            var xml = stepQueryWithIteratorMock.Object.SendXML(authenticatedTicket);

            XmlDocument requestXmlDoc = new XmlDocument();
            requestXmlDoc.LoadXml(xml);

            var node = requestXmlDoc.SelectSingleNode("//CustomerQueryRq");

            Assert.IsNotNull(node);
            Assert.AreEqual("Start", node.Attributes.GetNamedItem("iterator").Value);
            Assert.IsNotNull(node.SelectSingleNode("MaxReturned"));
            Assert.AreEqual(expectedMaxResult.ToString(), node.SelectSingleNode("MaxReturned").InnerText);
        }

        [Test]
        public void CustomerQueryWithIteratorZeroMaxReturned()
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

            var xml = stepQueryWithIteratorMock.Object.SendXML(authenticatedTicket);

            XmlDocument requestXmlDoc = new XmlDocument();
            requestXmlDoc.LoadXml(xml);

            var node = requestXmlDoc.SelectSingleNode("//CustomerQueryRq");

            Assert.IsNotNull(node);
            Assert.AreEqual("Start", node.Attributes.GetNamedItem("iterator").Value);
            Assert.IsNotNull(node.SelectSingleNode("MaxReturned"));
            Assert.AreEqual(expectedMaxResult.ToString(), node.SelectSingleNode("MaxReturned").InnerText);
        }

        [Test]
        public void CustomerQueryWithIteratorNegativeMaxReturned()
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

            var xml = stepQueryWithIteratorMock.Object.SendXML(authenticatedTicket);

            XmlDocument requestXmlDoc = new XmlDocument();
            requestXmlDoc.LoadXml(xml);

            var node = requestXmlDoc.SelectSingleNode("//CustomerQueryRq");

            Assert.IsNotNull(node);
            Assert.AreEqual("Start", node.Attributes.GetNamedItem("iterator").Value);
            Assert.IsNotNull(node.SelectSingleNode("MaxReturned"));
            Assert.AreEqual(expectedMaxResult.ToString(), node.SelectSingleNode("MaxReturned").InnerText);
        }

        [Test]
        public void CustomerQueryWithIteratorValidMaxReturned()
        {
            string defaultMaxResult = "500";
            var authenticatedTicket = new AuthenticatedTicket
            {
                Ticket = Guid.NewGuid().ToString(),
                CurrentStep = "step4"
            };

            var stepQueryWithIteratorMock = new Mock<StepQueryWithIteratorMaxReturnedHarness>(defaultMaxResult);
            stepQueryWithIteratorMock.CallBase = true;

            var xml = stepQueryWithIteratorMock.Object.SendXML(authenticatedTicket);

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
