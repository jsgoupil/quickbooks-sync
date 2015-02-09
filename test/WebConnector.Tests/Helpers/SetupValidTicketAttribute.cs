using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QbSync.WebConnector.Tests.Helpers
{
    class SetupValidTicketAttribute : AuthenticatorAttribute
    {
        public SetupValidTicketAttribute()
        {
            AuthenticatedTicket = new AuthenticatedTicket
            {
                Authenticated = true,
                CurrentStep = 0,
                Ticket = Guid.NewGuid().ToString()
            };
        }
    }
}