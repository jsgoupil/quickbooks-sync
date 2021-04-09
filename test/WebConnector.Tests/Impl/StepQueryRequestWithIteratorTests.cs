using Moq;
using Moq.Protected;
using NUnit.Framework;
using QbSync.QbXml.Objects;
using QbSync.WebConnector.Impl;
using QbSync.WebConnector.Tests.Helpers;
using QbSync.WebConnector.Tests.Models;
using System;
using System.Threading.Tasks;
using System.Xml;

namespace QbSync.WebConnector.Tests.Impl
{
    [TestFixture]
    class StepQueryRequestWithIteratorTests
    {
        [Test]
        public async Task StepQueryRequestWithIterator_CustomerQueryWithNoInitialMessage()
        {
            var defaultMaxResult = 100;
            var authenticatedTicket = new AuthenticatedTicket
            {
                Ticket = Guid.NewGuid().ToString(),
                CurrentStep = "step4"
            };

            var stepQueryRequestWithIteratorMock = new Mock<StepQueryRequestWithIterator<CustomerQueryRqType>>
            {
                CallBase = true
            };

            var xml = await stepQueryRequestWithIteratorMock.Object.SendXMLAsync(authenticatedTicket);

            XmlDocument requestXmlDoc = new XmlDocument();
            requestXmlDoc.LoadXml(xml);

            var node = requestXmlDoc.SelectSingleNode("//CustomerQueryRq");

            Assert.IsNotNull(node);
            Assert.AreEqual("Start", node.Attributes.GetNamedItem("iterator").Value);
            Assert.IsNotNull(node.SelectSingleNode("MaxReturned"));
            Assert.AreEqual(defaultMaxResult.ToString(), node.SelectSingleNode("MaxReturned").InnerText);
        }

        [Test]
        public async Task StepQueryRequestWithIterator_CustomerQueryWithPreviousIterator()
        {
            var authenticatedTicket = new AuthenticatedTicket
            {
                Ticket = Guid.NewGuid().ToString(),
                CurrentStep = "step4"
            };
            var iteratorID = "123456";
            var iteratorKey = StepQueryRequestWithIterator.IteratorKey;
            var updateCustomerMock = new Mock<StepQueryRequestWithIterator<CustomerQueryRqType>>();
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
        public async Task StepQueryRequestWithIterator_CustomerQueryWithShortCircuitExecuteRequest()
        {
            var authenticatedTicket = new AuthenticatedTicket
            {
                Ticket = Guid.NewGuid().ToString(),
                CurrentStep = "step4"
            };
            var updateCustomerMock = new Mock<StepQueryRequestWithIterator<CustomerQueryRqType>>();
            updateCustomerMock
                .Protected()
                .Setup<Task<bool>>("ExecuteRequestAsync", ItExpr.IsAny<AuthenticatedTicket>(), ItExpr.IsAny<CustomerQueryRqType>())
                .Returns(Task.FromResult(false));
            updateCustomerMock.CallBase = true;

            var xml = await updateCustomerMock.Object.SendXMLAsync(authenticatedTicket);

            Assert.IsNull(xml);
        }

        [Test]
        public async Task StepQueryRequestWithIterator_CustomerQueryWithIteratorInvalidMaxReturned()
        {
            const int expectedMaxResult = 100;

            var defaultMaxResult = "INVALID_MAX_RETURN_VALUE";
            var authenticatedTicket = new AuthenticatedTicket
            {
                Ticket = Guid.NewGuid().ToString(),
                CurrentStep = "step4"
            };

            var stepQueryRequestWithIteratorMock = new Mock<StepQueryRequestWithIteratorMaxReturnedHarness>(defaultMaxResult)
            {
                CallBase = true
            };

            var xml = await stepQueryRequestWithIteratorMock.Object.SendXMLAsync(authenticatedTicket);

            XmlDocument requestXmlDoc = new XmlDocument();
            requestXmlDoc.LoadXml(xml);

            var node = requestXmlDoc.SelectSingleNode("//CustomerQueryRq");

            Assert.IsNotNull(node);
            Assert.AreEqual("Start", node.Attributes.GetNamedItem("iterator").Value);
            Assert.IsNotNull(node.SelectSingleNode("MaxReturned"));
            Assert.AreEqual(expectedMaxResult.ToString(), node.SelectSingleNode("MaxReturned").InnerText);
        }

        [Test]
        public async Task StepQueryRequestWithIterator_CustomerQueryWithIteratorZeroMaxReturned()
        {
            const int expectedMaxResult = 100;

            var defaultMaxResult = "0";
            var authenticatedTicket = new AuthenticatedTicket
            {
                Ticket = Guid.NewGuid().ToString(),
                CurrentStep = "step4"
            };

            var stepQueryRequestWithIteratorMock = new Mock<StepQueryRequestWithIteratorMaxReturnedHarness>(defaultMaxResult)
            {
                CallBase = true
            };

            var xml = await stepQueryRequestWithIteratorMock.Object.SendXMLAsync(authenticatedTicket);

            XmlDocument requestXmlDoc = new XmlDocument();
            requestXmlDoc.LoadXml(xml);

            var node = requestXmlDoc.SelectSingleNode("//CustomerQueryRq");

            Assert.IsNotNull(node);
            Assert.AreEqual("Start", node.Attributes.GetNamedItem("iterator").Value);
            Assert.IsNotNull(node.SelectSingleNode("MaxReturned"));
            Assert.AreEqual(expectedMaxResult.ToString(), node.SelectSingleNode("MaxReturned").InnerText);
        }

        [Test]
        public async Task StepQueryRequestWithIterator_CustomerQueryWithIteratorNegativeMaxReturned()
        {
            const int expectedMaxResult = 100;

            var defaultMaxResult = "-1";
            var authenticatedTicket = new AuthenticatedTicket
            {
                Ticket = Guid.NewGuid().ToString(),
                CurrentStep = "step4"
            };

            var stepQueryRequestWithIteratorMock = new Mock<StepQueryRequestWithIteratorMaxReturnedHarness>(defaultMaxResult)
            {
                CallBase = true
            };

            var xml = await stepQueryRequestWithIteratorMock.Object.SendXMLAsync(authenticatedTicket);

            XmlDocument requestXmlDoc = new XmlDocument();
            requestXmlDoc.LoadXml(xml);

            var node = requestXmlDoc.SelectSingleNode("//CustomerQueryRq");

            Assert.IsNotNull(node);
            Assert.AreEqual("Start", node.Attributes.GetNamedItem("iterator").Value);
            Assert.IsNotNull(node.SelectSingleNode("MaxReturned"));
            Assert.AreEqual(expectedMaxResult.ToString(), node.SelectSingleNode("MaxReturned").InnerText);
        }

        [Test]
        public async Task StepQueryRequestWithIterator_CustomerQueryWithIteratorValidMaxReturned()
        {
            string defaultMaxResult = "500";
            var authenticatedTicket = new AuthenticatedTicket
            {
                Ticket = Guid.NewGuid().ToString(),
                CurrentStep = "step4"
            };

            var stepQueryRequestWithIteratorMock = new Mock<StepQueryRequestWithIteratorMaxReturnedHarness>(defaultMaxResult)
            {
                CallBase = true
            };

            var xml = await stepQueryRequestWithIteratorMock.Object.SendXMLAsync(authenticatedTicket);

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
