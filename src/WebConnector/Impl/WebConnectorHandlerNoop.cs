using QbSync.QbXml;
using QbSync.WebConnector.Core;
using System;
using System.Threading.Tasks;

namespace QbSync.WebConnector.Impl
{
    public class WebConnectorHandlerNoop : IWebConnectorHandler
    {
        public virtual Task CloseConnectionAsync(IAuthenticatedTicket authenticatedTicket)
        {
            return Task.CompletedTask;
        }

        public virtual Task<string> GetCompanyFileAsync(IAuthenticatedTicket authenticatedTicket)
        {
            return Task.FromResult(string.Empty);
        }

        public virtual Task<QbXmlResponseOptions> GetOptionsAsync(IAuthenticatedTicket authenticatedTicket)
        {
            return Task.FromResult((QbXmlResponseOptions)null);
        }

        public virtual Task<int> GetWaitTimeAsync(IAuthenticatedTicket authenticatedTicket)
        {
            return Task.FromResult(0);
        }

        public virtual Task OnExceptionAsync(IAuthenticatedTicket authenticatedTicket, Exception exception)
        {
            return Task.CompletedTask;
        }

        public virtual Task ProcessClientInformationAsync(IAuthenticatedTicket authenticatedTicket, string response)
        {
            return Task.CompletedTask;
        }
    }
}