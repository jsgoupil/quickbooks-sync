using Microsoft.Extensions.DependencyInjection;
using QbSync.WebConnector.Core;
using System;
using System.ComponentModel;

namespace QbSync.WebConnector.AspNetCore
{
    /// <summary>
    /// Controls the injection of steps, authentication, etc.
    /// </summary>
    public class WebConnectorBuilder
    {
        /// <summary>
        /// Cosntructs a WebConnectorBuilder.
        /// </summary>
        /// <param name="services">Service Collection.</param>
        public WebConnectorBuilder(IServiceCollection services)
        {
            Services = services ?? throw new ArgumentNullException(nameof(services));
        }

        /// <summary>
        /// Gets the services collection.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public IServiceCollection Services { get; }

        /// <summary>
        /// Adds a step for QuickBooks to process.
        /// The order matters.
        /// </summary>
        /// <typeparam name="Request">A class handling the request.</typeparam>
        /// <typeparam name="Response">A class handling the response.</typeparam>
        /// <param name="lifetime">Optional lifetime of the instance.</param>
        /// <returns>Itself. Chainable.</returns>
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

        /// <summary>
        /// Adds an authenticator.
        /// </summary>
        /// <typeparam name="Authenticator">A class handling the authentication.</typeparam>
        /// <param name="lifetime">Optional lifetime of the instance.</param>
        /// <returns>Itself. Chainable.</returns>
        public WebConnectorBuilder AddAuthenticator<Authenticator>(ServiceLifetime lifetime = ServiceLifetime.Scoped)
            where Authenticator : IAuthenticator
        {
            var authenticatorDescriptor = new ServiceDescriptor(typeof(IAuthenticator), typeof(Authenticator), lifetime);

            Services.Add(authenticatorDescriptor);

            return this;
        }

        /// <summary>
        /// Adds a message validator.
        /// </summary>
        /// <typeparam name="MessageValidator">A class handling the validation of messages.</typeparam>
        /// <param name="lifetime">Optional lifetime of the instance.</param>
        /// <returns>Itself. Chainable.</returns>
        public WebConnectorBuilder WithMessageValidator<MessageValidator>(ServiceLifetime lifetime = ServiceLifetime.Scoped)
            where MessageValidator : IMessageValidator
        {
            var messageValidatorDescriptor = new ServiceDescriptor(typeof(IMessageValidator), typeof(MessageValidator), lifetime);

            Services.Add(messageValidatorDescriptor);

            return this;
        }

        /// <summary>
        /// Adds a web connector handler.
        /// </summary>
        /// <typeparam name="WebConnectorHandler">A class handling the web connector messages.</typeparam>
        /// <param name="lifetime">Optional lifetime of the instance.</param>
        /// <returns>Itself. Chainable.</returns>
        public WebConnectorBuilder WithWebConnectorHandler<WebConnectorHandler>(ServiceLifetime lifetime = ServiceLifetime.Scoped)
            where WebConnectorHandler : IWebConnectorHandler
        {
            var webConnectorHandlerDescriptor = new ServiceDescriptor(typeof(IWebConnectorHandler), typeof(WebConnectorHandler), lifetime);

            Services.Add(webConnectorHandlerDescriptor);

            return this;
        }
    }
}
