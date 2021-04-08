using QbSync.WebConnector.Core;
using QbSync.WebConnector.Impl;
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

        public override async Task CloseConnectionAsync(IAuthenticatedTicket? authenticatedTicket)
        {
            if (authenticatedTicket != null)
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
}
