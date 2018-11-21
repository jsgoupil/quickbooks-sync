namespace QbSync.WebConnector.Tests.Helpers
{
    class SetupInvalidTicketAttribute : AuthenticatorAttribute
    {
        public SetupInvalidTicketAttribute()
        {
            AuthenticatedTicket = null;
        }
    }
}
