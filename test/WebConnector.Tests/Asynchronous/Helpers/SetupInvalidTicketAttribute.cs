namespace QbSync.WebConnector.Tests.Asynchronous.Helpers
{
    class SetupInvalidTicketAttribute : AuthenticatorAttribute
    {
        public SetupInvalidTicketAttribute()
        {
            AuthenticatedTicket = null;
        }
    }
}
