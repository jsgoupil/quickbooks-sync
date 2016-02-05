namespace QbSync.WebConnector.Tests.Synchronous.Helpers
{
    class SetupInvalidTicketAttribute : AuthenticatorAttribute
    {
        public SetupInvalidTicketAttribute()
        {
            AuthenticatedTicket = null;
        }
    }
}
