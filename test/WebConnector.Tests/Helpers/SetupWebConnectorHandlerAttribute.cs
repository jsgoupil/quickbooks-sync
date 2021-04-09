using Moq;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using QbSync.WebConnector.Core;
using QbSync.WebConnector.Tests.Impl;

namespace QbSync.WebConnector.Tests.Helpers
{
    class SetupWebConnectorHandlerAttribute : TestActionAttribute
    {
        public SetupWebConnectorHandlerAttribute()
        {
            CompanyFile = string.Empty;
            WaitTime = 0;
        }

        public string CompanyFile { get; set; }
        public int WaitTime { get; set; }

        public override void BeforeTest(ITest testDetails)
        {
            if (testDetails.Fixture is QbManagerTests baseTests)
            {
                baseTests.webConnectorHandlerMock
                    .Setup(m => m.GetCompanyFileAsync(It.IsAny<IAuthenticatedTicket>()))
                    .ReturnsAsync(CompanyFile);

                baseTests.webConnectorHandlerMock
                    .Setup(m => m.GetWaitTimeAsync(It.IsAny<IAuthenticatedTicket>()))
                    .ReturnsAsync(WaitTime);
            }

            base.BeforeTest(testDetails);
        }
    }
}
