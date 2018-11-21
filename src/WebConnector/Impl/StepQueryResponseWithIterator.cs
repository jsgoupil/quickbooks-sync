using QbSync.QbXml.Objects;
using QbSync.WebConnector.Core;
using System.Threading.Tasks;

namespace QbSync.WebConnector.Impl
{
    public abstract class StepQueryResponseWithIterator<QbResponse> : StepQueryResponseBase<QbResponse>
        where QbResponse : class, IQbIteratorResponse, IQbResponse, new()
    {
        private bool gotoNextStep = true;

        public StepQueryResponseWithIterator()
            : base()
        {
        }

        protected override async Task ExecuteResponseAsync(IAuthenticatedTicket authenticatedTicket, QbResponse response)
        {
            // We have more that can come our way.
            if (response.iteratorRemainingCount.HasValue && response.iteratorRemainingCount.Value > 0)
            {
                gotoNextStep = false;
                await SaveMessageAsync(authenticatedTicket, StepQueryRequestWithIterator.IteratorKey, response.iteratorID);
            }

            await base.ExecuteResponseAsync(authenticatedTicket, response);
        }

        public override Task<bool> GotoNextStepAsync()
        {
            return Task.FromResult(gotoNextStep);
        }

        protected abstract Task SaveMessageAsync(IAuthenticatedTicket ticket, string key, string value);
    }
}