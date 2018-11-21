using QbSync.QbXml;
using QbSync.QbXml.Objects;
using QbSync.WebConnector.Core;
using System.Threading.Tasks;

namespace QbSync.WebConnector.Impl
{
    public abstract class StepQueryResponseBase<QbResponse> : IStepQueryResponse
        where QbResponse : class, IQbResponse, new()
    {
        protected QbXmlResponseOptions qbXmlResponseOptions;

        public StepQueryResponseBase()
        {
        }

        public abstract string Name { get; }

        public virtual async Task<int> ReceiveXMLAsync(IAuthenticatedTicket authenticatedTicket, string response, string hresult, string message)
        {
            var responseObject = new QbXmlResponse(qbXmlResponseOptions);

            if (!string.IsNullOrEmpty(response))
            {
                var msgResponseObject = responseObject.GetSingleItemFromResponse(response, typeof(QbResponse)) as QbResponse;
                await ExecuteResponseAsync(authenticatedTicket, msgResponseObject);

                return 0;
            }

            return -1;
        }

        public virtual Task<string> GotoStepAsync()
        {
            return Task.FromResult<string>((string)null);
        }

        public virtual Task<bool> GotoNextStepAsync()
        {
            return Task.FromResult<bool>(true);
        }

        public virtual Task<string> LastErrorAsync(IAuthenticatedTicket authenticatedTicket)
        {
            return Task.FromResult(string.Empty);
        }

        public Task SetOptionsAsync(QbXmlResponseOptions qbXmlResponseOptions)
        {
            this.qbXmlResponseOptions = qbXmlResponseOptions;

            return Task.FromResult<object>(null);
        }
        
        protected virtual Task ExecuteResponseAsync(IAuthenticatedTicket authenticatedTicket, QbResponse response)
        {
            return Task.FromResult<object>(null);
        }
    }
}
