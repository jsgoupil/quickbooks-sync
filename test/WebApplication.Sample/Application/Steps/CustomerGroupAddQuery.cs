using QbSync.QbXml.Objects;
using QbSync.WebConnector.Core;
using QbSync.WebConnector.Impl;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplication.Sample.Db;
using WebApplication.Sample.Extensions;

namespace WebApplication.Sample.Application.Steps
{
    public class CustomerGroupAddQuery
    {
        public const string NAME = "CustomerGroupAddQuery";

        public class Request : GroupStepQueryRequestBase
        {
            public override string Name => NAME;

            private readonly ApplicationDbContext dbContext;

            public Request(
                ApplicationDbContext dbContext
            )
            {
                this.dbContext = dbContext;
            }

            protected override Task<IEnumerable<IQbRequest>?> ExecuteRequestAsync(IAuthenticatedTicket authenticatedTicket)
            {
                var list = new List<IQbRequest>
                {
                    new CustomerAddRqType
                    {
                        CustomerAdd = new QbSync.QbXml.Objects.CustomerAdd
                        {
                            Name = "Unique Name" + Guid.NewGuid().ToString("D"),
                            FirstName = "User " + authenticatedTicket.GetUserId().ToString()
                        }
                    },
                    new CustomerQueryRqType
                    {
                        ActiveStatus = ActiveStatus.All
                    }
                };

                return Task.FromResult<IEnumerable<IQbRequest>?>(list as IEnumerable<IQbRequest>);
            }

            protected override Task<QBXMLMsgsRqOnError> GetOnErrorAttributeAsync(IAuthenticatedTicket authenticatedTicket)
            {
                // This is the default behavior, use this overriden method to change it to stopOnError
                // QuickBooks does not support rollbackOnError
                return Task.FromResult(QBXMLMsgsRqOnError.continueOnError);
            }
        }

        public class Response : GroupStepQueryResponseBase
        {
            public override string Name => NAME;

            private readonly ApplicationDbContext dbContext;

            public Response(
                ApplicationDbContext dbContext
            )
            {
                this.dbContext = dbContext;
            }

            protected override Task ExecuteResponseAsync(IAuthenticatedTicket authenticatedTicket, IEnumerable<IQbResponse> responses)
            {
                foreach (var item in responses)
                {
                    switch (item)
                    {
                        case CustomerQueryRsType customerQueryRsType:
                            // Do something with the CustomerQuery data
                            break;
                        case CustomerAddRsType customerAddRsType:
                            // Do something with the CustomerAdd data
                            break;
                    }
                }

                return base.ExecuteResponseAsync(authenticatedTicket, responses);
            }
        }
    }
}