using QbSync.QbXml;
using QbSync.QbXml.Objects;

namespace QbSync.WebConnector.Messages
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

        protected override bool ExecuteRequest(AuthenticatedTicket authenticatedTicket, T request)
        {
            var savedMessage = RetrieveMessage(authenticatedTicket, IteratorKey);

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

            return base.ExecuteRequest(authenticatedTicket, request);
        }

        protected override void ExecuteResponse(AuthenticatedTicket authenticatedTicket, Y response)
        {
            // We have more that can come our way.
            if (response.iteratorRemainingCount.HasValue && response.iteratorRemainingCount.Value > 0)
            {
                gotoNextStep = false;
                SaveMessage(authenticatedTicket, IteratorKey, response.iteratorID);
            }

            base.ExecuteResponse(authenticatedTicket, response);
        }

        public override bool GotoNextStep()
        {
            return gotoNextStep;
        }

        protected abstract void SaveMessage(AuthenticatedTicket ticket, string key, string value);
        protected abstract string RetrieveMessage(AuthenticatedTicket ticket, string key);
    }
}