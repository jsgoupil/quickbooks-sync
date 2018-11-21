using QbSync.WebConnector.Core;

namespace QbSync.WebConnector.Tests.Models
{
    public class AuthenticatedTicket : IAuthenticatedTicket
    {
        public string Ticket { get; set; }
        public string CurrentStep { get; set; }
        public bool Authenticated { get; set; }
    }
}
