using QbSync.QbXml;

namespace QbSync.WebConnector.Messages
{
    public abstract class StepQueryWithIterator<T, Y, YResult> : StepQueryResponseBase<T, Y, YResult>
        where T : IteratorRequest, new()
        where Y : QbXmlResponse<YResult>, new()
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
            if (!string.IsNullOrEmpty(savedMessage))
            {
                request.IteratorID = savedMessage;
            }

            request.MaxReturned = 100;

            return base.ExecuteRequest(authenticatedTicket, request);
        }

        protected override void ExecuteResponse(AuthenticatedTicket authenticatedTicket, QbXmlMsgResponse<YResult> response)
        {
            // We have more that can come our way.
            if (response.IteratorRemainingCount.HasValue && response.IteratorRemainingCount.Value > 0)
            {
                gotoNextStep = false;
                SaveMessage(authenticatedTicket.Ticket, authenticatedTicket.CurrentStep, IteratorKey, response.IteratorID);
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