using Moq;
using NUnit.Framework;
using QbSync.QbXml.Objects;
using QbSync.WebConnector.Core;
using QbSync.WebConnector.Impl;
using QbSync.WebConnector.Tests.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml;

namespace QbSync.WebConnector.Tests.Impl
{
    [TestFixture]
    class GroupStepQueryRequestBaseTests
    {
        [Test]
        public async Task GroupStepQueryRequestBaseTests_WithNoMessage()
        {
            var authenticatedTicket = new AuthenticatedTicket
            {
                Ticket = Guid.NewGuid().ToString(),
                CurrentStep = "step4"
            };

            var groupStepQueryRequestMock = new Mock<GroupStepQueryRequestBase>();
            groupStepQueryRequestMock.CallBase = true;

            var xml = await groupStepQueryRequestMock.Object.SendXMLAsync(authenticatedTicket);
            Assert.IsNull(xml);
        }

        [Test]
        public async Task GroupStepQueryRequestBaseTests_WithTwoMessages()
        {
            var authenticatedTicket = new AuthenticatedTicket
            {
                Ticket = Guid.NewGuid().ToString(),
                CurrentStep = "step4"
            };

            var groupStepQueryRequestMock = new Mock<GroupStepQueryRequestBase>();
            groupStepQueryRequestMock
                .Setup(m => m.ExecuteRequestAsync(It.Is<IAuthenticatedTicket>(n => n == authenticatedTicket)))
                .ReturnsAsync(() =>
                {
                    return new List<IQbRequest>
                    {
                        new CustomerQueryRqType
                        {
                            ActiveStatus = ActiveStatus.ActiveOnly
                        },
                        new CustomerAddRqType
                        {
                            CustomerAdd = new CustomerAdd
                            {
                                Name = "Unique Name",
                                FirstName = "User 1"
                            }
                        }
                    };
                });
            groupStepQueryRequestMock.CallBase = true;

            var xml = await groupStepQueryRequestMock.Object.SendXMLAsync(authenticatedTicket);

            XmlDocument requestXmlDoc = new XmlDocument();
            requestXmlDoc.LoadXml(xml);

            var msgRqs = requestXmlDoc.SelectNodes("//QBXMLMsgsRq");
            Assert.AreEqual(1, msgRqs.Count);

            var msgRq = msgRqs.Item(0);
            Assert.AreEqual("continueOnError", msgRq.Attributes.GetNamedItem("onError").Value);
            var requestNodes = msgRq.SelectNodes("*");

            Assert.AreEqual(2, requestNodes.Count);

            var node1 = requestNodes.Item(0);
            var node2 = requestNodes.Item(1);

            Assert.AreEqual("CustomerQueryRq", node1.Name);
            Assert.AreEqual("CustomerAddRq", node2.Name);
        }

        [Test]
        public async Task GroupStepQueryRequestBaseTests_WithDifferentOnError()
        {
            var authenticatedTicket = new AuthenticatedTicket
            {
                Ticket = Guid.NewGuid().ToString(),
                CurrentStep = "step4"
            };

            var groupStepQueryRequestMock = new Mock<GroupStepQueryRequestBase>();
            groupStepQueryRequestMock
                .Setup(m => m.ExecuteRequestAsync(It.Is<IAuthenticatedTicket>(n => n == authenticatedTicket)))
                .ReturnsAsync(() =>
                {
                    return new List<IQbRequest>
                    {
                        new CustomerQueryRqType
                        {
                            ActiveStatus = ActiveStatus.ActiveOnly
                        }
                    };
                });
            groupStepQueryRequestMock
                .Setup(m => m.GetOnErrorAttributeAsync(It.Is<IAuthenticatedTicket>(n => n == authenticatedTicket)))
                .ReturnsAsync(() =>
                {
                    return QBXMLMsgsRqOnError.stopOnError;
                });
            groupStepQueryRequestMock.CallBase = true;

            var xml = await groupStepQueryRequestMock.Object.SendXMLAsync(authenticatedTicket);

            XmlDocument requestXmlDoc = new XmlDocument();
            requestXmlDoc.LoadXml(xml);

            var msgRqs = requestXmlDoc.SelectNodes("//QBXMLMsgsRq");

            var msgRq = msgRqs.Item(0);
            Assert.AreEqual("stopOnError", msgRq.Attributes.GetNamedItem("onError").Value);
        }
    }
}
