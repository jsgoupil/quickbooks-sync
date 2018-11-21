using Microsoft.AspNetCore.Builder;
using QbSync.WebConnector.Core;
using SoapCore;
using System;
using System.ServiceModel;

namespace QbSync.WebConnector.Extensions
{
    public static class ApplicationBuilderExtensions
    {
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

        public static IApplicationBuilder UseWebConnector(
            this IApplicationBuilder app,
            Action<WebConnectorOptions> configuration
        )
        {
            return app.UseWebConnector<IQbManager>(configuration);
        }
    }
}
