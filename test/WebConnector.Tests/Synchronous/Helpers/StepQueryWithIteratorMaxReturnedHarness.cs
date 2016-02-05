using QbSync.QbXml.Objects;
using QbSync.WebConnector.Synchronous.Messages;
using System.Threading.Tasks;

namespace QbSync.WebConnector.Tests.Synchronous.Helpers
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