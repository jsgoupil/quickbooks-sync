using Moq;
using Moq.Protected;
using NUnit.Framework;
using QbSync.QbXml;
using QbSync.WebConnector.Asynchronous;
using QbSync.WebConnector.Tests.Asynchronous.Helpers;
using System;
using System.Threading.Tasks;

namespace QbSync.WebConnector.Tests.Asynchronous
{
    [TestFixture]
    class QbManagerTests
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
        public async Task AuthenticateWithInvalidImplementation()
        {
            var syncManagerMock = new Mock<QbManager>(authenticatorMock.Object);
            syncManagerMock
                .Protected()
                .Setup<Task>("OnExceptionAsync", ItExpr.IsAny<AuthenticatedTicket>(), ItExpr.IsAny<Exception>())
                .Returns(Task.FromResult<object>(null));

            syncManagerMock.CallBase = true;
            await syncManagerMock.Object.AuthenticateAsync("user", "password");

            syncManagerMock
                .Protected()
                .Verify("OnExceptionAsync", Times.Once(), ItExpr.Is<AuthenticatedTicket>(m => m == null), ItExpr.Is<Exception>(m => m.Message == "GetAuthenticationFromLogin must return a ticket."));
        }

        [Test]
        public async Task AuthenticateWithWrongCredentials()
        {
            authenticatorMock
                .Setup(m => m.GetAuthenticationFromLoginAsync(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.FromResult(new AuthenticatedTicket
                {
                    Ticket = Guid.NewGuid().ToString(),
                    Authenticated = false
                }));

            var syncManagerMock = new Mock<QbManager>(authenticatorMock.Object);
            syncManagerMock
                .Protected()
                .Setup<Task>("SaveChangesAsync")
                .Returns(Task.FromResult<object>(null));

            syncManagerMock.CallBase = true;
            var result = await syncManagerMock.Object.AuthenticateAsync("user", "password");

            Assert.IsNotEmpty(result[0]);
            Assert.AreEqual("nvu", result[1]);
            Assert.IsEmpty(result[2]);
            Assert.IsEmpty(result[3]);
            syncManagerMock
                .Protected()
                .Verify("SaveChangesAsync", Times.Once());
        }

        [Test]
        [SetupValidTicket]
        public async Task AuthenticateWithValidCredentials()
        {
            var syncManagerMock = new Mock<QbManager>(authenticatorMock.Object);
            syncManagerMock
                .Protected()
                .Setup<Task>("SaveChangesAsync")
                .Returns(Task.FromResult<object>(null));
            syncManagerMock
                .Protected()
                .Setup<int>("GetWaitTime", ItExpr.IsAny<AuthenticatedTicket>())
                .Returns(0);

            syncManagerMock.CallBase = true;
            var result = await syncManagerMock.Object.AuthenticateAsync("user", "password");

            Assert.IsNotEmpty(result[0]);
            Assert.IsEmpty(result[1]);
            Assert.IsEmpty(result[2]);
            Assert.IsEmpty(result[3]);
            syncManagerMock
                .Protected()
                .Verify("SaveChangesAsync", Times.Once());
        }

        [Test]
        [SetupValidTicket]
        public async Task AuthenticateWithValidCredentialsOpenCustomCompanyFile()
        {
            var companyFile = "D:\\file.qbw";

            var syncManagerMock = new Mock<QbManager>(authenticatorMock.Object);
            syncManagerMock
                .Protected()
                .Setup<Task>("SaveChangesAsync")
                .Returns(Task.FromResult<object>(null));
            syncManagerMock
                .Protected()
                .Setup<string>("GetCompanyFile", ItExpr.IsAny<AuthenticatedTicket>())
                .Returns(companyFile);

            syncManagerMock.CallBase = true;
            var result = await syncManagerMock.Object.AuthenticateAsync("user", "password");

            Assert.IsNotEmpty(result[0]);
            Assert.AreEqual(companyFile, result[1]);
            Assert.IsEmpty(result[2]);
            Assert.IsEmpty(result[3]);
            syncManagerMock
                .Protected()
                .Verify("SaveChangesAsync", Times.Once());
        }

        [Test]
        [SetupValidTicket]
        public async Task AuthenticateWithValidCredentialsHasNoWork()
        {
            var timeWait = 60;
            var syncManagerMock = new Mock<QbManager>(authenticatorMock.Object);
            syncManagerMock
                .Protected()
                .Setup<int>("GetWaitTime", ItExpr.IsAny<AuthenticatedTicket>())
                .Returns(timeWait);

            syncManagerMock.CallBase = true;
            var result = await syncManagerMock.Object.AuthenticateAsync("user", "password");

            Assert.IsNotEmpty(result[0]);
            Assert.AreEqual("none", result[1]);
            Assert.AreEqual(timeWait.ToString(), result[2]);
            Assert.IsEmpty(result[3]);
        }

        [Test]
        public void ClientVersionWithValidVersion()
        {
            var syncManager = new QbManager(null);
            var result = syncManager.ClientVersion("2.1.0.30");

            Assert.IsEmpty(result);
        }

        [Test]
        public void ClientVersionWithNewerVersion()
        {
            var syncManager = new QbManager(null);
            var result = syncManager.ClientVersion("2.1.0.31");

            Assert.IsEmpty(result);
        }

        [Test]
        public void ClientVersionWithInvalidVersion()
        {
            var syncManager = new QbManager(null);
            var result = syncManager.ClientVersion("2.1.0.29");

            Assert.AreEqual("W:", result.Substring(0, 2));
        }

        [Test]
        public void ClientVersionWithSpecifiedVersion()
        {
            var specifiedVersion = new Version(1, 1, 1, 1);
            var syncManagerMock = new Mock<QbManager>(null);
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
        public async Task SendRequestXML_WithInvalidTicket()
        {
            var guid = Guid.NewGuid().ToString();
            var syncManagerMock = new Mock<QbManager>(authenticatorMock.Object);
            syncManagerMock
                .Protected()
                .Setup<Task>("SaveChangesAsync")
                .Returns(Task.FromResult<object>(null));

            syncManagerMock.CallBase = true;
            var result = await syncManagerMock.Object.SendRequestXMLAsync(guid, null, null, null, 13, 0);

            Assert.IsEmpty(result);
            syncManagerMock
                .Protected()
                .Verify("SaveChangesAsync", Times.Once());
        }

        [Test]
        [SetupValidTicket]
        public async Task SendRequestXML_WithFirstResponse()
        {
            var guid = Guid.NewGuid().ToString();
            var syncManagerMock = new Mock<QbManager>(authenticatorMock.Object);
            var initial = "INITIAL";
            syncManagerMock
                .Protected()
                .Setup<Task>("ProcessClientInformationAsync", ItExpr.IsAny<AuthenticatedTicket>(), ItExpr.IsAny<string>())
                .Returns(Task.FromResult<object>(null));

            syncManagerMock.CallBase = true;

            await syncManagerMock.Object.SendRequestXMLAsync(guid, initial, null, null, 13, 0);

            syncManagerMock
                .Protected()
                .Verify("ProcessClientInformationAsync", Times.Once(), ItExpr.IsAny<AuthenticatedTicket>(), ItExpr.Is<string>(m => m == initial));
        }

        [Test]
        [SetupValidTicket]
        public async Task SendRequestXML_WithValidTicket_FirstStepHasWork()
        {
            var guid = Guid.NewGuid().ToString();
            var syncManagerMock = new Mock<QbManager>(authenticatorMock.Object);
            syncManagerMock
                .Protected()
                .Setup<Task>("SaveChangesAsync")
                .Returns(Task.FromResult<object>(null));

            syncManagerMock.CallBase = true;

            var versionValidator = new Mock<IVersionValidator>();
            versionValidator
                .Setup(m => m.ValidateVersionAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()))
                .Returns(Task.FromResult(true));
            syncManagerMock.Object.VersionValidator = versionValidator.Object;

            var expectedResult = "abc";
            var IStepQueryResponseMock1 = new Mock<IStepQueryResponse>();
            IStepQueryResponseMock1
                .Setup(m => m.SendXMLAsync(AuthenticatedTicket))
                .Returns(Task.FromResult(expectedResult));
            IStepQueryResponseMock1
                .SetupGet(m => m.Name)
                .Returns("Mock1");

            syncManagerMock.Object.RegisterStep(IStepQueryResponseMock1.Object);

            var result = await syncManagerMock.Object.SendRequestXMLAsync(guid, null, null, null, 13, 0);

            Assert.AreEqual(expectedResult, result);
            syncManagerMock
                .Protected()
                .Verify("SaveChangesAsync", Times.Once());
            Assert.AreEqual(AuthenticatedTicket.InitialStep, AuthenticatedTicket.CurrentStep);
        }

        [Test]
        [SetupValidTicket]
        public async Task SendRequestXML_WithValidTicket_WithOldQbXML()
        {
            var guid = Guid.NewGuid().ToString();
            var syncManagerMock = new Mock<QbManager>(authenticatorMock.Object);
            syncManagerMock
                .Protected()
                .Setup<Task>("SaveChangesAsync")
                .Returns(Task.FromResult<object>(null));

            syncManagerMock.CallBase = true;

            var versionValidator = new Mock<IVersionValidator>();
            versionValidator
                .Setup(m => m.ValidateVersionAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()))
                .Returns(Task.FromResult(false));
            syncManagerMock.Object.VersionValidator = versionValidator.Object;

            var expectedResult = string.Empty;
            var IStepQueryResponseMock1 = new Mock<IStepQueryResponse>();
            IStepQueryResponseMock1
                .Setup(m => m.SendXMLAsync(AuthenticatedTicket))
                .Returns(Task.FromResult("abc"));
            IStepQueryResponseMock1
                .SetupGet(m => m.Name)
                .Returns("Mock1");

            syncManagerMock.Object.RegisterStep(IStepQueryResponseMock1.Object);

            var result = await syncManagerMock.Object.SendRequestXMLAsync(guid, null, null, null, 12, 0);

            Assert.AreEqual(expectedResult, result);
            syncManagerMock
                .Protected()
                .Verify("SaveChangesAsync", Times.Once());
        }

