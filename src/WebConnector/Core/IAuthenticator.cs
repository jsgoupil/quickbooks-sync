using System.Threading.Tasks;

namespace QbSync.WebConnector.Core
{
    /// <summary>
    /// An authenticator will keep the state in your database of what is happening with the Web Connector session.
    /// </summary>
    public interface IAuthenticator
    {
        /// <summary>
        /// Returns an Authenticated ticket based on the login and password.
        /// You must return an authenticated ticket containing a ticket even
        /// if the login/password combination is incorrect. Leave the Authenticated
        /// value to false.
        /// </summary>
        /// <param name="login">Login.</param>
        /// <param name="password">Password.</param>
        /// <returns>Always an AuthenticatedTicket.</returns>
        Task<IAuthenticatedTicket?> GetAuthenticationFromLoginAsync(string login, string password);

        /// <summary>
        /// Returns the AuthenticatedTicket based on the ticket string.
        /// Returns null if the ticket is invalid.
        /// You should return null if the connection has been closed. It is considered an invalid ticket.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <returns>The AuthenticatedTicket if found, null if invalid ticket.</returns>
        Task<IAuthenticatedTicket?> GetAuthenticationFromTicketAsync(string ticket);

        /// <summary>
        /// Saves the ticket in the database for future retrieval.
        /// </summary>
        /// <param name="ticket">The ticket</param>
        /// <returns>Completed Task.</returns>
        Task SaveTicketAsync(IAuthenticatedTicket ticket);
    }
}
