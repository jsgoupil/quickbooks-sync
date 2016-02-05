using QbSync.QbXml;
using QbSync.QbXml.Objects;

namespace QbSync.WebConnector.Synchronous.Messages
{
    public abstract class StepQueryResponseBase<T, Y> : IStepQueryResponse
        where T : class, IQbRequest, new()
        where Y : class, IQbResponse, new()
    {
        protected QbXmlResponseOptions qbXmlResponseOptions;

        public StepQueryResponseBase()
        {
        }

        public abstract string Name { get; }

        public virtual string SendXML(AuthenticatedTicket authenticatedTicket)
        {
            var requestObject = CreateRequest(authenticatedTicket);
            if (requestObject != null)
            {
                if (ExecuteRequest(authenticatedTicket, requestObject))
                {
                    var qbXmlRequest = new QbXmlRequest();
                    qbXmlRequest.AddToSingle(requestObject);

                    return qbXmlRequest.GetRequest();
                }
            }

            return null;
        }

        public virtual int ReceiveXML(AuthenticatedTicket authenticatedTicket, string response, string hresult, string message)
        {
            var responseObject = new QbXmlResponse(qbXmlResponseOptions);

            if (!string.IsNullOrEmpty(response))
            {
                var msgResponseObject = responseObject.GetSingleItemFromResponse(response, typeof(Y)) as Y;
                ExecuteResponse(authenticatedTicket, msgResponseObject);

                return 0;
            }

            return -1;
        }

        public virtual string GotoStep()
        {
            return null;
        }

        public virtual bool GotoNextStep()
        {
            return true;
        }

        public virtual string LastError(AuthenticatedTicket authenticatedTicket)
        {
            return string.Empty;
        }

        public void SetOptions(QbXmlResponseOptions qbXmlResponseOptions)
        {
            this.qbXmlResponseOptions = qbXmlResponseOptions;
        }

        protected virtual T CreateRequest(AuthenticatedTicket authenticatedTicket)
        {
            return new T();
        }

        protected virtual bool ExecuteRequest(AuthenticatedTicket authenticatedTicket, T request)
        {
            return true;
        }

        protected virtual void ExecuteResponse(AuthenticatedTicket authenticatedTicket, Y response)
        {
        }
    }
}