        [Test]
        [SetupValidTicket]
        public async Task SendRequestXML_WithValidTicket_FirstStepHasNoWork()
        {
            var guid = Guid.NewGuid().ToString();

            var syncManagerMock = new Mock<QbManager>(authenticatorMock.Object);
            syncManagerMock
                .Protected()
                .Setup<Task>("SaveChangesAsync")
                .Returns(Task.FromResult<object>(null));

            syncManagerMock.CallBase = true;

            var expectedResult = "abc";
            var IStepQueryResponseMock1 = new Mock<IStepQueryResponse>();
            IStepQueryResponseMock1
                .Setup(m => m.SendXMLAsync(AuthenticatedTicket))
                .Returns(Task.FromResult((string)null));
            IStepQueryResponseMock1
                .SetupGet(m => m.Name)
                .Returns("Mock1");
            var IStepQueryResponseMock2 = new Mock<IStepQueryResponse>();
            IStepQueryResponseMock2
                .Setup(m => m.SendXMLAsync(AuthenticatedTicket))
                .Returns(Task.FromResult(expectedResult));
            IStepQueryResponseMock2
                .SetupGet(m => m.Name)
                .Returns("Mock2");

            syncManagerMock.Object.RegisterStep(IStepQueryResponseMock1.Object);
            syncManagerMock.Object.RegisterStep(IStepQueryResponseMock2.Object);

            var result = await syncManagerMock.Object.SendRequestXMLAsync(guid, null, null, null, 13, 0);

            Assert.AreEqual(expectedResult, result);
            syncManagerMock
                .Protected()
                .Verify("SaveChangesAsync", Times.Once());
            Assert.AreEqual(IStepQueryResponseMock2.Object.Name, AuthenticatedTicket.CurrentStep);
        }

