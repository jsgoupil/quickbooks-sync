using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
