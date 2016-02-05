using QbSync.QbXml;
using QbSync.QbXml.Objects;
using System.Threading.Tasks;

namespace QbSync.WebConnector.Asynchronous.Messages
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

        public virtual async Task<string> SendXMLAsync(AuthenticatedTicket authenticatedTicket)
        {
            var requestObject = await CreateRequestAsync(authenticatedTicket);
            if (requestObject != null)
            {
                if (await ExecuteRequestAsync(authenticatedTicket, requestObject))
                {
                    var qbXmlRequest = new QbXmlRequest();
                    qbXmlRequest.AddToSingle(requestObject);

                    return qbXmlRequest.GetRequest();
                }
            }

            return null;
        }

        public virtual async Task<int> ReceiveXMLAsync(AuthenticatedTicket authenticatedTicket, string response, string hresult, string message)
        {
            var responseObject = new QbXmlResponse(qbXmlResponseOptions);

            if (!string.IsNullOrEmpty(response))
            {
                var msgResponseObject = responseObject.GetSingleItemFromResponse(response, typeof(Y)) as Y;
                await ExecuteResponseAsync(authenticatedTicket, msgResponseObject);

                return 0;
            }

            return -1;
        }

        public virtual Task<string> GotoStepAsync()
        {
            return Task.FromResult<string>(null);
        }

        public virtual Task<bool> GotoNextStepAsync()
        {
            return Task.FromResult(true);
        }

        public virtual Task<string> LastErrorAsync(AuthenticatedTicket authenticatedTicket)
        {
            return Task.FromResult(string.Empty);
        }

        public Task SetOptionsAsync(QbXmlResponseOptions qbXmlResponseOptions)
        {
            this.qbXmlResponseOptions = qbXmlResponseOptions;

            return Task.FromResult<object>(null);
        }

        protected virtual Task<T> CreateRequestAsync(AuthenticatedTicket authenticatedTicket)
        {
            return Task.FromResult(new T());
        }

        protected virtual Task<bool> ExecuteRequestAsync(AuthenticatedTicket authenticatedTicket, T request)
        {
            return Task.FromResult(true);
        }

        protected virtual Task ExecuteResponseAsync(AuthenticatedTicket authenticatedTicket, Y response)
        {
            return Task.FromResult<object>(null);
        }
    }
}
