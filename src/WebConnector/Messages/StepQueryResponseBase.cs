using QbSync.QbXml;
using QbSync.QbXml.Messages.Requests;
using QbSync.QbXml.Messages.Responses;

namespace QbSync.WebConnector.Messages
{
    public abstract class StepQueryResponseBase<T, Y> : StepQueryResponse
        where T : QbRequestWrapper, new()
        where Y : class, new()
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
                    var qbXmlRequest = new QbXmlRequest();
                    qbXmlRequest.AddToSingle(requestObject);

                    return qbXmlRequest.GetRequest();
                }
            }

            return null;
        }

        public virtual int ReceiveXML(AuthenticatedTicket authenticatedTicket, string response, string hresult, string message)
        {
            var responseObject = new QbXmlResponse();

            if (!string.IsNullOrEmpty(response))
            {
                var msgResponseObject = responseObject.GetSingleItemFromResponse(response, typeof(Y)) as Y;
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

        protected virtual bool ExecuteRequest(AuthenticatedTicket authenticatedTicket, T request)
        {
            return true;
        }

        protected virtual void ExecuteResponse(AuthenticatedTicket authenticatedTicket, Y response)
        {
        }
    }
}
