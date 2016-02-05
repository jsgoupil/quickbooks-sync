using Moq;
using NUnit.Framework;
using System.Threading.Tasks;

namespace QbSync.WebConnector.Tests.Asynchronous.Helpers
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
