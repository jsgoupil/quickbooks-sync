using System;
using System.Threading.Tasks;

namespace QbSync.WebConnector.Core
{
    /// <summary>
    /// Interface used to handle some messages between the WebConnector and your server.
    /// </summary>
    public interface IWebConnectorHandler
    {
        /// <summary>
        /// Returns the configuration QuickBooks is in. This method is called once per session.
        /// </summary>
        /// <param name="authenticatedTicket">The ticket.</param>
        /// <param name="response">XML data.</param>
        /// <returns>Completed Task.</returns>
        Task ProcessClientInformationAsync(IAuthenticatedTicket? authenticatedTicket, string response);

        /// <summary>
        /// Called when any types of exception occur on the server.
        /// </summary>
        /// <param name="authenticatedTicket">The ticket.</param>
        /// <param name="exception">The exception.</param>
        /// <returns>Completed Task.</returns>
        Task OnExceptionAsync(IAuthenticatedTicket? authenticatedTicket, Exception exception);

        /// <summary>
        /// Tells the Web Connector to come back later after X seconds.
        /// Returning 0 means to do the work immediately.
        /// </summary>
        /// <param name="authenticatedTicket">The ticket.</param>
        /// <returns>Seconds to wait before reconnecting.</returns>
        Task<int> GetWaitTimeAsync(IAuthenticatedTicket authenticatedTicket);

        /// <summary>
        /// Uses the company file path. Return an empty string to use the file that is currently opened.
        /// </summary>
        /// <param name="authenticatedTicket">The ticket.</param>
        /// <returns>Company File or empty string.</returns>
        Task<string> GetCompanyFileAsync(IAuthenticatedTicket authenticatedTicket);

        /// <summary>
        /// The connection is closing, the Web Connector will not come back with this ticket.
        /// </summary>
        /// <param name="authenticatedTicket">The ticket.</param>
        /// <returns>Completed Task.</returns>
        Task CloseConnectionAsync(IAuthenticatedTicket? authenticatedTicket);
    }
}
