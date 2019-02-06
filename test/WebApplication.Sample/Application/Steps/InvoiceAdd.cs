using QbSync.QbXml.Objects;
using QbSync.WebConnector.Core;
using QbSync.WebConnector.Impl;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplication.Sample.Extensions;

namespace WebApplication.Sample.Application.Steps
{
    public class InvoiceAdd
    {
        public const string NAME = "InvoiceAdd";

        public class Request : StepQueryRequestBase<InvoiceAddRqType>
        {
            public override string Name => NAME;

            protected override Task<bool> ExecuteRequestAsync(IAuthenticatedTicket authenticatedTicket, InvoiceAddRqType request)
            {
                request.InvoiceAdd = new QbSync.QbXml.Objects.InvoiceAdd
                {
                    // Mandatory
                    CustomerRef = new CustomerRef
                    {
                        // Find the ListID for a customer with a previous Query.
                        ListID = "80000001-1541542677"
                    },
                    InvoiceLineAdd = new List<InvoiceLineAdd>
                    {
                        new InvoiceLineAdd
                        {
                            Amount = 12,
                            ItemRef = new ItemRef
                            {
                                // Mandatory, you can query the list of of items prior to
                                // add an invoice. You may use the full name or ListID.
                                FullName = "test"
                            }
                        }
                    }
                };
                return base.ExecuteRequestAsync(authenticatedTicket, request);
            }
        }

        public class Response : StepQueryResponseBase<InvoiceAddRsType>
        {
            public override string Name => NAME;

            protected override Task ExecuteResponseAsync(IAuthenticatedTicket authenticatedTicket, InvoiceAddRsType response)
            {
                return base.ExecuteResponseAsync(authenticatedTicket, response);
            }
        }
    }
}