        [Test]
        [SetupValidTicket]
        public async Task SendRequestXML_WithValidTicket_With3Steps()
        {
            var guid = Guid.NewGuid().ToString();

            var syncManagerMock = new Mock<QbManager>(authenticatorMock.Object);
            syncManagerMock
                .Protected()
                .Setup<Task>("SaveChangesAsync")
                .Returns(Task.FromResult<object>(null));

            syncManagerMock.CallBase = true;

            var expectedResult = "abc";
            var IStepQueryResponseMock1 = new Mock<IStepQueryResponse>();
            IStepQueryResponseMock1
                .Setup(m => m.SendXMLAsync(AuthenticatedTicket))
                .Returns(Task.FromResult((string)null));
            IStepQueryResponseMock1
                .SetupGet(m => m.Name)
                .Returns("Mock1");
            var IStepQueryResponseMock2 = new Mock<IStepQueryResponse>();
            IStepQueryResponseMock2
                .Setup(m => m.SendXMLAsync(AuthenticatedTicket))
                .Returns(Task.FromResult((string)null));
            IStepQueryResponseMock2
                .SetupGet(m => m.Name)
                .Returns("Mock2");
            var IStepQueryResponseMock3 = new Mock<IStepQueryResponse>();
            IStepQueryResponseMock3
                .Setup(m => m.SendXMLAsync(AuthenticatedTicket))
                .Returns(Task.FromResult(expectedResult));
            IStepQueryResponseMock3
                .SetupGet(m => m.Name)
                .Returns("Mock3");

            syncManagerMock.Object.RegisterStep(IStepQueryResponseMock1.Object);
            syncManagerMock.Object.RegisterStep(IStepQueryResponseMock2.Object);
            syncManagerMock.Object.RegisterStep(IStepQueryResponseMock3.Object);

            var result = await syncManagerMock.Object.SendRequestXMLAsync(guid, null, null, null, 13, 0);

            Assert.AreEqual(expectedResult, result);
            syncManagerMock
                .Protected()
                .Verify("SaveChangesAsync", Times.Once());
            Assert.AreEqual(IStepQueryResponseMock3.Object.Name, AuthenticatedTicket.CurrentStep);
        }

