using QBSync.QbXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QbSync.WebConnector.Messages
{
    public abstract class StepQueryWithIterator<T, Y, YResult> : StepQueryResponseBase<T, Y, YResult>
        where T : IteratorRequest, new()
        where Y : QbXmlResponse<YResult>, new()
    {
        internal const string IteratorKey = "Iterator";
        private bool gotoNextStep = true;

        public StepQueryWithIterator(int step)
            : base(step)
        {
        }

        protected override void ExecuteRequest(T request)
        {
            base.ExecuteRequest(request);

            var savedMessage = RetrieveMessage(step, IteratorKey);
            if (!string.IsNullOrEmpty(savedMessage))
            {
                request.IteratorID = savedMessage;
            }

            request.MaxReturned = 100;
        }

        protected override void ExecuteResponse(QbXmlMsgResponse<YResult> response)
        {
            // We have more that can come our way.
            if (response.IteratorRemainingCount.HasValue && response.IteratorRemainingCount.Value > 0)
            {
                gotoNextStep = false;
                SaveMessage(step, IteratorKey, response.IteratorID);
            }

            base.ExecuteResponse(response);
        }

        public override bool GotoNextStep()
        {
            return gotoNextStep;
        }
    }
}