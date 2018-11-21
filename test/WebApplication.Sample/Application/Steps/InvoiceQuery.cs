using QbSync.QbXml.Objects;
using QbSync.WebConnector.Core;
using QbSync.WebConnector.Impl;
using System.Threading.Tasks;

namespace WebApplication.Sample.Application.Steps
{
    public class InvoiceQuery
    {
        public const string NAME = "InvoiceQuery";

        public class Request : StepQueryRequestBase<InvoiceQueryRqType>
        {
            public override string Name => NAME;

            protected override Task<bool> ExecuteRequestAsync(IAuthenticatedTicket authenticatedTicket, InvoiceQueryRqType request)
            {
                return base.ExecuteRequestAsync(authenticatedTicket, request);
            }
        }

        public class Response : StepQueryResponseBase<InvoiceQueryRsType>
        {
            public override string Name => NAME;

            protected override Task ExecuteResponseAsync(IAuthenticatedTicket authenticatedTicket, InvoiceQueryRsType response)
            {
                return base.ExecuteResponseAsync(authenticatedTicket, response);
            }
        }
    }
}
