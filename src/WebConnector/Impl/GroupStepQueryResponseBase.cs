using QbSync.QbXml;
using QbSync.QbXml.Objects;
using QbSync.WebConnector.Core;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QbSync.WebConnector.Impl
{
    public abstract class GroupStepQueryResponseBase : IStepQueryResponse
    {
        protected QbXmlResponseOptions qbXmlResponseOptions;

        public GroupStepQueryResponseBase()
        {
        }

        public abstract string Name { get; }

        public virtual async Task<int> ReceiveXMLAsync(IAuthenticatedTicket authenticatedTicket, string response, string hresult, string message)
        {
            var responseObject = new QbXmlResponse(qbXmlResponseOptions);

            if (!string.IsNullOrEmpty(response))
            {
                var objects = responseObject.GetItemsFromResponse(response, typeof(IQbResponse));
                var msgResponseObject = ((IEnumerable)objects).Cast<IQbResponse>();
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
        
        protected internal virtual Task ExecuteResponseAsync(IAuthenticatedTicket authenticatedTicket, IEnumerable<IQbResponse> responses)
        {
            return Task.FromResult<object>(null);
        }
    }
}
