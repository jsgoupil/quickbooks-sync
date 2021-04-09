using QbSync.QbXml.Objects;
using QbSync.WebConnector.Core;
using System.Threading.Tasks;

namespace QbSync.WebConnector.Impl
{
    internal class StepQueryRequestWithIterator
    {
        internal const string IteratorKey = "Iterator";
    }

    /// <summary>
    /// Helper to send a request with an interator to the WebConnector.
    /// </summary>
    /// <typeparam name="QbRequest">The request.</typeparam>
    public abstract class StepQueryRequestWithIterator<QbRequest> : StepQueryRequestBase<QbRequest>
        where QbRequest : class, IQbIteratorRequest, IQbRequest, new()
    {
        private const string MaxReturnedDefault = "100";

        /// <summary>
        /// Constructs a StepQueryRequestWithIterator.
        /// </summary>
        public StepQueryRequestWithIterator()
            : base()
        {
        }

        /// <summary>
        /// Applies properties to the request.
        /// </summary>
        /// <param name="authenticatedTicket">The ticket</param>
        /// <param name="request">The QbRequest</param>
        /// <returns>True if we should continue with the step. False to not execute it.</returns>
        protected override async Task<bool> ExecuteRequestAsync(IAuthenticatedTicket authenticatedTicket, QbRequest request)
        {
            var savedMessage = await RetrieveMessageAsync(authenticatedTicket, StepQueryRequestWithIterator.IteratorKey);

            request.iterator = IteratorType.Start;
            if (!string.IsNullOrEmpty(savedMessage))
            {
                request.iterator = IteratorType.Continue;
                request.iteratorID = savedMessage;
            }

            bool isValid = int.TryParse(request.MaxReturned, out int maxVal);

            if (!isValid || (isValid && (maxVal < 1)))
            {
                // Not a valid max returned value. Set to 100 by default.
                request.MaxReturned = MaxReturnedDefault;
            }

            return await base.ExecuteRequestAsync(authenticatedTicket, request);
        }

        /// <summary>
        /// Gets the message previously saved with a ticket and key.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <param name="key">The key.</param>
        /// <returns>Previously saved message.</returns>
        protected abstract Task<string?> RetrieveMessageAsync(IAuthenticatedTicket ticket, string key);
    }
}