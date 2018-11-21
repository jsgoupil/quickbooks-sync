using QbSync.QbXml.Objects;
using QbSync.WebConnector.Core;
using QbSync.WebConnector.Impl;
using System;
using System.Threading.Tasks;

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
                    Name = "Unique Name" + Guid.NewGuid().ToString("D")
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