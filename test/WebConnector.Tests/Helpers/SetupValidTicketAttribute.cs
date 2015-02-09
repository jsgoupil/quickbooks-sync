using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QBSync.WebConnector.Tests.Helpers
{
    class SetupValidTicketAttribute : AuthenticatorAttribute
    {
        public SetupValidTicketAttribute()
        {
            AuthenticatedTicket = new QBSync.WebConnector.AuthenticatedTicket
            {
                Authenticated = true,
                CurrentStep = 0,
                Ticket = Guid.NewGuid().ToString()
            };
        }
    }
}