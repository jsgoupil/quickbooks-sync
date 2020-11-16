namespace QbSync.WebConnector.Core
{
    /// <summary>
    /// An object which will keep the state of what is happening with the Web Connector session.
    /// </summary>
    public interface IAuthenticatedTicket
    {
        /// <summary>
        /// Ticket exchanged with the WebConnector. It acts as a session identifier.
        /// </summary>
        string Ticket { get; set; }

        /// <summary>
        /// State indicating what to exchange with the Web Connector.
        /// </summary>
        string CurrentStep { get; set; }

        /// <summary>
        /// Simple boolean indicating if the ticket is authenticated.
        /// </summary>
        bool Authenticated { get; set; }
    }
}
