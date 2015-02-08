using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QbSync.QbWebConnector.Tests.Helpers
{
    class SetupInvalidTicketAttribute : AuthenticatorAttribute
    {
        public SetupInvalidTicketAttribute()
        {
            AuthenticatedTicket = null;
        }
    }
}
