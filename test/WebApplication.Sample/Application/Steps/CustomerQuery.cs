using Microsoft.EntityFrameworkCore;
using QbSync.QbXml.Objects;
using QbSync.WebConnector.Core;
using QbSync.WebConnector.Impl;
using System;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.Sample.Db;

namespace WebApplication.Sample.Application.Steps
{
    public class CustomerQuery
    {
        public const string NAME = "CustomerQuery";

        public class Request : StepQueryRequestWithIterator<CustomerQueryRqType>
        {
            public override string Name => NAME;

            private readonly ApplicationDbContext dbContext;

            public Request(
                ApplicationDbContext dbContext
            )
            {
                this.dbContext = dbContext;
            }

            protected override Task<bool> ExecuteRequestAsync(IAuthenticatedTicket authenticatedTicket, CustomerQueryRqType request)
            {
                // By default, we return 100, let's do 5 here.
                request.MaxReturned = "5";
                request.FromModifiedDate = new DATETIMETYPE(new DateTime(2019, 4, 28, 0, 55, 40, DateTimeKind.Utc));

                return base.ExecuteRequestAsync(authenticatedTicket, request);
            }

            protected override async Task<string> RetrieveMessageAsync(IAuthenticatedTicket ticket, string key)
            {
                var state = await dbContext.QbKvpStates
                    .Where(m => m.Ticket == ticket.Ticket)
                    .Where(m => m.CurrentStep == ticket.CurrentStep)
                    .Where(m => m.Key == key)
                    .FirstOrDefaultAsync();

                return state?.Value;
            }
        }

        public class Response : StepQueryResponseWithIterator<CustomerQueryRsType>
        {
            public override string Name => NAME;

            private readonly ApplicationDbContext dbContext;

            public Response(
                ApplicationDbContext dbContext
            )
            {
                this.dbContext = dbContext;
            }

            protected override Task ExecuteResponseAsync(IAuthenticatedTicket authenticatedTicket, CustomerQueryRsType response)
            {
                if (response.CustomerRet != null)
                {
                    foreach (var customer in response.CustomerRet)
                    {
                        // save these customers.
                    }
                }

                return base.ExecuteResponseAsync(authenticatedTicket, response);
            }

            protected override async Task SaveMessageAsync(IAuthenticatedTicket ticket, string key, string value)
            {
                var state = await dbContext.QbKvpStates
                    .Where(m => m.Ticket == ticket.Ticket)
                    .Where(m => m.CurrentStep == ticket.CurrentStep)
                    .Where(m => m.Key == key)
                    .FirstOrDefaultAsync();

                if (state == null)
                {
                    state = new QbKvpState
                    {
                        CurrentStep = ticket.CurrentStep,
                        Ticket = ticket.Ticket,
                        Key = key
                    };
                    dbContext.QbKvpStates.Add(state);
                }

                state.Value = value;

                await dbContext.SaveChangesAsync();
            }
        }
    }
}