        [Test]
        [SetupValidTicket]
        public async Task SendRequestXML_WithValidTicket_NoStepsHaveWork()
        {
            var guid = Guid.NewGuid().ToString();
            var syncManagerMock = new Mock<QbManager>(authenticatorMock.Object);
            syncManagerMock
                .Protected()
                .Setup<Task>("SaveChangesAsync")
                .Returns(Task.FromResult<object>(null));

            syncManagerMock.CallBase = true;

            var IStepQueryResponseMock1 = new Mock<IStepQueryResponse>();
            IStepQueryResponseMock1
                .Setup(m => m.SendXMLAsync(AuthenticatedTicket))
                .Returns(Task.FromResult((string)null));
            IStepQueryResponseMock1
                .SetupGet(m => m.Name)
                .Returns("Mock1");

            syncManagerMock.Object.RegisterStep(IStepQueryResponseMock1.Object);

            var result = await syncManagerMock.Object.SendRequestXMLAsync(guid, null, null, null, 13, 0);

            Assert.AreEqual(string.Empty, result);
            syncManagerMock
                .Protected()
                .Verify("SaveChangesAsync", Times.Once());
            Assert.AreEqual(null, AuthenticatedTicket.CurrentStep);
        }

        [Test]
        [SetupInvalidTicket]
        public async Task ReceiveRequestXML_WithInvalidTicket()
        {
            var guid = Guid.NewGuid().ToString();
            var syncManagerMock = new Mock<QbManager>(authenticatorMock.Object);
            syncManagerMock
                .Protected()
                .Setup<Task>("SaveChangesAsync")
                .Returns(Task.FromResult<object>(null));

            syncManagerMock.CallBase = true;
            var result = await syncManagerMock.Object.ReceiveRequestXMLAsync(guid, null, null, null);

            Assert.AreEqual(-1, result);
            syncManagerMock
                .Protected()
                .Verify("SaveChangesAsync", Times.Once());
        }

