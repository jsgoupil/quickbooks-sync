using QbSync.WebConnector.Core;
using System;
using System.Threading.Tasks;

namespace QbSync.WebConnector.Impl
{
    /// <summary>
    /// Handler that does nothing.
    /// </summary>
    public class WebConnectorHandlerNoop : IWebConnectorHandler
    {
        /// <summary>
        /// The connection is closing, the Web Connector will not come back with this ticket.
        /// </summary>
        /// <param name="authenticatedTicket">The ticket.</param>
        /// <returns>Completed Task.</returns>
        public virtual Task CloseConnectionAsync(IAuthenticatedTicket? authenticatedTicket)
        {
            return Task.CompletedTask;
        }

        /// <summary>
        /// Uses the company file path. Return an empty string to use the file that is currently opened.
        /// </summary>
        /// <param name="authenticatedTicket">The ticket.</param>
        /// <returns>Company File or empty string.</returns>
        public virtual Task<string> GetCompanyFileAsync(IAuthenticatedTicket authenticatedTicket)
        {
            return Task.FromResult(string.Empty);
        }

        /// <summary>
        /// Tells the Web Connector to come back later after X seconds.
        /// Returning 0 means to do the work immediately.
        /// </summary>
        /// <param name="authenticatedTicket">The ticket.</param>
        /// <returns>Seconds to wait before reconnecting.</returns>
        public virtual Task<int> GetWaitTimeAsync(IAuthenticatedTicket authenticatedTicket)
        {
            return Task.FromResult(0);
        }

        /// <summary>
        /// Called when any types of exception occur on the server.
        /// </summary>
        /// <param name="authenticatedTicket">The ticket.</param>
        /// <param name="exception">The exception.</param>
        /// <returns>Completed Task.</returns>
        public virtual Task OnExceptionAsync(IAuthenticatedTicket? authenticatedTicket, Exception exception)
        {
            return Task.CompletedTask;
        }

        /// <summary>
        /// Returns the configuration QuickBooks is in. This method is called once per session.
        /// </summary>
        /// <param name="authenticatedTicket">The ticket.</param>
        /// <param name="response">XML data.</param>
        /// <returns>Completed Task.</returns>
        public virtual Task ProcessClientInformationAsync(IAuthenticatedTicket? authenticatedTicket, string response)
        {
            return Task.CompletedTask;
        }
    }
}