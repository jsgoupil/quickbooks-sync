using System;

namespace QbSync.WebConnector.Core
{
    /// <summary>
    /// An QuickBooks Synchrnonization Exception.
    /// </summary>
    public class QbSyncException : Exception
    {
        /// <summary>
        /// Creates a QbSyncException.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <param name="innerException">Inner Exception.</param>
        public QbSyncException(IAuthenticatedTicket? ticket, Exception innerException)
            : base(null, innerException)
        {
            this.Ticket = ticket;
        }

        /// <summary>
        /// The ticket.
        /// </summary>
        public IAuthenticatedTicket? Ticket { get; private set; }
    }
}
