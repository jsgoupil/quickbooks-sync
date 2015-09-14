using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QbSync.WebConnector
{
    public class QbSyncException : Exception
    {
        public QbSyncException(AuthenticatedTicket ticket, Exception innerException)
            : base(null, innerException)
        {
            this.Ticket = ticket;
        }

        public AuthenticatedTicket Ticket { get; private set; }
    }
}
