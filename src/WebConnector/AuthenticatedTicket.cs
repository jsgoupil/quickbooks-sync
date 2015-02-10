namespace QbSync.WebConnector
{
    public class AuthenticatedTicket
    {
        /// <summary>
        /// Ticket exchanged with the WebConnector. It acts as a session identifier.
        /// </summary>
        public virtual string Ticket { get; set; }

        /// <summary>
        /// State indicating what to exchange with the Web Connector.
        /// </summary>
        public virtual int CurrentStep { get; set; }

        /// <summary>
        /// Simple boolean indicating if the ticket is authenticated.
        /// </summary>
        public virtual bool Authenticated { get; set; }
    }
}
