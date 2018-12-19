using System;

namespace QbSync.WebConnector.Core
{
    public class QbSyncException : Exception
    {
        public QbSyncException(IAuthenticatedTicket ticket, Exception innerException)
            : base(null, innerException)
        {
            this.Ticket = ticket;
        }

        public IAuthenticatedTicket Ticket { get; private set; }
    }
}
