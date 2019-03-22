using QbSync.QbXml.Objects;
using QbSync.WebConnector.Core;
using QbSync.WebConnector.Impl;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplication.Sample.Extensions;

namespace WebApplication.Sample.Application.Steps
{
    public class SaleOrderAdd
    {
        public const string NAME = "SaleOrderAdd";

        public class Request : StepQueryRequestBase<SalesOrderAddRqType>
        {
            public override string Name => NAME;

            protected override Task<bool> ExecuteRequestAsync(IAuthenticatedTicket authenticatedTicket, SalesOrderAddRqType request)
{
request.SalesOrderAdd = new SalesOrderAdd
{
    SalesOrderLineAdd = new List<SalesOrderLineAdd>
    {
        new SalesOrderLineAdd
        {
            Rate = 1,
            ItemRef = new ItemRef
            {
                FullName = "TechReady Item"
            },
            Quantity = 2,
            Amount = 200
        }
    },
    CustomerRef = new CustomerRef
    {
        ListID = "80000001-1541542677"
    },
    RefNumber = "SO-001",
    BillAddress = new BillAddress
    {
        Addr1 = "110 Main Street",
        Addr2 = "Suite 2000",
        State = "TX",
        PostalCode = "99875",
        Country = "US"
    },
    DueDate = new DATETYPE("2019-10-10"),
};
                return base.ExecuteRequestAsync(authenticatedTicket, request);
            }
        }

        public class Response : StepQueryResponseBase<SalesOrderAddRsType>
        {
            public override string Name => NAME;

            protected override Task ExecuteResponseAsync(IAuthenticatedTicket authenticatedTicket, SalesOrderAddRsType response)
            {
                return base.ExecuteResponseAsync(authenticatedTicket, response);
            }
        }
    }
}