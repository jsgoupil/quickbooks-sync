using Moq;
using NUnit.Framework;

namespace QbSync.WebConnector.Tests.Synchronous.Helpers
{
    class AuthenticatorAttribute : TestActionAttribute
    {
        public AuthenticatedTicket AuthenticatedTicket
        {
            get;
            set;
        }

        public override void BeforeTest(TestDetails testDetails)
        {
            var baseTests = testDetails.Fixture as QbManagerTests;
            if (baseTests != null)
            {
                baseTests.AuthenticatedTicket = AuthenticatedTicket;

                baseTests.authenticatorMock
                    .Setup(m => m.GetAuthenticationFromLogin(It.IsAny<string>(), It.IsAny<string>()))
                    .Returns(AuthenticatedTicket);

                baseTests.authenticatorMock
                    .Setup(m => m.GetAuthenticationFromTicket(It.IsAny<string>()))
                    .Returns(AuthenticatedTicket);
            }

            base.BeforeTest(testDetails);
        }
    }
}
