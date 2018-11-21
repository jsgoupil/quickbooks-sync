using QbSync.WebConnector.Tests.Models;
using System;

namespace QbSync.WebConnector.Tests.Helpers
{
    class SetupValidTicketAttribute : AuthenticatorAttribute
    {
        public SetupValidTicketAttribute()
        {
            AuthenticatedTicket = new AuthenticatedTicket
            {
                Authenticated = true,
                Ticket = Guid.NewGuid().ToString()
            };
        }
    }
}