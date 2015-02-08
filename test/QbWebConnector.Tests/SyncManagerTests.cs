using Moq;
using NUnit.Framework;
using QBSync.QbWebConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Moq.Protected;
using QbSync.QbWebConnector.Tests.Helpers;

namespace QbSync.QbWebConnector.Tests
{
    [TestFixture]
    class SyncManagerTests
    {
        // Used by AuthenticatorAttribute
        public Mock<IAuthenticator> authenticatorMock;
        public AuthenticatedTicket AuthenticatedTicket { get; set; }

        [SetUp]
        public void Initialize()
        {
            authenticatorMock = new Mock<IAuthenticator>();
        }

        [Test]
        [SetupInvalidTicket]
        [ExpectedException(ExpectedException = typeof(Exception), ExpectedMessage = "GetAuthenticationFromLogin must return a ticket.")]
        public void AuthenticateWithInvalidImplementation()
        {
            var syncManagerMock = new Mock<SyncManager>(authenticatorMock.Object);
            syncManagerMock.CallBase = true;
            var result = syncManagerMock.Object.Authenticate("user", "password");
        }

        [Test]
        public void AuthenticateWithWrongCredentials()
        {
            authenticatorMock
                .Setup(m => m.GetAuthenticationFromLogin(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new AuthenticatedTicket
                {
                    Ticket = Guid.NewGuid().ToString(),
                    Authenticated = false
                });

            var syncManagerMock = new Mock<SyncManager>(authenticatorMock.Object);
            syncManagerMock
                .Protected()
                .Setup("SaveChanges");

            syncManagerMock.CallBase = true;
            var result = syncManagerMock.Object.Authenticate("user", "password");

            Assert.IsNotEmpty(result[0]);
            Assert.AreEqual("nvu", result[1]);
            Assert.IsEmpty(result[2]);
            Assert.IsEmpty(result[3]);
            syncManagerMock
                .Protected()
                .Verify("SaveChanges", Times.Once());
        }

        [Test]
        [SetupValidTicket]
        public void AuthenticateWithValidCredentials()
        {
            var syncManagerMock = new Mock<SyncManager>(authenticatorMock.Object);
            syncManagerMock
                .Protected()
                .Setup("SaveChanges");
            syncManagerMock
                .Protected()
                .Setup<int>("GetWaitTime", ItExpr.IsAny<AuthenticatedTicket>())
                .Returns(0);

            syncManagerMock.CallBase = true;
            var result = syncManagerMock.Object.Authenticate("user", "password");

            Assert.IsNotEmpty(result[0]);
            Assert.IsEmpty(result[1]);
            Assert.IsEmpty(result[2]);
            Assert.IsEmpty(result[3]);
            syncManagerMock
                .Protected()
                .Verify("SaveChanges", Times.Once());
        }

        [Test]
        [SetupValidTicket]
        public void AuthenticateWithValidCredentialsHasNoWork()
        {
            var timeWait = 60;
            var syncManagerMock = new Mock<SyncManager>(authenticatorMock.Object);
            syncManagerMock
                .Protected()
                .Setup<int>("GetWaitTime", ItExpr.IsAny<AuthenticatedTicket>())
                .Returns(timeWait);

            syncManagerMock.CallBase = true;
            var result = syncManagerMock.Object.Authenticate("user", "password");

            Assert.IsNotEmpty(result[0]);
            Assert.AreEqual("none", result[1]);
            Assert.AreEqual(timeWait.ToString(), result[2]);
            Assert.IsEmpty(result[3]);
        }

        [Test]
        public void ClientVersionWithValidVersion()
        {
            var syncManager = new SyncManager(null);
            var result = syncManager.ClientVersion("2.1.0.30");

            Assert.IsEmpty(result);
        }

        [Test]
        public void ClientVersionWithNewerVersion()
        {
            var syncManager = new SyncManager(null);
            var result = syncManager.ClientVersion("2.1.0.31");

            Assert.IsEmpty(result);
        }

        [Test]
        public void ClientVersionWithInvalidVersion()
        {
            var syncManager = new SyncManager(null);
            var result = syncManager.ClientVersion("2.1.0.29");

            Assert.AreEqual("W:", result.Substring(0, 2));
        }

        [Test]
        public void ClientVersionWithSpecifiedVersion()
        {
            var specifiedVersion = new Version(1, 1, 1, 1);
            var syncManagerMock = new Mock<SyncManager>(null);
            syncManagerMock
                .Protected()
                .Setup<Version>("GetMinimumRequiredVersion")
                .Returns(specifiedVersion);
            syncManagerMock.CallBase = true;

            var result1 = syncManagerMock.Object.ClientVersion("1.1.1.2");
            var result2 = syncManagerMock.Object.ClientVersion("1.1.1.0");

            Assert.IsEmpty(result1);
            Assert.AreEqual("W:", result2.Substring(0, 2));
        }

        [Test]
        [SetupInvalidTicket]
        public void SendRequestXML_WithInvalidTicket()
        {
            var guid = Guid.NewGuid().ToString();
            var syncManagerMock = new Mock<SyncManager>(authenticatorMock.Object);
            syncManagerMock
                .Protected()
                .Setup("SaveChanges");

            syncManagerMock.CallBase = true;
            var result = syncManagerMock.Object.SendRequestXML(guid, null, null, null, 1, 1);

            Assert.IsEmpty(result);
            syncManagerMock
                .Protected()
                .Verify("SaveChanges", Times.Once());
        }

        [Test]
        [ExpectedException(ExpectedException = typeof(Exception), ExpectedMessage = "The type System.Object does not implement StepQueryResponse.")]
        public void SendRequestXML_InvalidStep()
        {
            var syncManagerMock = new Mock<SyncManager>(null);
            syncManagerMock.CallBase = true;
            syncManagerMock.Object.RegisterStep(0, typeof(object));
        }

        [Test]
        [SetupValidTicket]
        public void SendRequestXML_WithValidTicket_FirstStepHasWork()
        {
            var guid = Guid.NewGuid().ToString();
            var syncManagerMock = new Mock<SyncManager>(authenticatorMock.Object);
            syncManagerMock
                .Protected()
                .Setup("SaveChanges");

            syncManagerMock.CallBase = true;

            var expectedResult = "abc";
            var stepQueryResponseMock = new Mock<StepQueryResponse>();
            stepQueryResponseMock
                .Setup(m => m.SendXML())
                .Returns(expectedResult);

            syncManagerMock
                .Protected()
                .Setup<StepQueryResponse>("Invoke", ItExpr.Is<Type>((t) => t == typeof(MockStepQueryResponse1)), ItExpr.IsAny<AuthenticatedTicket>())
                .Returns(stepQueryResponseMock.Object);

            syncManagerMock.Object.RegisterStep(0, typeof(MockStepQueryResponse1));

            var result = syncManagerMock.Object.SendRequestXML(guid, null, null, null, 1, 1);

            Assert.AreEqual(expectedResult, result);
            syncManagerMock
                .Protected()
                .Verify("SaveChanges", Times.Once());
            Assert.AreEqual(0, AuthenticatedTicket.CurrentStep);
        }

        [Test]
        [SetupValidTicket]
        public void SendRequestXML_WithValidTicket_FirstStepHasNoWork()
        {
            var guid = Guid.NewGuid().ToString();

            var syncManagerMock = new Mock<SyncManager>(authenticatorMock.Object);
            syncManagerMock
                .Protected()
                .Setup("SaveChanges");

            syncManagerMock.CallBase = true;

            var expectedResult = "abc";
            var stepQueryResponseMock1 = new Mock<StepQueryResponse>();
            stepQueryResponseMock1
                .Setup(m => m.SendXML())
                .Returns((string)null);
            var stepQueryResponseMock2 = new Mock<StepQueryResponse>();
            stepQueryResponseMock2
                .Setup(m => m.SendXML())
                .Returns(expectedResult);

            syncManagerMock
                .Protected()
                .Setup<StepQueryResponse>("Invoke", ItExpr.Is<Type>((t) => t == typeof(MockStepQueryResponse1)), ItExpr.IsAny<AuthenticatedTicket>())
                .Returns(stepQueryResponseMock1.Object);
            syncManagerMock
                .Protected()
                .Setup<StepQueryResponse>("Invoke", ItExpr.Is<Type>((t) => t == typeof(MockStepQueryResponse2)), ItExpr.IsAny<AuthenticatedTicket>())
                .Returns(stepQueryResponseMock2.Object);

            syncManagerMock.Object.RegisterStep(0, typeof(MockStepQueryResponse1));
            syncManagerMock.Object.RegisterStep(1, typeof(MockStepQueryResponse2));

            var result = syncManagerMock.Object.SendRequestXML(guid, null, null, null, 1, 1);

            Assert.AreEqual(expectedResult, result);
            syncManagerMock
                .Protected()
                .Verify("SaveChanges", Times.Once());
            Assert.AreEqual(1, AuthenticatedTicket.CurrentStep);
        }

        [Test]
        [SetupValidTicket]
        public void SendRequestXML_WithValidTicket_NoStepsHaveWork()
        {
            var guid = Guid.NewGuid().ToString();
            var syncManagerMock = new Mock<SyncManager>(authenticatorMock.Object);
            syncManagerMock
                .Protected()
                .Setup("SaveChanges");

            syncManagerMock.CallBase = true;

            var stepQueryResponseMock1 = new Mock<StepQueryResponse>();
            stepQueryResponseMock1
                .Setup(m => m.SendXML())
                .Returns((string)null);

            syncManagerMock
                .Protected()
                .Setup<StepQueryResponse>("Invoke", ItExpr.Is<Type>((t) => t == typeof(MockStepQueryResponse1)), ItExpr.IsAny<AuthenticatedTicket>())
                .Returns(stepQueryResponseMock1.Object);

            syncManagerMock.Object.RegisterStep(0, typeof(MockStepQueryResponse1));

            var result = syncManagerMock.Object.SendRequestXML(guid, null, null, null, 1, 1);

            Assert.AreEqual(string.Empty, result);
            syncManagerMock
                .Protected()
                .Verify("SaveChanges", Times.Once());
            Assert.AreEqual(1, AuthenticatedTicket.CurrentStep);
        }

        [Test]
        [SetupInvalidTicket]
        public void ReceiveRequestXML_WithInvalidTicket()
        {
            var guid = Guid.NewGuid().ToString();
            var syncManagerMock = new Mock<SyncManager>(authenticatorMock.Object);
            syncManagerMock
                .Protected()
                .Setup("SaveChanges");

            syncManagerMock.CallBase = true;
            var result = syncManagerMock.Object.ReceiveRequestXML(guid, null, null, null);

            Assert.AreEqual(-1, result);
            syncManagerMock
                .Protected()
                .Verify("SaveChanges", Times.Once());
        }

        [Test]
        [SetupValidTicket]
        public void ReceiveRequestXML_WithValidTicket_ValidResponseAndGoToNext()
        {
            var guid = Guid.NewGuid().ToString();
            var syncManagerMock = new Mock<SyncManager>(authenticatorMock.Object);
            syncManagerMock
                .Protected()
                .Setup("SaveChanges");

            syncManagerMock.CallBase = true;

            var expectedResult = 0;
            var stepQueryResponseMock = new Mock<StepQueryResponse>();
            stepQueryResponseMock
                .Setup(m => m.ReceiveXML(null, null, null))
                .Returns(expectedResult);
            stepQueryResponseMock
                .Setup(m => m.GotoNextStep())
                .Returns(true);

            syncManagerMock
                .Protected()
                .Setup<StepQueryResponse>("Invoke", ItExpr.Is<Type>((t) => t == typeof(MockStepQueryResponse1)), ItExpr.IsAny<AuthenticatedTicket>())
                .Returns(stepQueryResponseMock.Object);

            syncManagerMock.Object.RegisterStep(0, typeof(MockStepQueryResponse1));

            var result = syncManagerMock.Object.ReceiveRequestXML(guid, null, null, null);

            Assert.AreEqual(expectedResult, result);
            syncManagerMock
                .Protected()
                .Verify("SaveChanges", Times.Once());
            Assert.AreEqual(1, AuthenticatedTicket.CurrentStep);
        }

        [Test]
        [SetupValidTicket]
        public void ReceiveRequestXML_WithValidTicket_ValidResponseAndAndStaySameStep()
        {
            var guid = Guid.NewGuid().ToString();
            var syncManagerMock = new Mock<SyncManager>(authenticatorMock.Object);
            syncManagerMock
                .Protected()
                .Setup("SaveChanges");

            syncManagerMock.CallBase = true;

            var expectedResult = 0;
            var stepQueryResponseMock = new Mock<StepQueryResponse>();
            stepQueryResponseMock
                .Setup(m => m.ReceiveXML(null, null, null))
                .Returns(expectedResult);
            stepQueryResponseMock
                .Setup(m => m.GotoNextStep())
                .Returns(false);

            syncManagerMock
                .Protected()
                .Setup<StepQueryResponse>("Invoke", ItExpr.Is<Type>((t) => t == typeof(MockStepQueryResponse1)), ItExpr.IsAny<AuthenticatedTicket>())
                .Returns(stepQueryResponseMock.Object);

            syncManagerMock.Object.RegisterStep(0, typeof(MockStepQueryResponse1));

            var result = syncManagerMock.Object.ReceiveRequestXML(guid, null, null, null);

            Assert.AreEqual(expectedResult, result);
            syncManagerMock
                .Protected()
                .Verify("SaveChanges", Times.Once());
            Assert.AreEqual(0, AuthenticatedTicket.CurrentStep);
        }

        [Test]
        [SetupValidTicket]
        public void ReceiveRequestXML_WithValidTicket_InvalidResponse()
        {
            var guid = Guid.NewGuid().ToString();
            var syncManagerMock = new Mock<SyncManager>(authenticatorMock.Object);
            syncManagerMock
                .Protected()
                .Setup("SaveChanges");

            syncManagerMock.CallBase = true;

            var expectedResult = -1;
            var stepQueryResponseMock = new Mock<StepQueryResponse>();
            stepQueryResponseMock
                .Setup(m => m.ReceiveXML(null, null, null))
                .Returns(expectedResult);

            syncManagerMock
                .Protected()
                .Setup<StepQueryResponse>("Invoke", ItExpr.Is<Type>((t) => t == typeof(MockStepQueryResponse1)), ItExpr.IsAny<AuthenticatedTicket>())
                .Returns(stepQueryResponseMock.Object);

            syncManagerMock.Object.RegisterStep(0, typeof(MockStepQueryResponse1));

            var result = syncManagerMock.Object.ReceiveRequestXML(guid, null, null, null);

            Assert.AreEqual(expectedResult, result);
            syncManagerMock
                .Protected()
                .Verify("SaveChanges", Times.Once());
            Assert.AreEqual(0, AuthenticatedTicket.CurrentStep);
        }

        [Test]
        [SetupInvalidTicket]
        public void GetLastError_WithInvalidTicket()
        {
            var guid = Guid.NewGuid().ToString();
            var syncManagerMock = new Mock<SyncManager>(authenticatorMock.Object);
            syncManagerMock
                .Protected()
                .Setup("SaveChanges");

            syncManagerMock.CallBase = true;
            var result = syncManagerMock.Object.GetLastError(guid);

            Assert.IsEmpty(result);
            syncManagerMock
                .Protected()
                .Verify("SaveChanges", Times.Once());
        }

        [Test]
        [SetupValidTicket]
        public void GetLastError_WithValidTicket_ValidResponse()
        {
            var guid = Guid.NewGuid().ToString();
            var syncManagerMock = new Mock<SyncManager>(authenticatorMock.Object);
            syncManagerMock
                .Protected()
                .Setup("SaveChanges");

            syncManagerMock.CallBase = true;

            var expectedResult = "abc";
            var stepQueryResponseMock = new Mock<StepQueryResponse>();
            stepQueryResponseMock
                .Setup(m => m.LastError())
                .Returns(expectedResult);

            syncManagerMock
                .Protected()
                .Setup<StepQueryResponse>("Invoke", ItExpr.Is<Type>((t) => t == typeof(MockStepQueryResponse1)), ItExpr.IsAny<AuthenticatedTicket>())
                .Returns(stepQueryResponseMock.Object);

            syncManagerMock.Object.RegisterStep(0, typeof(MockStepQueryResponse1));

            var result = syncManagerMock.Object.GetLastError(guid);

            Assert.AreEqual(expectedResult, result);
            syncManagerMock
                .Protected()
                .Verify("SaveChanges", Times.Once());
            Assert.AreEqual(0, AuthenticatedTicket.CurrentStep);
        }

        [Test]
        [SetupInvalidTicket]
        public void ConnectionError_WithInvalidTicket()
        {
            var guid = Guid.NewGuid().ToString();
            var syncManagerMock = new Mock<SyncManager>(authenticatorMock.Object);
            syncManagerMock
                .Protected()
                .Setup("SaveChanges");

            syncManagerMock.CallBase = true;
            var result = syncManagerMock.Object.ConnectionError(guid, null, null);

            Assert.AreEqual("done", result);
            syncManagerMock
                .Protected()
                .Verify("SaveChanges", Times.Once());
        }

        [Test]
        [SetupValidTicket]
        public void ConnectionError_WithValidTicket_ValidResponse()
        {
            var guid = Guid.NewGuid().ToString();
            var syncManagerMock = new Mock<SyncManager>(authenticatorMock.Object);
            syncManagerMock
                .Protected()
                .Setup("SaveChanges");

            syncManagerMock.CallBase = true;
            var result = syncManagerMock.Object.ConnectionError(guid, null, null);

            Assert.AreEqual("done", result);
            syncManagerMock
                .Protected()
                .Verify("SaveChanges", Times.Once());
            Assert.AreEqual(0, AuthenticatedTicket.CurrentStep);
        }

        [Test]
        [SetupInvalidTicket]
        public void CloseConnection_WithInvalidTicket()
        {
            var guid = Guid.NewGuid().ToString();

            var syncManagerMock = new Mock<SyncManager>(authenticatorMock.Object);
            syncManagerMock
                .Protected()
                .Setup("SaveChanges");

            syncManagerMock.CallBase = true;
            var result = syncManagerMock.Object.CloseConnection(guid);

            Assert.AreEqual("Invalid Ticket", result);
            syncManagerMock
                .Protected()
                .Verify("SaveChanges", Times.Once());
        }

        [Test]
        [SetupValidTicket]
        public void CloseConnection_WithValidTicket_ValidResponse()
        {
            var guid = Guid.NewGuid().ToString();
            var syncManagerMock = new Mock<SyncManager>(authenticatorMock.Object);
            syncManagerMock
                .Protected()
                .Setup("SaveChanges");

            syncManagerMock.CallBase = true;
            var result = syncManagerMock.Object.CloseConnection(guid);

            Assert.AreEqual("Sync Completed", result);
            syncManagerMock
                .Protected()
                .Verify("SaveChanges", Times.Once());
        }
    }
}