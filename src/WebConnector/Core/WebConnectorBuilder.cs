using Microsoft.Extensions.DependencyInjection;
using System;
using System.ComponentModel;

namespace QbSync.WebConnector.Core
{
    public class WebConnectorBuilder
    {
        public WebConnectorBuilder(IServiceCollection services)
        {
            Services = services ?? throw new ArgumentNullException(nameof(services));
        }

        /// <summary>
        /// Gets the services collection.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public IServiceCollection Services { get; }

        public WebConnectorBuilder WithStep<Request, Response>(ServiceLifetime lifetime = ServiceLifetime.Scoped)
            where Request : IStepQueryRequest
            where Response : IStepQueryResponse
        {
            var requestDescriptor = new ServiceDescriptor(typeof(IStepQueryRequest), typeof(Request), lifetime);
            var responseDescriptor = new ServiceDescriptor(typeof(IStepQueryResponse), typeof(Response), lifetime);

            Services.Add(requestDescriptor);
            Services.Add(responseDescriptor);

            return this;
        }

        public WebConnectorBuilder AddAuthenticator<Authenticator>(ServiceLifetime lifetime = ServiceLifetime.Scoped)
            where Authenticator : IAuthenticator
        {
            var authenticatorDescriptor = new ServiceDescriptor(typeof(IAuthenticator), typeof(Authenticator), lifetime);

            Services.Add(authenticatorDescriptor);

            return this;
        }

        public WebConnectorBuilder WithVersionValidator<VersionValidator>(ServiceLifetime lifetime = ServiceLifetime.Scoped)
            where VersionValidator : IVersionValidator
        {
            var versionValidatorDescriptor = new ServiceDescriptor(typeof(IVersionValidator), typeof(VersionValidator), lifetime);

            Services.Add(versionValidatorDescriptor);

            return this;
        }

        public WebConnectorBuilder WithWebConnectorHandler<WebConnectorHandler>(ServiceLifetime lifetime = ServiceLifetime.Scoped)
            where WebConnectorHandler : IWebConnectorHandler
        {
            var webConnectorHandlerDescriptor = new ServiceDescriptor(typeof(IWebConnectorHandler), typeof(WebConnectorHandler), lifetime);

            Services.Add(webConnectorHandlerDescriptor);

            return this;
        }
    }
}
