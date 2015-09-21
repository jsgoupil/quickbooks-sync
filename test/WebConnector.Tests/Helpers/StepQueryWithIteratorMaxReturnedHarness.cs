using QbSync.QbXml.Objects;
using QbSync.WebConnector.Messages;

namespace QbSync.WebConnector.Tests.Helpers
{
    public abstract class StepQueryWithIteratorMaxReturnedHarness : StepQueryWithIterator<CustomerQueryRqType, CustomerQueryRsType>
    {
        public StepQueryWithIteratorMaxReturnedHarness(string defaultMaxReturned)
        {
            this.DefaultMaxReturned = defaultMaxReturned;
        }

        protected override CustomerQueryRqType CreateRequest(AuthenticatedTicket authenticatedTicket)
        {
            CustomerQueryRqType req = base.CreateRequest(authenticatedTicket);
            req.MaxReturned = this.DefaultMaxReturned;
            return req;
        }

        public string DefaultMaxReturned { get; private set; }
    }
}