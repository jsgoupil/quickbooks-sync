using QbSync.QbXml.Objects;
using QbSync.WebConnector.Core;
using QbSync.WebConnector.Impl;
using System;
using System.Threading.Tasks;
using WebApplication.Sample.Extensions;

namespace WebApplication.Sample.Application.Steps
{
    public class CustomerAdd
    {
        public const string NAME = "CustomerAdd";

        public class Request : StepQueryRequestBase<CustomerAddRqType>
        {
            public override string Name => NAME;

            protected override Task<bool> ExecuteRequestAsync(IAuthenticatedTicket authenticatedTicket, CustomerAddRqType request)
            {
                request.CustomerAdd = new QbSync.QbXml.Objects.CustomerAdd
                {
                    Name = "Unique Name" + Guid.NewGuid().ToString("D"),
                    FirstName = "User " + authenticatedTicket.GetUserId().ToString()
                };

                return base.ExecuteRequestAsync(authenticatedTicket, request);
            }
        }

        public class Response : StepQueryResponseBase<CustomerAddRsType>
        {
            public override string Name => NAME;

            protected override Task ExecuteResponseAsync(IAuthenticatedTicket authenticatedTicket, CustomerAddRsType response)
            {
                return base.ExecuteResponseAsync(authenticatedTicket, response);
            }
        }
    }
}