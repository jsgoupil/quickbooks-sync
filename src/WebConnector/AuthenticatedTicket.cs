namespace QbSync.WebConnector
{
    public class AuthenticatedTicket
    {
        /// <summary>
        /// Constant defining the initial step.
        /// </summary>
        public const string InitialStep = "##INITIAL##";

        /// <summary>
        /// Authenticated ticket constructor.
        /// </summary>
        public AuthenticatedTicket()
        {
            CurrentStep = InitialStep;
        }

        /// <summary>
        /// Ticket exchanged with the WebConnector. It acts as a session identifier.
        /// </summary>
        public virtual string Ticket { get; set; }

        /// <summary>
        /// State indicating what to exchange with the Web Connector.
        /// </summary>
        public virtual string CurrentStep { get; set; }

        /// <summary>
        /// Simple boolean indicating if the ticket is authenticated.
        /// </summary>
        public virtual bool Authenticated { get; set; }
    }
}
