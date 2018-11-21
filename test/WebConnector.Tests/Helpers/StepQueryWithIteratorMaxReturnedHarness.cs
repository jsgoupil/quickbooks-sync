using QbSync.QbXml.Objects;
using QbSync.WebConnector.Core;
using QbSync.WebConnector.Impl;
using System.Threading.Tasks;

namespace QbSync.WebConnector.Tests.Helpers
{
    public abstract class StepQueryRequestWithIteratorMaxReturnedHarness : StepQueryRequestWithIterator<CustomerQueryRqType>
    {
        public StepQueryRequestWithIteratorMaxReturnedHarness(string defaultMaxReturned)
        {
            this.DefaultMaxReturned = defaultMaxReturned;
        }

        protected override async Task<CustomerQueryRqType> CreateRequestAsync(IAuthenticatedTicket authenticatedTicket)
        {
            CustomerQueryRqType req = await base.CreateRequestAsync(authenticatedTicket);
            req.MaxReturned = this.DefaultMaxReturned;
            return req;
        }

        public string DefaultMaxReturned { get; private set; }
    }
}