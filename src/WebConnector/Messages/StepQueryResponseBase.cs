using QbSync.QbXml;

namespace QbSync.WebConnector.Messages
{
    public abstract class StepQueryResponseBase<T, Y, YResult> : StepQueryResponse
        where T : QbXmlRequest, new()
        where Y : QbXmlResponse<YResult>, new()
    {
        public StepQueryResponseBase()
        {
        }

        public abstract string GetName();

        public virtual string SendXML(AuthenticatedTicket authenticatedTicket)
        {
            var requestObject = CreateRequest(authenticatedTicket);
            if (requestObject != null)
            {
                if (ExecuteRequest(authenticatedTicket, requestObject))
                {
                    return requestObject.GetRequest();
                }
            }

            return null;
        }

        public virtual int ReceiveXML(AuthenticatedTicket authenticatedTicket, string response, string hresult, string message)
        {
            var responseObject = CreateResponse(authenticatedTicket);

            if (responseObject != null && !string.IsNullOrEmpty(response))
            {
                var msgResponseObject = responseObject.ParseResponse(response);
                ExecuteResponse(authenticatedTicket, msgResponseObject);

                return 0;
            }

            return -1;
        }

        public virtual bool GotoNextStep()
        {
            return true;
        }

        public virtual string LastError(AuthenticatedTicket authenticatedTicket)
        {
            return string.Empty;
        }

        protected virtual T CreateRequest(AuthenticatedTicket authenticatedTicket)
        {
            return new T();
        }

        protected virtual Y CreateResponse(AuthenticatedTicket authenticatedTicket)
        {
            return new Y();
        }

        protected virtual bool ExecuteRequest(AuthenticatedTicket authenticatedTicket, T request)
        {
            return true;
        }

        protected virtual void ExecuteResponse(AuthenticatedTicket authenticatedTicket, QbXmlMsgResponse<YResult> response)
        {
        }
    }
}
