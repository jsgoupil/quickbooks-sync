using QbSync.WebConnector.Core;
using WebApplication.Sample.Db;

namespace WebApplication.Sample.Extensions
{
    public static class AuthenticatedTicketExtensions
    {
        /// <summary>
        /// Extension method to help fetching the extra properties off the IAuthenticatedTicket.
        /// </summary>
        /// <param name="authenticatedTicket">The ticket.</param>
        /// <returns>The user id.</returns>
        public static int GetUserId(this IAuthenticatedTicket authenticatedTicket)
        {
            if (authenticatedTicket != null)
            {
                var qbTicket = authenticatedTicket as QbTicket;
                if (qbTicket != null && qbTicket.UserId.HasValue)
                {
                    return qbTicket.UserId.Value;
                }
            }

            return 0;
        }
    }
}
