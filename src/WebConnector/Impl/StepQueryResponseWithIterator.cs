using QbSync.QbXml.Objects;
using QbSync.WebConnector.Core;
using System.Threading.Tasks;

namespace QbSync.WebConnector.Impl
{
    /// <summary>
    /// Helper to receive a response with an interator from the WebConnector.
    /// </summary>
    /// <typeparam name="QbResponse">The response.</typeparam>
    public abstract class StepQueryResponseWithIterator<QbResponse> : StepQueryResponseBase<QbResponse>
        where QbResponse : class, IQbIteratorResponse, IQbResponse, new()
    {
        private bool gotoNextStep = true;

        /// <summary>
        /// Constructs a StepQueryResponseWithIterator.
        /// </summary>
        public StepQueryResponseWithIterator()
            : base()
        {
        }

        /// <summary>
        /// Executes the response, useful to save the data coming from QuickBooks.
        /// </summary>
        /// <param name="authenticatedTicket">The ticket</param>
        /// <param name="response">The QbResponse</param>
        /// <returns>Task.</returns>
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

        /// <summary>
        /// After receiving XML from the Web Connector, we check if we should move to the next step
        /// by calling this method. Usually, you want to move to the next step unless your step
        /// has other messages to send to the Web Connector. It is the case when you use an iterator.
        /// </summary>
        /// <returns>False stays on the current step. True goes to the next step.</returns>
        public override Task<bool> GotoNextStepAsync()
        {
            return Task.FromResult(gotoNextStep);
        }

        /// <summary>
        /// Saves the message that will need to be loaded in the request.
        /// Save it with the ticket and key as the "key" and the value should be restored later.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <param name="key">They key.</param>
        /// <param name="value">The value.</param>
        /// <returns>Task.</returns>
        protected abstract Task SaveMessageAsync(IAuthenticatedTicket ticket, string key, string value);
    }
}