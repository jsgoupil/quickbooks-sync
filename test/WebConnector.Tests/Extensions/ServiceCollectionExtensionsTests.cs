using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using QbSync.WebConnector.AspNetCore;
using QbSync.WebConnector.Core;
using QbSync.WebConnector.Impl;
using System;
using System.Linq;

namespace QbSync.WebConnector.Tests.Extensions
{
    [TestFixture]
    class ServiceCollectionExtensionsTests
    {
        [Test]
        public void AddWebConnector_ThrowsAnExceptionForNullConfiguration()
        {
            // Arrange
            var services = new ServiceCollection();

            // Act and assert
            var exception = Assert.Throws<ArgumentNullException>(() => services.AddWebConnector(configuration: null));

            Assert.AreEqual("configuration", exception.ParamName);
        }

        [TestCase(typeof(IAuthenticator), typeof(AuthenticatorRequired))]
        [TestCase(typeof(IMessageValidator), typeof(MessageValidatorNoop))]
        [TestCase(typeof(IWebConnectorHandler), typeof(WebConnectorHandlerNoop))]
        [TestCase(typeof(IQbManager), typeof(QbManager))]
        [TestCase(typeof(IWebConnectorQwc), typeof(WebConnectorQwc))]
        public void AddWebConnector_RegistersDefaultServices(Type serviceType, Type implementationType)
        {
            // Arrange
            var called = false;
            var services = new ServiceCollection();

            // Act
            services.AddWebConnector(_ => {
                called = true;
            });

            // Assert
            Assert.IsTrue(services.Any(m => m.ServiceType == serviceType && m.ImplementationType == implementationType));
            Assert.IsTrue(called);
        }
    }
}
