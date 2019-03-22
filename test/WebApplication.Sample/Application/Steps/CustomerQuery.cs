using Microsoft.EntityFrameworkCore;
using QbSync.QbXml.Objects;
using QbSync.WebConnector.Core;
using QbSync.WebConnector.Impl;
using System;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.Sample.Db;
using WebApplication.Sample.Extensions;

namespace WebApplication.Sample.Application.Steps
{
    public class CustomerQuery
    {
        public const string NAME = "CustomerQuery";
        private const string LAST_MODIFIED_CUSTOMER = "LAST_MODIFIED_CUSTOMER";

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

            protected override async Task<bool> ExecuteRequestAsync(IAuthenticatedTicket authenticatedTicket, CustomerQueryRqType request)
            {
                // Let's see if we had a previous saved time so we don't re-query the entire QuickBooks every time.
                var previouslySavedFromModified = (await dbContext.QbSettings
                    .FirstOrDefaultAsync(m => m.Name == LAST_MODIFIED_CUSTOMER))?.Value;
               
                request.FromModifiedDate = DATETIMETYPE.ParseOrDefault(previouslySavedFromModified, DATETIMETYPE.MinValue).GetQueryFromModifiedDate();
                return await base.ExecuteRequestAsync(authenticatedTicket, request);
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

            protected override async Task ExecuteResponseAsync(IAuthenticatedTicket authenticatedTicket, CustomerQueryRsType response)
            {
                if (response.CustomerRet != null)
                {
                    foreach (var customer in response.CustomerRet)
                    {
                        // save these customers.
                    }

                    var lastFromModifiedDate = response.CustomerRet.OrderBy(m => m.TimeModified).Select(m => m.TimeModified).LastOrDefault();
                    await dbContext.SaveIfNewerAsync(LAST_MODIFIED_CUSTOMER, lastFromModifiedDate);
                }

                await base.ExecuteResponseAsync(authenticatedTicket, response);
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