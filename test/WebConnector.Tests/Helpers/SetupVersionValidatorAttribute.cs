using Moq;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using QbSync.WebConnector.Tests.Impl;

namespace QbSync.WebConnector.Tests.Helpers
{
    class SetupVersionValidatorAttribute : TestActionAttribute
    {
        public SetupVersionValidatorAttribute()
        {
            ValidVersion = true;
        }

        public bool ValidVersion { get; set; }

        public override void BeforeTest(ITest testDetails)
        {
            var baseTests = testDetails.Fixture as QbManagerTests;
            if (baseTests != null)
            {
                baseTests.versionValidatorMock
                    .Setup(m => m.ValidateVersionAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()))
                    .ReturnsAsync(ValidVersion);

                baseTests.versionValidatorMock
                    .Setup(m => m.IsValidTicketAsync(It.IsAny<string>()))
                    .ReturnsAsync(ValidVersion);
            }

            base.BeforeTest(testDetails);
        }
    }
}
