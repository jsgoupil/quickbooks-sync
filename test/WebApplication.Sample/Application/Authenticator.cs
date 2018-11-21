using Microsoft.EntityFrameworkCore;
using QbSync.WebConnector.Core;
using System;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.Sample.Db;

namespace WebApplication.Sample.Application
{
    public class Authenticator : IAuthenticator
    {
        private readonly ApplicationDbContext dbContext;

        public Authenticator(
            ApplicationDbContext dbContext
        )
        {
            this.dbContext = dbContext;
        }

        public async Task<IAuthenticatedTicket> GetAuthenticationFromLoginAsync(string login, string password)
        {
            var user = await dbContext.Users
                .Where(m => m.UserName == login)
                .Where(m => m.Password == password)
                .FirstOrDefaultAsync();

            if (user != null)
            {
                return new QbTicket
                {
                    Authenticated = true,
                    Ticket = Guid.NewGuid().ToString(),
                    User = user,
                    UserId = user.Id
                };
            }

            return new QbTicket
            {
                Authenticated = false
            };
        }

        public async Task<IAuthenticatedTicket> GetAuthenticationFromTicketAsync(string ticket)
        {
            var qbTicket = await dbContext.QbTickets
                .FirstOrDefaultAsync(m => m.Ticket == ticket);

            return qbTicket;
        }

        public async Task SaveTicketAsync(IAuthenticatedTicket ticket)
        {
            var qbTicket = await dbContext.QbTickets
                .FirstOrDefaultAsync(m => m.Ticket == ticket.Ticket);

            if (qbTicket == null)
            {
                var ticketAsQbTicket = ticket as QbTicket;
                if (ticketAsQbTicket != null)
                {
                    qbTicket = new QbTicket
                    {
                        Ticket = ticket.Ticket,
                        Authenticated = ticket.Authenticated,
                        User = ticketAsQbTicket.User,
                        UserId = ticketAsQbTicket.UserId
                    };
                    dbContext.QbTickets.Add(qbTicket);
                }
            }

            qbTicket.CurrentStep = ticket.CurrentStep;
            await dbContext.SaveChangesAsync();
        }
    }
}
