using QbSync.QbXml;
using QbSync.QbXml.Objects;

namespace QbSync.WebConnector.Messages
{
    public abstract class StepQueryWithIterator<T, Y> : StepQueryResponseBase<T, Y>
        where T : class, QbIteratorRequest, QbRequest, new()
        where Y : class, QbIteratorResponse, QbResponse, new()
    {
        internal const string IteratorKey = "Iterator";
        private bool gotoNextStep = true;

        public StepQueryWithIterator()
            : base()
        {
        }

        protected override bool ExecuteRequest(AuthenticatedTicket authenticatedTicket, T request)
        {
            var savedMessage = RetrieveMessage(authenticatedTicket.Ticket, authenticatedTicket.CurrentStep, IteratorKey);

            request.iterator = IteratorType.Start;
            if (!string.IsNullOrEmpty(savedMessage))
            {
                request.iterator = IteratorType.Continue;
                request.iteratorID = savedMessage;
            }

            request.MaxReturned = "100";

            return base.ExecuteRequest(authenticatedTicket, request);
        }

        protected override void ExecuteResponse(AuthenticatedTicket authenticatedTicket, Y response)
        {
            // We have more that can come our way.
            if (response.iteratorRemainingCount.HasValue && response.iteratorRemainingCount.Value > 0)
            {
                gotoNextStep = false;
                SaveMessage(authenticatedTicket.Ticket, authenticatedTicket.CurrentStep, IteratorKey, response.iteratorID);
            }

            base.ExecuteResponse(authenticatedTicket, response);
        }

        public override bool GotoNextStep()
        {
            return gotoNextStep;
        }

        protected abstract void SaveMessage(string ticket, string step, string key, string value);
        protected abstract string RetrieveMessage(string ticket, string step, string key);
    }
}