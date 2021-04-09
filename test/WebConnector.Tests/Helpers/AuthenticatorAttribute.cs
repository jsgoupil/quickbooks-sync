using Moq;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using QbSync.WebConnector.Core;
using QbSync.WebConnector.Tests.Impl;
using System.Threading.Tasks;

namespace QbSync.WebConnector.Tests.Helpers
{
    class AuthenticatorAttribute : TestActionAttribute
    {
        public IAuthenticatedTicket AuthenticatedTicket
        {
            get;
            set;
        }

        public override void BeforeTest(ITest testDetails)
        {
            if (testDetails.Fixture is QbManagerTests baseTests)
            {
                baseTests.AuthenticatedTicket = AuthenticatedTicket;

                baseTests.authenticatorMock
                    .Setup(m => m.GetAuthenticationFromLoginAsync(It.IsAny<string>(), It.IsAny<string>()))
                    .Returns(Task.FromResult(AuthenticatedTicket));

                baseTests.authenticatorMock
                    .Setup(m => m.GetAuthenticationFromTicketAsync(It.IsAny<string>()))
                    .Returns(Task.FromResult(AuthenticatedTicket));
            }

            base.BeforeTest(testDetails);
        }
    }
}
