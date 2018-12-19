using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using QbSync.WebConnector.Core;
using QbSync.WebConnector.Impl;
using System;

namespace QbSync.WebConnector.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddWebConnector(this IServiceCollection services, Action<WebConnectorBuilder> configuration)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            services.TryAddScoped<IAuthenticator, AuthenticatorRequired>();
            services.TryAddScoped<IMessageValidator, MessageValidatorNoop>();
            services.TryAddScoped<IWebConnectorHandler, WebConnectorHandlerNoop>();
            services.TryAddScoped<IQbManager, QbManager>();
            services.TryAddScoped<IWebConnectorQwc, WebConnectorQwc>();

            configuration(new WebConnectorBuilder(services));

            return services;
        }
    }
}
