using Moq;
using NUnit.Framework;
using QbSync.WebConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QbSync.WebConnector.Tests.Helpers
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
            var baseTests = testDetails.Fixture as SyncManagerTests;
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