        [Test]
        [SetupValidTicket]
        public async Task ReceiveRequestXML_WithValidTicket_ValidResponseAndGoToNext()
        {
            var guid = Guid.NewGuid().ToString();
            var syncManagerMock = new Mock<QbManager>(authenticatorMock.Object);
            syncManagerMock
                .Protected()
                .Setup<Task>("SaveChangesAsync")
                .Returns(Task.FromResult<object>(null));

            syncManagerMock.CallBase = true;

            var expectedResult = 0;
            var IStepQueryResponseMock1 = new Mock<IStepQueryResponse>();
            IStepQueryResponseMock1
                .Setup(m => m.ReceiveXMLAsync(AuthenticatedTicket, null, null, null))
                .Returns(Task.FromResult(expectedResult));
            IStepQueryResponseMock1
                .Setup(m => m.GotoNextStepAsync())
                .Returns(Task.FromResult(true));
            IStepQueryResponseMock1
                .SetupGet(m => m.Name)
                .Returns("Mock1");

            syncManagerMock.Object.RegisterStep(IStepQueryResponseMock1.Object);

            var result = await syncManagerMock.Object.ReceiveRequestXMLAsync(guid, null, null, null);

            Assert.AreEqual(expectedResult, result);
            syncManagerMock
                .Protected()
                .Verify("SaveChangesAsync", Times.Once());
            Assert.AreEqual(null, AuthenticatedTicket.CurrentStep);
        }

        [Test]
        [SetupValidTicket]
        public async Task ReceiveRequestXML_WithValidTicket_ValidResponseAndGoToStep()
        {
            var guid = Guid.NewGuid().ToString();
            var syncManagerMock = new Mock<QbManager>(authenticatorMock.Object);
            syncManagerMock
                .Protected()
                .Setup<Task>("SaveChangesAsync")
                .Returns(Task.FromResult<object>(null));

            syncManagerMock.CallBase = true;

            var expectedResult = "step2";
            var IStepQueryResponseMock1 = new Mock<IStepQueryResponse>();
            IStepQueryResponseMock1
                .Setup(m => m.ReceiveXMLAsync(AuthenticatedTicket, null, null, null))
                .Returns(Task.FromResult(0));
            IStepQueryResponseMock1
                .Setup(m => m.GotoStepAsync())
                .Returns(Task.FromResult(expectedResult));
            IStepQueryResponseMock1
                .SetupGet(m => m.Name)
                .Returns("Mock1");

            syncManagerMock.Object.RegisterStep(IStepQueryResponseMock1.Object);

            await syncManagerMock.Object.ReceiveRequestXMLAsync(guid, null, null, null);

            syncManagerMock
                .Protected()
                .Verify("SaveChangesAsync", Times.Once());
            Assert.AreEqual(expectedResult, AuthenticatedTicket.CurrentStep);
        }

        [Test]
        [SetupValidTicket]
        public async Task ReceiveRequestXML_WithValidTicket_ValidResponseAndAndStaySameStep()
        {
            var guid = Guid.NewGuid().ToString();
            var syncManagerMock = new Mock<QbManager>(authenticatorMock.Object);
            syncManagerMock
                .Protected()
                .Setup<Task>("SaveChangesAsync")
                .Returns(Task.FromResult<object>(null));

            syncManagerMock.CallBase = true;

            var expectedResult = 0;
            var IStepQueryResponseMock1 = new Mock<IStepQueryResponse>();
            IStepQueryResponseMock1
                .Setup(m => m.ReceiveXMLAsync(AuthenticatedTicket, null, null, null))
                .Returns(Task.FromResult(expectedResult));
            IStepQueryResponseMock1
                .Setup(m => m.GotoNextStepAsync())
                .Returns(Task.FromResult(false));
            IStepQueryResponseMock1
                .SetupGet(m => m.Name)
                .Returns("Mock1");

            syncManagerMock.Object.RegisterStep(IStepQueryResponseMock1.Object);

            var result = await syncManagerMock.Object.ReceiveRequestXMLAsync(guid, null, null, null);

            Assert.AreEqual(expectedResult, result);
            syncManagerMock
                .Protected()
                .Verify("SaveChangesAsync", Times.Once());
            Assert.AreEqual(AuthenticatedTicket.InitialStep, AuthenticatedTicket.CurrentStep);
        }

