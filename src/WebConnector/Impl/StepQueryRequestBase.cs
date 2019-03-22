using QbSync.QbXml;
using QbSync.QbXml.Objects;
using QbSync.WebConnector.Core;
using System.Threading.Tasks;

namespace QbSync.WebConnector.Impl
{
    public abstract class StepQueryRequestBase<QbRequest> : IStepQueryRequest
        where QbRequest : class, IQbRequest, new()
    {
        public StepQueryRequestBase()
        {
        }

        public abstract string Name { get; }

        public virtual async Task<string> SendXMLAsync(IAuthenticatedTicket authenticatedTicket)
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

        protected virtual Task<QbRequest> CreateRequestAsync(IAuthenticatedTicket authenticatedTicket)
        {
            return Task.FromResult(new QbRequest());
        }

        protected virtual Task<bool> ExecuteRequestAsync(IAuthenticatedTicket authenticatedTicket, QbRequest request)
        {
            return Task.FromResult(true);
        }
    }
}
