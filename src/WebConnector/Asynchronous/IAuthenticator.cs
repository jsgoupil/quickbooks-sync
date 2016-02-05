using System.Threading.Tasks;

namespace QbSync.WebConnector.Asynchronous
{
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
        Task<AuthenticatedTicket> GetAuthenticationFromLoginAsync(string login, string password);

        /// <summary>
        /// Returns the AuthenticatedTicket based on the ticket string.
        /// Returns null if the ticket is invalid.
        /// You should return null if the connection has been closed. It is considered an invalid ticket.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <returns>The AuthenticatedTicket if found, null if invalid ticket.</returns>
        Task<AuthenticatedTicket> GetAuthenticationFromTicketAsync(string ticket);
    }
}