        [Test]
        [SetupValidTicket]
        public async Task ReceiveRequestXML_WithValidTicket_InvalidResponse()
        {
            var guid = Guid.NewGuid().ToString();
            var syncManagerMock = new Mock<QbManager>(authenticatorMock.Object);
            syncManagerMock
                .Protected()
                .Setup<Task>("SaveChangesAsync")
                .Returns(Task.FromResult<object>(null));

            syncManagerMock.CallBase = true;

            var expectedResult = -1;
            var IStepQueryResponseMock1 = new Mock<IStepQueryResponse>();
            IStepQueryResponseMock1
                .Setup(m => m.ReceiveXMLAsync(AuthenticatedTicket, null, null, null))
                .Returns(Task.FromResult(expectedResult));
            IStepQueryResponseMock1
                .SetupGet(m => m.Name)
                .Returns("Mock1");

            syncManagerMock.Object.RegisterStep(IStepQueryResponseMock1.Object);

            var result = await syncManagerMock.Object.ReceiveRequestXMLAsync(guid, null, null, null);

            Assert.AreEqual(expectedResult, result);
            syncManagerMock
                .Protected()
                .Verify("SaveChangesAsync", Times.Once());
            Assert.AreEqual(AuthenticatedTicket.InitialStep, AuthenticatedTicket.CurrentStep);
        }

        [Test]
        [SetupInvalidTicket]
        public async Task GetLastError_WithInvalidTicket()
        {
            var guid = Guid.NewGuid().ToString();
            var syncManagerMock = new Mock<QbManager>(authenticatorMock.Object);
            syncManagerMock
                .Protected()
                .Setup<Task>("SaveChangesAsync")
                .Returns(Task.FromResult<object>(null));

            syncManagerMock.CallBase = true;
            var result = await syncManagerMock.Object.GetLastErrorAsync(guid);

            Assert.IsEmpty(result);
            syncManagerMock
                .Protected()
                .Verify("SaveChangesAsync", Times.Once());
        }

        [Test]
        [SetupValidTicket]
        public async Task GetLastError_WithValidTicket_ValidResponse()
        {
            var guid = Guid.NewGuid().ToString();
            var syncManagerMock = new Mock<QbManager>(authenticatorMock.Object);
            syncManagerMock
                .Protected()
                .Setup<Task>("SaveChangesAsync")
                .Returns(Task.FromResult<object>(null));

            syncManagerMock.CallBase = true;

            var expectedResult = "abc";
            var IStepQueryResponseMock1 = new Mock<IStepQueryResponse>();
            IStepQueryResponseMock1
                .Setup(m => m.LastErrorAsync(AuthenticatedTicket))
                .Returns(Task.FromResult(expectedResult));
            IStepQueryResponseMock1
                .SetupGet(m => m.Name)
                .Returns("Mock1");

            syncManagerMock.Object.RegisterStep(IStepQueryResponseMock1.Object);

            var result = await syncManagerMock.Object.GetLastErrorAsync(guid);

            Assert.AreEqual(expectedResult, result);
            syncManagerMock
                .Protected()
                .Verify("SaveChangesAsync", Times.Once());
            Assert.AreEqual(AuthenticatedTicket.InitialStep, AuthenticatedTicket.CurrentStep);
        }

        [Test]
        [SetupValidTicket]
        public async Task GetLastError_WithValidTicket_WrongVersion()
        {
            var guid = Guid.NewGuid().ToString();
            var syncManagerMock = new Mock<QbManager>(authenticatorMock.Object);
            syncManagerMock
                .Protected()
                .Setup<Task>("SaveChangesAsync")
                .Returns(Task.FromResult<object>(null));

            var versionValidator = new Mock<IVersionValidator>();
            versionValidator
                .Setup(m => m.IsValidTicketAsync(It.IsAny<string>()))
                .Returns(Task.FromResult(false));
            syncManagerMock.Object.VersionValidator = versionValidator.Object;

            syncManagerMock.CallBase = true;

            var expectedResult = string.Format("The server requires QbXml version {0}.{1} or higher. Please upgrade QuickBooks.", QbXmlRequest.VERSION.Major, QbXmlRequest.VERSION.Minor);
            var result = await syncManagerMock.Object.GetLastErrorAsync(guid);

            Assert.AreEqual(expectedResult, result);
            syncManagerMock
                .Protected()
                .Verify("SaveChangesAsync", Times.Once());
            Assert.AreEqual(AuthenticatedTicket.InitialStep, AuthenticatedTicket.CurrentStep);
        }

