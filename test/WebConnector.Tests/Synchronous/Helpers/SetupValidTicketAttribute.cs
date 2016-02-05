using System;

namespace QbSync.WebConnector.Tests.Synchronous.Helpers
{
    class SetupValidTicketAttribute : AuthenticatorAttribute
    {
        public SetupValidTicketAttribute()
        {
            AuthenticatedTicket = new AuthenticatedTicket
            {
                Authenticated = true,
                CurrentStep = AuthenticatedTicket.InitialStep,
                Ticket = Guid.NewGuid().ToString()
            };
        }
    }
}