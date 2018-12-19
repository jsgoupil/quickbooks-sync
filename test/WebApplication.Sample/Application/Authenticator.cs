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
            // Log in the user via the database.
            var user = await dbContext.Users
                .Where(m => m.UserName == login)
                .Where(m => m.Password == password)
                .FirstOrDefaultAsync();

            var guid = Guid.NewGuid().ToString();
            if (user != null)
            {
                return new QbTicket
                {
                    Authenticated = true,
                    Ticket = guid,

                    // We store more information about the ticket, such as the user.
                    // Check the extension to learn how to reach for this user.
                    User = user,
                    UserId = user.Id
                };
            }

            return new QbTicket
            {
                Authenticated = false,
                Ticket = guid
            };
        }

        public async Task<IAuthenticatedTicket> GetAuthenticationFromTicketAsync(string ticket)
        {
            // Fetch the ticket based on the guid.
            var qbTicket = await dbContext.QbTickets
                .FirstOrDefaultAsync(m => m.Ticket == ticket);

            return qbTicket;
        }

        public async Task SaveTicketAsync(IAuthenticatedTicket ticket)
        {
            // Save the ticket to the databse.
            // It contains the information about the current step.
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