        [Test]
        [SetupInvalidTicket]
        public async Task ConnectionError_WithInvalidTicket()
        {
            var guid = Guid.NewGuid().ToString();
            var syncManagerMock = new Mock<QbManager>(authenticatorMock.Object);
            syncManagerMock
                .Protected()
                .Setup<Task>("SaveChangesAsync")
                .Returns(Task.FromResult<object>(null));

            syncManagerMock.CallBase = true;
            var result = await syncManagerMock.Object.ConnectionErrorAsync(guid, null, null);

            Assert.AreEqual("done", result);
            syncManagerMock
                .Protected()
                .Verify("SaveChangesAsync", Times.Once());
        }

        [Test]
        [SetupValidTicket]
        public async Task ConnectionError_WithValidTicket_ValidResponse()
        {
            var guid = Guid.NewGuid().ToString();
            var syncManagerMock = new Mock<QbManager>(authenticatorMock.Object);
            syncManagerMock
                .Protected()
                .Setup<Task>("SaveChangesAsync")
                .Returns(Task.FromResult<object>(null));

            syncManagerMock.CallBase = true;
            var result = await syncManagerMock.Object.ConnectionErrorAsync(guid, null, null);

            Assert.AreEqual("done", result);
            syncManagerMock
                .Protected()
                .Verify("SaveChangesAsync", Times.Once());
            Assert.AreEqual(AuthenticatedTicket.InitialStep, AuthenticatedTicket.CurrentStep);
        }

        [Test]
        [SetupInvalidTicket]
        public async Task CloseConnection_WithInvalidTicket()
        {
            var guid = Guid.NewGuid().ToString();

            var syncManagerMock = new Mock<QbManager>(authenticatorMock.Object);
            syncManagerMock
                .Protected()
                .Setup<Task>("SaveChangesAsync")
                .Returns(Task.FromResult<object>(null));

            syncManagerMock.CallBase = true;
            var result = await syncManagerMock.Object.CloseConnectionAsync(guid);

            Assert.AreEqual("Invalid Ticket", result);
            syncManagerMock
                .Protected()
                .Verify("SaveChangesAsync", Times.Once());
        }

        [Test]
        [SetupValidTicket]
        public async Task CloseConnection_WithValidTicket_ValidResponse()
        {
            var guid = Guid.NewGuid().ToString();
            var syncManagerMock = new Mock<QbManager>(authenticatorMock.Object);
            syncManagerMock
                .Protected()
                .Setup<Task>("SaveChangesAsync")
                .Returns(Task.FromResult<object>(null));

            syncManagerMock.CallBase = true;
            var result = await syncManagerMock.Object.CloseConnectionAsync(guid);

            Assert.AreEqual("Sync Completed", result);
            syncManagerMock
                .Protected()
                .Verify("SaveChangesAsync", Times.Once());
        }

        [Test]
        [SetupValidTicket]
        [Platform(Exclude = "Mono", Reason = "Mono doesn't like the throw on async, so it's not handled properly.")]
        public async Task Function_With_Exception()
        {
            var guid = Guid.NewGuid().ToString();
            var syncManagerMock = new Mock<QbManager>(authenticatorMock.Object);
            syncManagerMock
                .Protected()
                .Setup<Task>("OnExceptionAsync", ItExpr.IsAny<AuthenticatedTicket>(), ItExpr.IsAny<Exception>())
                .Returns(Task.FromResult<object>(null));

            syncManagerMock.CallBase = true;

            var ex = new Exception();
            var expectedResult = -1;
            var IStepQueryResponseMock1 = new Mock<IStepQueryResponse>();
            IStepQueryResponseMock1
                .Setup(m => m.ReceiveXMLAsync(AuthenticatedTicket, null, null, null))
                .Throws(ex);

            syncManagerMock.Object.RegisterStep(IStepQueryResponseMock1.Object);

            var result = await syncManagerMock.Object.ReceiveRequestXMLAsync(guid, null, null, null);

            Assert.AreEqual(expectedResult, result);

            syncManagerMock
                .Protected()
                .Verify("OnExceptionAsync", Times.Once(), ItExpr.Is<AuthenticatedTicket>(m => m == AuthenticatedTicket), ItExpr.Is<Exception>(m => m.InnerException == ex));
        }
    }
}