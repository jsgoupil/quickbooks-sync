using QbSync.QbXml;
using QbSync.QbXml.Objects;
using QbSync.WebConnector.Core;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace QbSync.WebConnector.Impl
{
    public abstract class GroupStepQueryRequestBase : IStepQueryRequest
    {
        public GroupStepQueryRequestBase()
        {
        }

        public abstract string Name { get; }

        public virtual async Task<string> SendXMLAsync(IAuthenticatedTicket authenticatedTicket)
        {
            var requestObject = await ExecuteRequestAsync(authenticatedTicket);
            var requestObjectArray = requestObject?.ToArray();
            if (requestObjectArray?.Any() == true)
            {
                var qbXmlRequest = new QbXmlRequest();

                var list = new List<object>(requestObjectArray.Count());
                foreach (var request in requestObjectArray)
                {
                    list.Add(request);
                }

                qbXmlRequest.Add(new QBXMLMsgsRq
                {
                    onError = await GetOnErrorAttributeAsync(authenticatedTicket),
                    Items = list.ToArray()
                });

                return qbXmlRequest.GetRequest();
            }

            return null;
        }

        protected internal virtual Task<IEnumerable<IQbRequest>> ExecuteRequestAsync(IAuthenticatedTicket authenticatedTicket)
        {
            return Task.FromResult(Enumerable.Empty<IQbRequest>());
        }

        protected internal virtual Task<QBXMLMsgsRqOnError> GetOnErrorAttributeAsync(IAuthenticatedTicket authenticatedTicket)
        {
            return Task.FromResult(QBXMLMsgsRqOnError.continueOnError);
        }
    }
}
