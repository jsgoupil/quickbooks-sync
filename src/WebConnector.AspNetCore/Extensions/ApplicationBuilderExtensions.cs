using Microsoft.AspNetCore.Builder;
using QbSync.WebConnector.Core;
using SoapCore;
using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Xml;

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
                .UseSoapEndpoint(
                    typeof(EndPoint),
                    options.SoapPath,
                    new SoapEncoderOptions()
                    {
                        ReaderQuotas = XmlDictionaryReaderQuotas.Max,
                        WriteEncoding = Encoding.UTF8,
                        MessageVersion = MessageVersion.Soap11
                    },
                    SoapSerializer.XmlSerializer,
                    caseInsensitivePath: false,
                    soapModelBounder: null,
                    wsdlFileOptions: null,
                    indentXml: true,
                    omitXmlDeclaration: false
                );

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
