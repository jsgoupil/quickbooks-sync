using QbSync.QbXml.Objects;
using QbSync.WebConnector.Core;
using System.Threading.Tasks;

namespace QbSync.WebConnector.Impl
{
    internal class StepQueryRequestWithIterator
    {
        internal const string IteratorKey = "Iterator";
    }

    public abstract class StepQueryRequestWithIterator<QbRequest> : StepQueryRequestBase<QbRequest>
        where QbRequest : class, IQbIteratorRequest, IQbRequest, new()
    {
        private const string MaxReturnedDefault = "100";

        public StepQueryRequestWithIterator()
            : base()
        {
        }

        protected override async Task<bool> ExecuteRequestAsync(IAuthenticatedTicket authenticatedTicket, QbRequest request)
        {
            var savedMessage = await RetrieveMessageAsync(authenticatedTicket, StepQueryRequestWithIterator.IteratorKey);

            request.iterator = IteratorType.Start;
            if (!string.IsNullOrEmpty(savedMessage))
            {
                request.iterator = IteratorType.Continue;
                request.iteratorID = savedMessage;
            }

            int maxVal;
            bool isValid = int.TryParse(request.MaxReturned, out maxVal);

            if (!isValid || (isValid && (maxVal < 1)))
            {
                // Not a valid max returned value. Set to 100 by default.
                request.MaxReturned = MaxReturnedDefault;
            }

            return await base.ExecuteRequestAsync(authenticatedTicket, request);
        }

        protected abstract Task<string> RetrieveMessageAsync(IAuthenticatedTicket ticket, string key);
    }
}