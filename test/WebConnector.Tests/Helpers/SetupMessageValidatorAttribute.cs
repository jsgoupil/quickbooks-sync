using Moq;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using QbSync.WebConnector.Tests.Impl;

namespace QbSync.WebConnector.Tests.Helpers
{
    class SetupMessageValidatorAttribute : TestActionAttribute
    {
        public SetupMessageValidatorAttribute()
        {
            ValidMessage = true;
        }

        public bool ValidMessage { get; set; }

        public override void BeforeTest(ITest testDetails)
        {
            if (testDetails.Fixture is QbManagerTests baseTests)
            {
                baseTests.messageValidatorMock
                    .Setup(m => m.ValidateMessageAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()))
                    .ReturnsAsync(ValidMessage);

                baseTests.messageValidatorMock
                    .Setup(m => m.IsValidTicketAsync(It.IsAny<string>()))
                    .ReturnsAsync(ValidMessage);
            }

            base.BeforeTest(testDetails);
        }
    }
}
