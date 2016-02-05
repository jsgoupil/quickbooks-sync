using QbSync.QbXml;
using QbSync.QbXml.Objects;
using System.Threading.Tasks;

namespace QbSync.WebConnector.Asynchronous.Messages
{
    public abstract class StepQueryWithIterator<T, Y> : StepQueryResponseBase<T, Y>
        where T : class, IQbIteratorRequest, IQbRequest, new()
        where Y : class, IQbIteratorResponse, IQbResponse, new()
    {
        internal const string IteratorKey = "Iterator";
        private const string MaxReturnedDefault = "100";
        private bool gotoNextStep = true;

        public StepQueryWithIterator()
            : base()
        {
        }

        protected override async Task<bool> ExecuteRequestAsync(AuthenticatedTicket authenticatedTicket, T request)
        {
            var savedMessage = await RetrieveMessageAsync(authenticatedTicket, IteratorKey);

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

        protected override async Task ExecuteResponseAsync(AuthenticatedTicket authenticatedTicket, Y response)
        {
            // We have more that can come our way.
            if (response.iteratorRemainingCount.HasValue && response.iteratorRemainingCount.Value > 0)
            {
                gotoNextStep = false;
                await SaveMessageAsync(authenticatedTicket, IteratorKey, response.iteratorID);
            }

            await base.ExecuteResponseAsync(authenticatedTicket, response);
        }

        public override Task<bool> GotoNextStepAsync()
        {
            return Task.FromResult(gotoNextStep);
        }

        protected abstract Task SaveMessageAsync(AuthenticatedTicket ticket, string key, string value);
        protected abstract Task<string> RetrieveMessageAsync(AuthenticatedTicket ticket, string key);
    }
}