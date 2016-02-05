using QbSync.QbXml.Objects;
using QbSync.WebConnector.Asynchronous.Messages;
using System.Threading.Tasks;

namespace QbSync.WebConnector.Tests.Asynchronous.Helpers
{
    public abstract class StepQueryWithIteratorMaxReturnedHarness : StepQueryWithIterator<CustomerQueryRqType, CustomerQueryRsType>
    {
        public StepQueryWithIteratorMaxReturnedHarness(string defaultMaxReturned)
        {
            this.DefaultMaxReturned = defaultMaxReturned;
        }

        protected override async Task<CustomerQueryRqType> CreateRequestAsync(AuthenticatedTicket authenticatedTicket)
        {
            CustomerQueryRqType req = await base.CreateRequestAsync(authenticatedTicket);
            req.MaxReturned = this.DefaultMaxReturned;
            return req;
        }

        public string DefaultMaxReturned { get; private set; }
    }
}