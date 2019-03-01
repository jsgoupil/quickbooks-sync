using QbSync.QbXml;
using QbSync.WebConnector.Core;
using QbSync.WebConnector.Impl;
using System;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.Sample.Db;

namespace WebApplication.Sample.Application
{
    public class WebConnectorHandler : WebConnectorHandlerNoop
    {
        private readonly ApplicationDbContext dbContext;

        public WebConnectorHandler(
            ApplicationDbContext dbContext
        )
        {
            this.dbContext = dbContext;
        }

        public override Task<QbXmlResponseOptions> GetOptionsAsync(IAuthenticatedTicket authenticatedTicket)
        {
            // If you want to have UTC offsets included in timestamps returned from QuickBooks
            // you should return a TimeZoneInfo based on the authenticated ticket.
            // The timezone where the user is running their QuickBooks.
            return Task.FromResult(new QbXmlResponseOptions
            {
                QuickBooksDesktopTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time")
            });
        }

        public override async Task CloseConnectionAsync(IAuthenticatedTicket authenticatedTicket)
        {
            // We do some clean up.
            var savedStates = dbContext.QbKvpStates
                .Where(m => m.Ticket == authenticatedTicket.Ticket)
                .ToList();

            dbContext.QbKvpStates.RemoveRange(savedStates);

            var savedTickets = dbContext.QbTickets
                .Where(m => m.Ticket == authenticatedTicket.Ticket)
                .ToList();

            dbContext.QbTickets.RemoveRange(savedTickets);

            await dbContext.SaveChangesAsync();
        }
    }
}
