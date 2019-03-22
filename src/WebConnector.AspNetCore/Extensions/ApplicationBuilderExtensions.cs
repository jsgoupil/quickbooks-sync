using Microsoft.AspNetCore.Builder;
using QbSync.WebConnector.AspNetCore;
using QbSync.WebConnector.Core;
using SoapCore;
using System;
using System.ServiceModel;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Helper class to attach the SoapCore for WebConnector.
    /// </summary>
    public static class ApplicationBuilderExtensions
    {
        /// <summary>
        /// Start using the middleware with SoapCore, with specific options.
        /// </summary>
        /// <typeparam name="EndPoint">The endpoint handling the WebConnector messages.</typeparam>
        /// <param name="app">AspNetCore ApplicationBuilder.</param>
        /// <param name="configuration">The configuration.</param>
        /// <returns></returns>
        public static IApplicationBuilder UseWebConnector<EndPoint>(
            this IApplicationBuilder app,
            Action<WebConnectorOptions> configuration
        )
        {
            var options = new WebConnectorOptions();
            configuration(options);

            app
                .UseSoapEndpoint<EndPoint>(options.SoapPath, new BasicHttpBinding(), SoapSerializer.XmlSerializer);

            return app;
        }

        /// <summary>
        /// Start using the middleware with SoapCore.
        /// </summary>
        /// <param name="app">AspNetCore ApplicationBuilder.</param>
        /// <param name="configuration">The configuration.</param>
        /// <returns></returns>
        public static IApplicationBuilder UseWebConnector(
            this IApplicationBuilder app,
            Action<WebConnectorOptions> configuration
        )
        {
            return app.UseWebConnector<IQbManager>(configuration);
        }
    }
}
