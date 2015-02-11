using QbSync.QbXml;

namespace QbSync.WebConnector.Messages
{
    public abstract class StepQueryResponseBase<T, Y, YResult> : StepQueryResponse
        where T : QbXmlRequest, new()
        where Y : QbXmlResponse<YResult>, new()
    {
        protected AuthenticatedTicket authenticatedTicket;

        public StepQueryResponseBase(AuthenticatedTicket authenticatedTicket)
        {
            this.authenticatedTicket = authenticatedTicket;
        }

        public virtual string SendXML()
        {
            var requestObject = CreateRequest();
            if (requestObject != null)
            {
                ExecuteRequest(requestObject);

                return requestObject.GetRequest();
            }

            return null;
        }

        public virtual int ReceiveXML(string response, string hresult, string message)
        {
            var responseObject = CreateResponse();

            if (responseObject != null)
            {
                var msgResponseObject = responseObject.ParseResponse(response);
                ExecuteResponse(msgResponseObject);

                return 0;
            }

            return -1;
        }

        public virtual bool GotoNextStep()
        {
            return true;
        }

        public virtual string LastError()
        {
            return string.Empty;
        }

        protected virtual T CreateRequest()
        {
            return new T();
        }

        protected virtual Y CreateResponse()
        {
            return new Y();
        }

        protected virtual void ExecuteRequest(T request)
        {
        }

        protected virtual void ExecuteResponse(QbXmlMsgResponse<YResult> response)
        {
        }
    }
}
