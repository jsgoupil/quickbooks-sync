using Moq;
using Moq.Protected;
using NUnit.Framework;
using QbSync.QbXml;
using QbSync.WebConnector.Tests.Helpers;
using System;

namespace QbSync.WebConnector.Tests
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
        public void AuthenticateWithInvalidImplementation()
        {
            var syncManagerMock = new Mock<SyncManager>(authenticatorMock.Object);
            syncManagerMock
                .Protected()
                .Setup("OnException", ItExpr.IsAny<AuthenticatedTicket>(), ItExpr.IsAny<Exception>());

            syncManagerMock.CallBase = true;
            syncManagerMock.Object.Authenticate("user", "password");

            syncManagerMock
                .Protected()
                .Verify("OnException", Times.Once(), ItExpr.Is<AuthenticatedTicket>(m => m == null), ItExpr.Is<Exception>(m => m.Message == "GetAuthenticationFromLogin must return a ticket."));
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
            var result = syncManagerMock.Object.SendRequestXML(guid, null, null, null, 13, 0);

            Assert.IsEmpty(result);
            syncManagerMock
                .Protected()
                .Verify("SaveChanges", Times.Once());
        }

        [Test]
        [SetupValidTicket]
        public void SendRequestXML_WithFirstResponse()
        {
            var guid = Guid.NewGuid().ToString();
            var syncManagerMock = new Mock<SyncManager>(authenticatorMock.Object);
            var initial = "INITIAL";
            syncManagerMock
                .Protected()
                .Setup("ProcessClientInformation", ItExpr.IsAny<AuthenticatedTicket>(), ItExpr.IsAny<string>());

            syncManagerMock.CallBase = true;

            var result = syncManagerMock.Object.SendRequestXML(guid, initial, null, null, 13, 0);

            syncManagerMock
                .Protected()
                .Verify("ProcessClientInformation", Times.Once(), ItExpr.IsAny<AuthenticatedTicket>(), ItExpr.Is<string>(m => m == initial));
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

            var versionValidator = new Mock<IVersionValidator>();
            versionValidator
                .Setup(m => m.ValidateVersion(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()))
                .Returns(true);
            syncManagerMock.Object.VersionValidator = versionValidator.Object;

            var expectedResult = "abc";
            var stepQueryResponseMock1 = new Mock<StepQueryResponse>();
            stepQueryResponseMock1
                .Setup(m => m.SendXML(AuthenticatedTicket))
                .Returns(expectedResult);
            stepQueryResponseMock1
                .SetupGet(m => m.Name)
                .Returns("Mock1");

            syncManagerMock.Object.RegisterStep(stepQueryResponseMock1.Object);

            var result = syncManagerMock.Object.SendRequestXML(guid, null, null, null, 13, 0);

            Assert.AreEqual(expectedResult, result);
            syncManagerMock
                .Protected()
                .Verify("SaveChanges", Times.Once());
            Assert.AreEqual(AuthenticatedTicket.InitialStep, AuthenticatedTicket.CurrentStep);
        }

        [Test]
        [SetupValidTicket]
        public void SendRequestXML_WithValidTicket_WithOldQbXML()
        {
            var guid = Guid.NewGuid().ToString();
            var syncManagerMock = new Mock<SyncManager>(authenticatorMock.Object);
            syncManagerMock
                .Protected()
                .Setup("SaveChanges");

            syncManagerMock.CallBase = true;

            var versionValidator = new Mock<IVersionValidator>();
            versionValidator
                .Setup(m => m.ValidateVersion(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()))
                .Returns(false);
            syncManagerMock.Object.VersionValidator = versionValidator.Object;

            var expectedResult = string.Empty;
            var stepQueryResponseMock1 = new Mock<StepQueryResponse>();
            stepQueryResponseMock1
                .Setup(m => m.SendXML(AuthenticatedTicket))
                .Returns("abc");
            stepQueryResponseMock1
                .SetupGet(m => m.Name)
                .Returns("Mock1");

            syncManagerMock.Object.RegisterStep(stepQueryResponseMock1.Object);

            var result = syncManagerMock.Object.SendRequestXML(guid, null, null, null, 12, 0);

            Assert.AreEqual(expectedResult, result);
            syncManagerMock
                .Protected()
                .Verify("SaveChanges", Times.Once());
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
                .Setup(m => m.SendXML(AuthenticatedTicket))
                .Returns((string)null);
            stepQueryResponseMock1
                .SetupGet(m => m.Name)
                .Returns("Mock1");
            var stepQueryResponseMock2 = new Mock<StepQueryResponse>();
            stepQueryResponseMock2
                .Setup(m => m.SendXML(AuthenticatedTicket))
                .Returns(expectedResult);
            stepQueryResponseMock2
                .SetupGet(m => m.Name)
                .Returns("Mock2");

            syncManagerMock.Object.RegisterStep(stepQueryResponseMock1.Object);
            syncManagerMock.Object.RegisterStep(stepQueryResponseMock2.Object);

            var result = syncManagerMock.Object.SendRequestXML(guid, null, null, null, 13, 0);

            Assert.AreEqual(expectedResult, result);
            syncManagerMock
                .Protected()
                .Verify("SaveChanges", Times.Once());
            Assert.AreEqual(stepQueryResponseMock2.Object.Name, AuthenticatedTicket.CurrentStep);
        }

        [Test]
        [SetupValidTicket]
        public void SendRequestXML_WithValidTicket_With3Steps()
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
                .Setup(m => m.SendXML(AuthenticatedTicket))
                .Returns((string)null);
            stepQueryResponseMock1
                .SetupGet(m => m.Name)
                .Returns("Mock1");
            var stepQueryResponseMock2 = new Mock<StepQueryResponse>();
            stepQueryResponseMock2
                .Setup(m => m.SendXML(AuthenticatedTicket))
                .Returns((string)null);
            stepQueryResponseMock2
                .SetupGet(m => m.Name)
                .Returns("Mock2");
            var stepQueryResponseMock3 = new Mock<StepQueryResponse>();
            stepQueryResponseMock3
                .Setup(m => m.SendXML(AuthenticatedTicket))
                .Returns(expectedResult);
            stepQueryResponseMock3
                .SetupGet(m => m.Name)
                .Returns("Mock3");

            syncManagerMock.Object.RegisterStep(stepQueryResponseMock1.Object);
            syncManagerMock.Object.RegisterStep(stepQueryResponseMock2.Object);
            syncManagerMock.Object.RegisterStep(stepQueryResponseMock3.Object);

            var result = syncManagerMock.Object.SendRequestXML(guid, null, null, null, 13, 0);

            Assert.AreEqual(expectedResult, result);
            syncManagerMock
                .Protected()
                .Verify("SaveChanges", Times.Once());
            Assert.AreEqual(stepQueryResponseMock3.Object.Name, AuthenticatedTicket.CurrentStep);
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
                .Setup(m => m.SendXML(AuthenticatedTicket))
                .Returns((string)null);
            stepQueryResponseMock1
                .SetupGet(m => m.Name)
                .Returns("Mock1");

            syncManagerMock.Object.RegisterStep(stepQueryResponseMock1.Object);

            var result = syncManagerMock.Object.SendRequestXML(guid, null, null, null, 13, 0);

            Assert.AreEqual(string.Empty, result);
            syncManagerMock
                .Protected()
                .Verify("SaveChanges", Times.Once());
            Assert.AreEqual(null, AuthenticatedTicket.CurrentStep);
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
            var stepQueryResponseMock1 = new Mock<StepQueryResponse>();
            stepQueryResponseMock1
                .Setup(m => m.ReceiveXML(AuthenticatedTicket, null, null, null))
                .Returns(expectedResult);
            stepQueryResponseMock1
                .Setup(m => m.GotoNextStep())
                .Returns(true);
            stepQueryResponseMock1
                .SetupGet(m => m.Name)
                .Returns("Mock1");

            syncManagerMock.Object.RegisterStep(stepQueryResponseMock1.Object);

            var result = syncManagerMock.Object.ReceiveRequestXML(guid, null, null, null);

            Assert.AreEqual(expectedResult, result);
            syncManagerMock
                .Protected()
                .Verify("SaveChanges", Times.Once());
            Assert.AreEqual(null, AuthenticatedTicket.CurrentStep);
        }

        [Test]
        [SetupValidTicket]
        public void ReceiveRequestXML_WithValidTicket_ValidResponseAndGoToStep()
        {
            var guid = Guid.NewGuid().ToString();
            var syncManagerMock = new Mock<SyncManager>(authenticatorMock.Object);
            syncManagerMock
                .Protected()
                .Setup("SaveChanges");

            syncManagerMock.CallBase = true;

            var expectedResult = "step2";
            var stepQueryResponseMock1 = new Mock<StepQueryResponse>();
            stepQueryResponseMock1
                .Setup(m => m.ReceiveXML(AuthenticatedTicket, null, null, null))
                .Returns(0);
            stepQueryResponseMock1
                .Setup(m => m.GotoStep())
                .Returns(expectedResult);
            stepQueryResponseMock1
                .SetupGet(m => m.Name)
                .Returns("Mock1");

            syncManagerMock.Object.RegisterStep(stepQueryResponseMock1.Object);

            var result = syncManagerMock.Object.ReceiveRequestXML(guid, null, null, null);

            syncManagerMock
                .Protected()
                .Verify("SaveChanges", Times.Once());
            Assert.AreEqual(expectedResult, AuthenticatedTicket.CurrentStep);
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
            var stepQueryResponseMock1 = new Mock<StepQueryResponse>();
            stepQueryResponseMock1
                .Setup(m => m.ReceiveXML(AuthenticatedTicket, null, null, null))
                .Returns(expectedResult);
            stepQueryResponseMock1
                .Setup(m => m.GotoNextStep())
                .Returns(false);
            stepQueryResponseMock1
                .SetupGet(m => m.Name)
                .Returns("Mock1");

            syncManagerMock.Object.RegisterStep(stepQueryResponseMock1.Object);

            var result = syncManagerMock.Object.ReceiveRequestXML(guid, null, null, null);

            Assert.AreEqual(expectedResult, result);
            syncManagerMock
                .Protected()
                .Verify("SaveChanges", Times.Once());
            Assert.AreEqual(AuthenticatedTicket.InitialStep, AuthenticatedTicket.CurrentStep);
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
            var stepQueryResponseMock1 = new Mock<StepQueryResponse>();
            stepQueryResponseMock1
                .Setup(m => m.ReceiveXML(AuthenticatedTicket, null, null, null))
                .Returns(expectedResult);
            stepQueryResponseMock1
                .SetupGet(m => m.Name)
                .Returns("Mock1");

            syncManagerMock.Object.RegisterStep(stepQueryResponseMock1.Object);

            var result = syncManagerMock.Object.ReceiveRequestXML(guid, null, null, null);

            Assert.AreEqual(expectedResult, result);
            syncManagerMock
                .Protected()
                .Verify("SaveChanges", Times.Once());
            Assert.AreEqual(AuthenticatedTicket.InitialStep, AuthenticatedTicket.CurrentStep);
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
            var stepQueryResponseMock1 = new Mock<StepQueryResponse>();
            stepQueryResponseMock1
                .Setup(m => m.LastError(AuthenticatedTicket))
                .Returns(expectedResult);
            stepQueryResponseMock1
                .SetupGet(m => m.Name)
                .Returns("Mock1");

            syncManagerMock.Object.RegisterStep(stepQueryResponseMock1.Object);

            var result = syncManagerMock.Object.GetLastError(guid);

            Assert.AreEqual(expectedResult, result);
            syncManagerMock
                .Protected()
                .Verify("SaveChanges", Times.Once());
            Assert.AreEqual(AuthenticatedTicket.InitialStep, AuthenticatedTicket.CurrentStep);
        }

        [Test]
        [SetupValidTicket]
        public void GetLastError_WithValidTicket_WrongVersion()
        {
            var guid = Guid.NewGuid().ToString();
            var syncManagerMock = new Mock<SyncManager>(authenticatorMock.Object);
            syncManagerMock
                .Protected()
                .Setup("SaveChanges");

            var versionValidator = new Mock<IVersionValidator>();
            versionValidator
                .Setup(m => m.IsValidTicket(It.IsAny<string>()))
                .Returns(false);
            syncManagerMock.Object.VersionValidator = versionValidator.Object;

            syncManagerMock.CallBase = true;

            var expectedResult = string.Format("The server requires QbXml version {0}.{1} or higher. Please upgrade QuickBooks.", QbXmlRequest.VERSION.Major, QbXmlRequest.VERSION.Minor);
            var result = syncManagerMock.Object.GetLastError(guid);

            Assert.AreEqual(expectedResult, result);
            syncManagerMock
                .Protected()
                .Verify("SaveChanges", Times.Once());
            Assert.AreEqual(AuthenticatedTicket.InitialStep, AuthenticatedTicket.CurrentStep);
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
            Assert.AreEqual(AuthenticatedTicket.InitialStep, AuthenticatedTicket.CurrentStep);
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

        [Test]
        [SetupValidTicket]
        public void Function_With_Exception()
        {
            var guid = Guid.NewGuid().ToString();
            var syncManagerMock = new Mock<SyncManager>(authenticatorMock.Object);
            syncManagerMock
                .Protected()
                .Setup("OnException", ItExpr.IsAny<AuthenticatedTicket>(), ItExpr.IsAny<Exception>());

            syncManagerMock.CallBase = true;

            var ex = new Exception();
            var expectedResult = -1;
            var stepQueryResponseMock1 = new Mock<StepQueryResponse>();
            stepQueryResponseMock1
                .Setup(m => m.ReceiveXML(AuthenticatedTicket, null, null, null))
                .Throws(ex);

            syncManagerMock.Object.RegisterStep(stepQueryResponseMock1.Object);

            var result = syncManagerMock.Object.ReceiveRequestXML(guid, null, null, null);

            Assert.AreEqual(expectedResult, result);
            syncManagerMock
                .Protected()
                .Verify("OnException", Times.Once(), ItExpr.Is<AuthenticatedTicket>(m => m == AuthenticatedTicket), ItExpr.Is<Exception>(m => m.InnerException == ex));
        }
    }
}