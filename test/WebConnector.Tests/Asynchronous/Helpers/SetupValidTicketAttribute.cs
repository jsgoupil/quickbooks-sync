using System;

namespace QbSync.WebConnector.Tests.Asynchronous.Helpers
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