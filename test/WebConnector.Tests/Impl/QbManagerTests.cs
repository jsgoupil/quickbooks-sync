using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using NUnit.Framework;
using QbSync.QbXml;
using QbSync.WebConnector.AspNetCore;
using QbSync.WebConnector.Core;
using QbSync.WebConnector.Tests.Helpers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QbSync.WebConnector.Tests.Impl
{
    [TestFixture]
    class QbManagerTests
    {
        public Mock<IAuthenticator> authenticatorMock;
        public Mock<IMessageValidator> messageValidatorMock;
        public Mock<IWebConnectorHandler> webConnectorHandlerMock;
        public Mock<IStepQueryRequest> stepQueryRequestMock;
        public Mock<IStepQueryResponse> stepQueryResponseMock;
        public Mock<ILogger<QbManager>> loggerMock;

        public IAuthenticatedTicket AuthenticatedTicket { get; set; }

        [SetUp]
        public void Initialize()
        {
            authenticatorMock = new Mock<IAuthenticator>();
            messageValidatorMock = new Mock<IMessageValidator>();
            webConnectorHandlerMock = new Mock<IWebConnectorHandler>();
            stepQueryRequestMock = new Mock<IStepQueryRequest>();
            stepQueryResponseMock = new Mock<IStepQueryResponse>();
            loggerMock = new Mock<ILogger<QbManager>>();
        }

        [Test]
        [SetupInvalidTicket]
        public async Task QbManager_AuthenticateWithInvalidImplementation()
        {
            var serviceMock = GetServiceMock();
            serviceMock
                .Protected()
                .Setup<Task>("OnExceptionAsync", ItExpr.IsAny<IAuthenticatedTicket>(), ItExpr.IsAny<Exception>())
                .Returns(Task.CompletedTask);

            await serviceMock.Object.AuthenticateAsync("user", "password");

            serviceMock
                .Protected()
                .Verify("OnExceptionAsync", Times.Once(), ItExpr.Is<IAuthenticatedTicket>(m => m == null), ItExpr.Is<Exception>(m => m.Message == "GetAuthenticationFromLoginAsync must return a ticket."));
        }

        [Test]
        [SetupWrongCredentialTicket]
        public async Task QbManager_AuthenticateWithWrongCredentials()
        {
            var serviceMock = GetServiceMock();
            serviceMock
                .Protected()
                .Setup<Task>("SaveChangesAsync", ItExpr.IsAny<IAuthenticatedTicket>())
                .Returns(Task.CompletedTask);

            serviceMock.CallBase = true;
            var result = await serviceMock.Object.AuthenticateAsync("user", "password");

            Assert.IsNotEmpty(result[0]);
            Assert.AreEqual("nvu", result[1]);
            Assert.IsEmpty(result[2]);
            Assert.IsEmpty(result[3]);
            serviceMock
                .Protected()
                .Verify("SaveChangesAsync", Times.Once(), ItExpr.IsAny<IAuthenticatedTicket>());
        }

        [Test]
        [SetupValidTicket]
        [SetupWebConnectorHandler(WaitTime = 0)]
        public async Task QbManager_AuthenticateWithValidCredentials()
        {
            var service = GetService();
            var result = await service.AuthenticateAsync("user", "password");

            Assert.IsNotEmpty(result[0]);
            Assert.IsEmpty(result[1]);
            Assert.IsEmpty(result[2]);
            Assert.IsEmpty(result[3]);
        }

        [Test]
        [SetupValidTicket]
        [SetupWebConnectorHandler(CompanyFile = "D:\\file.qbw")]
        public async Task QbManager_AuthenticateWithValidCredentialsOpenCustomCompanyFile()
        {
            var service = GetService();
            var result = await service.AuthenticateAsync("user", "password");

            Assert.IsNotEmpty(result[0]);
            Assert.AreEqual("D:\\file.qbw", result[1]);
            Assert.IsEmpty(result[2]);
            Assert.IsEmpty(result[3]);
        }

        [Test]
        [SetupValidTicket]
        [SetupWebConnectorHandler(WaitTime = 60)]
        public async Task QbManager_AuthenticateWithValidCredentialsHasNoWork()
        {
            var service = GetService();
            var result = await service.AuthenticateAsync("user", "password");

            Assert.IsNotEmpty(result[0]);
            Assert.AreEqual("none", result[1]);
            Assert.AreEqual("60", result[2]);
            Assert.IsEmpty(result[3]);
        }

        [Test]
        public void QbManager_ClientVersionWithValidVersion()
        {
            var service = GetService();
            var result = service.ClientVersion("2.1.0.30");

            Assert.IsEmpty(result);
        }

        [Test]
        public void QbManager_ClientVersionWithNewerVersion()
        {
            var service = GetService();
            var result = service.ClientVersion("2.1.0.31");

            Assert.IsEmpty(result);
        }

        [Test]
        public void QbManager_ClientVersionWithInvalidVersion()
        {
            var syncManager = GetService();
            var result = syncManager.ClientVersion("2.1.0.29");

            Assert.AreEqual("W:", result.Substring(0, 2));
        }

        [Test]
        public void QbManager_ClientVersionWithSpecifiedVersion()
        {
            var specifiedVersion = new Version(1, 1, 1, 1);
            var serviceMock = GetServiceMock(); new Mock<QbManager>(null);
            serviceMock
                .Protected()
                .Setup<Version>("GetMinimumRequiredVersion")
                .Returns(specifiedVersion);

            var result1 = serviceMock.Object.ClientVersion("1.1.1.2");
            var result2 = serviceMock.Object.ClientVersion("1.1.1.0");

            Assert.IsEmpty(result1);
            Assert.AreEqual("W:", result2.Substring(0, 2));
        }

        [Test]
        [SetupInvalidTicket]
        public async Task QbManager_SendRequestXML_WithInvalidTicket()
        {
            var guid = Guid.NewGuid().ToString();
            var service = GetService();
            var result = await service.SendRequestXMLAsync(guid, null, null, null, 13, 0);

            Assert.IsEmpty(result);
        }

        [Test]
        [SetupValidTicket]
        public async Task QbManager_SendRequestXML_WithFirstResponse()
        {
            var guid = Guid.NewGuid().ToString();
            var serviceMock = GetServiceMock();
            var initial = "INITIAL";
            serviceMock
                .Protected()
                .Setup<Task>("ProcessClientInformationAsync", ItExpr.IsAny<IAuthenticatedTicket>(), ItExpr.IsAny<string>())
                .Returns(Task.CompletedTask);

            await serviceMock.Object.SendRequestXMLAsync(guid, initial, null, null, 13, 0);

            serviceMock
                .Protected()
                .Verify("ProcessClientInformationAsync", Times.Once(), ItExpr.IsAny<IAuthenticatedTicket>(), ItExpr.Is<string>(m => m == initial));
        }

        [Test]
        [SetupValidTicket]
        [SetupMessageValidator]
        public async Task QbManager_SendRequestXML_WithValidTicket_FirstStepHasWork()
        {
            var expectedResult = "abc";
            stepQueryRequestMock
                .Setup(m => m.SendXMLAsync(It.IsAny<IAuthenticatedTicket>()))
                .ReturnsAsync(expectedResult);
            stepQueryRequestMock
                .SetupGet(m => m.Name)
                .Returns("STEP1");

            var guid = Guid.NewGuid().ToString();
            var service = GetService();

            var result = await service.SendRequestXMLAsync(guid, null, null, null, 13, 0);

            Assert.AreEqual(expectedResult, result);
            Assert.AreEqual("STEP1", AuthenticatedTicket.CurrentStep);
        }

        [Test]
        [SetupValidTicket]
        [SetupMessageValidator(ValidMessage = false)]
        public async Task QbManager_SendRequestXML_WithValidTicket_WithOldQbXML()
        {
            var expectedResult = "abc";
            stepQueryRequestMock
                .Setup(m => m.SendXMLAsync(It.IsAny<IAuthenticatedTicket>()))
                .ReturnsAsync(expectedResult);
            stepQueryRequestMock
                .SetupGet(m => m.Name)
                .Returns("STEP1");

            var guid = Guid.NewGuid().ToString();
            var service = GetService();

            var result = await service.SendRequestXMLAsync(guid, null, null, null, 12, 0);

            Assert.AreEqual(string.Empty, result);
            Assert.AreEqual(null, AuthenticatedTicket.CurrentStep);
        }

        [Test]
        [SetupValidTicket]
        [SetupMessageValidator()]
        public async Task QbManager_SendRequestXML_WithValidTicket_FirstStepHasNoWork()
        {
            var expectedResult1 = (string)null;
            stepQueryRequestMock
                .Setup(m => m.SendXMLAsync(It.IsAny<IAuthenticatedTicket>()))
                .ReturnsAsync(expectedResult1);
            stepQueryRequestMock
                .SetupGet(m => m.Name)
                .Returns("STEP1");

            var step2 = "STEP2";
            var expectedResult2 = "def";
            var stepQueryRequestMock2 = new Mock<IStepQueryRequest>();
            stepQueryRequestMock2
                .Setup(m => m.SendXMLAsync(It.IsAny<IAuthenticatedTicket>()))
                .ReturnsAsync(expectedResult2);
            stepQueryRequestMock2
                .SetupGet(m => m.Name)
                .Returns(step2);

            var guid = Guid.NewGuid().ToString();
            var service = GetService(new List<IStepQueryRequest> { stepQueryRequestMock.Object, stepQueryRequestMock2.Object }, null);

            var result = await service.SendRequestXMLAsync(guid, null, null, null, 13, 0);

            Assert.AreEqual(expectedResult2, result);
            Assert.AreEqual(step2, AuthenticatedTicket.CurrentStep);
        }

        [Test]
        [SetupValidTicket]
        public async Task QbManager_SendRequestXML_WithValidTicket_NoStepsHaveWork()
        {
            var expectedResult = (string)null;
            stepQueryRequestMock
                .Setup(m => m.SendXMLAsync(It.IsAny<IAuthenticatedTicket>()))
                .ReturnsAsync(expectedResult);
            stepQueryRequestMock
                .SetupGet(m => m.Name)
                .Returns("STEP1");

            var guid = Guid.NewGuid().ToString();
            var service = GetService();

            var result = await service.SendRequestXMLAsync(guid, null, null, null, 13, 0);

            Assert.AreEqual(string.Empty, result);
            Assert.AreEqual(null, AuthenticatedTicket.CurrentStep);
        }

        [Test]
        [SetupInvalidTicket]
        public async Task QbManager_ReceiveRequestXML_WithInvalidTicket()
        {
            var guid = Guid.NewGuid().ToString();
            var service = GetService();
            var result = await service.ReceiveRequestXMLAsync(guid, null, null, null);

            Assert.AreEqual(-1, result);
        }

        [Test]
        [SetupValidTicket]
        public async Task QbManager_ReceiveRequestXML_WithValidTicket_ValidResponseAndGoToNext()
        {
            var expectedResult = 0;
            stepQueryResponseMock
                .Setup(m => m.ReceiveXMLAsync(AuthenticatedTicket, null, null, null, null))
                .ReturnsAsync(expectedResult);
            stepQueryResponseMock
                .Setup(m => m.GotoNextStepAsync())
                .ReturnsAsync(true);
            stepQueryResponseMock
                .SetupGet(m => m.Name)
                .Returns("STEP1");

            var guid = Guid.NewGuid().ToString();
            var service = GetService();
            var result = await service.ReceiveRequestXMLAsync(guid, null, null, null);

            Assert.AreEqual(QbManager.FINISHED_STEP, AuthenticatedTicket.CurrentStep);
        }

        [Test]
        [SetupValidTicket]
        public async Task QbManager_ReceiveRequestXML_WithValidTicket_ValidResponseAndGoToStep()
        {
            var expectedResult = 0;
            var gotoStep = "step2";
            stepQueryResponseMock
                .Setup(m => m.ReceiveXMLAsync(AuthenticatedTicket, null, null, null, null))
                .ReturnsAsync(expectedResult);
            stepQueryResponseMock
                .Setup(m => m.GotoStepAsync())
                .ReturnsAsync(gotoStep);
            stepQueryResponseMock
                .SetupGet(m => m.Name)
                .Returns("STEP1");

            var guid = Guid.NewGuid().ToString();
            var service = GetService();
            await service.ReceiveRequestXMLAsync(guid, null, null, null);

            Assert.AreEqual(gotoStep, AuthenticatedTicket.CurrentStep);
        }

        [Test]
        [SetupValidTicket]
        public async Task QbManager_ReceiveRequestXML_WithValidTicket_ValidResponseAndAndStaySameStep()
        {
            var expectedResult = 0;
            stepQueryResponseMock
                .Setup(m => m.ReceiveXMLAsync(AuthenticatedTicket, null, null, null, null))
                .ReturnsAsync(expectedResult);
            stepQueryResponseMock
                .Setup(m => m.GotoNextStepAsync())
                .ReturnsAsync(false);
            stepQueryResponseMock
                .SetupGet(m => m.Name)
                .Returns("STEP1");

            var guid = Guid.NewGuid().ToString();
            var service = GetService();
            var result = await service.ReceiveRequestXMLAsync(guid, null, null, null);

            Assert.AreEqual(expectedResult, result);
            Assert.AreEqual(null, AuthenticatedTicket.CurrentStep);
        }

        [Test]
        [SetupValidTicket]
        public async Task QbManager_ReceiveRequestXML_WithValidTicket_InvalidResponse()
        {
            var expectedResult = -1;
            stepQueryResponseMock
                .Setup(m => m.ReceiveXMLAsync(AuthenticatedTicket, null, null, null, null))
                .ReturnsAsync(expectedResult);
            stepQueryResponseMock
                .SetupGet(m => m.Name)
                .Returns("STEP1");

            var guid = Guid.NewGuid().ToString();
            var service = GetService();
            var result = await service.ReceiveRequestXMLAsync(guid, null, null, null);

            Assert.AreEqual(expectedResult, result);
            Assert.AreEqual(null, AuthenticatedTicket.CurrentStep);
        }

        [Test]
        [SetupInvalidTicket]
        public async Task QbManager_GetLastError_WithInvalidTicket()
        {
            var guid = Guid.NewGuid().ToString();
            var service = GetService();
            var result = await service.GetLastErrorAsync(guid);

            Assert.IsEmpty(result);
        }

        [Test]
        [SetupValidTicket]
        [SetupMessageValidator]
        public async Task QbManager_GetLastError_WithValidTicket_ValidResponse()
        {
            var expectedResult = "abc";
            stepQueryResponseMock
                .Setup(m => m.LastErrorAsync(AuthenticatedTicket))
                .ReturnsAsync(expectedResult);
            stepQueryResponseMock
                .SetupGet(m => m.Name)
                .Returns("STEP1");

            var guid = Guid.NewGuid().ToString();
            var service = GetService();
            var result = await service.GetLastErrorAsync(guid);

            Assert.AreEqual(expectedResult, result);
            Assert.AreEqual(null, AuthenticatedTicket.CurrentStep);
        }

        [Test]
        [SetupValidTicket]
        [SetupMessageValidator(ValidMessage = false)]
        public async Task QbManager_GetLastError_WithValidTicket_WrongVersion()
        {
            var guid = Guid.NewGuid().ToString();
            var service = GetService();

            var expectedResult = string.Format("The server requires QbXml version {0}.{1} or higher. Please upgrade QuickBooks.", QbXmlRequest.VERSION.Major, QbXmlRequest.VERSION.Minor);
            var result = await service.GetLastErrorAsync(guid);

            Assert.AreEqual(expectedResult, result);
            Assert.AreEqual(null, AuthenticatedTicket.CurrentStep);
        }

        [Test]
        [SetupInvalidTicket]
        public async Task QbManager_ConnectionError_WithInvalidTicket()
        {
            var guid = Guid.NewGuid().ToString();
            var service = GetService();
            var result = await service.ConnectionErrorAsync(guid, null, null);

            Assert.AreEqual("done", result);
        }

        [Test]
        [SetupValidTicket]
        public async Task QbManager_ConnectionError_WithValidTicket_ValidResponse()
        {
            var guid = Guid.NewGuid().ToString();
            var service = GetService();
            var result = await service.ConnectionErrorAsync(guid, null, null);

            Assert.AreEqual("done", result);
            Assert.AreEqual(null, AuthenticatedTicket.CurrentStep);
        }

        [Test]
        [SetupInvalidTicket]
        public async Task QbManager_CloseConnection_WithInvalidTicket()
        {
            var guid = Guid.NewGuid().ToString();
            var service = GetService();
            var result = await service.CloseConnectionAsync(guid);

            Assert.AreEqual("Invalid Ticket", result);
        }

        [Test]
        [SetupValidTicket]
        public async Task QbManager_CloseConnection_WithValidTicket_ValidResponse()
        {
            var guid = Guid.NewGuid().ToString();
            var service = GetService();
            var result = await service.CloseConnectionAsync(guid);

            Assert.AreEqual("Sync Completed", result);
        }

        [Test]
        [SetupValidTicket]
        //[Platform(Exclude = "Mono", Reason = "Mono doesn't like the throw on async, so it's not handled properly.")]
        public async Task QbManager_Function_With_Exception()
        {
            var ex = new Exception();
            stepQueryResponseMock
                .Setup(m => m.ReceiveXMLAsync(AuthenticatedTicket, null, null, null, null))
                .Throws(ex);

            var guid = Guid.NewGuid().ToString();
            var syncManagerMock = GetServiceMock();
            syncManagerMock
                .Protected()
                .Setup<Task>("OnExceptionAsync", ItExpr.IsAny<IAuthenticatedTicket>(), ItExpr.IsAny<Exception>())
                .Returns(Task.CompletedTask);

            syncManagerMock.CallBase = true;

            var expectedResult = -1;
            var IStepQueryResponseMock1 = new Mock<IStepQueryResponse>();
            IStepQueryResponseMock1
                .Setup(m => m.ReceiveXMLAsync(AuthenticatedTicket, null, null, null, null))
                .Throws(ex);

            var result = await syncManagerMock.Object.ReceiveRequestXMLAsync(guid, null, null, null);

            Assert.AreEqual(expectedResult, result);

            syncManagerMock
                .Protected()
                .Verify("OnExceptionAsync", Times.Once(), ItExpr.Is<IAuthenticatedTicket>(m => m == AuthenticatedTicket), ItExpr.Is<Exception>(m => m.InnerException == ex));
        }

        private QbManager GetService()
        {
            return GetService(
                new List<IStepQueryRequest>
                {
                    stepQueryRequestMock.Object,
                },
                new List<IStepQueryResponse>
                {
                    stepQueryResponseMock.Object
                }
            );
        }

        private QbManager GetService(IEnumerable<IStepQueryRequest> stepRequests, IEnumerable<IStepQueryResponse> stepResponses)
        {
            return new QbManager(
                authenticatorMock.Object,
                messageValidatorMock.Object,
                webConnectorHandlerMock.Object,
                stepRequests,
                stepResponses,
                loggerMock.Object
            );
        }

        private Mock<QbManager> GetServiceMock()
        {
            return GetServiceMock(
                new List<IStepQueryRequest>
                {
                    stepQueryRequestMock.Object,
                },
                new List<IStepQueryResponse>
                {
                    stepQueryResponseMock.Object
                }
            );
        }

        private Mock<QbManager> GetServiceMock(IEnumerable<IStepQueryRequest> stepRequests, IEnumerable<IStepQueryResponse> stepResponses)
        {
            var mock = new Mock<QbManager>(
                authenticatorMock.Object,
                messageValidatorMock.Object,
                webConnectorHandlerMock.Object,
                stepRequests,
                stepResponses,
                loggerMock.Object
            )
            {
                CallBase = true
            };

            return mock;
        }
    }
}