using QbSync.QbXml;
using QbSync.QbXml.Objects;
using QbSync.WebConnector.Core;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QbSync.WebConnector.Impl
{
    /// <summary>
    /// Helper to send more than one request to the WebConnector.
    /// </summary>
    public abstract class GroupStepQueryRequestBase : IStepQueryRequest
    {
        /// <summary>
        /// Constructs a GroupStepQueryRequestBase.
        /// </summary>
        public GroupStepQueryRequestBase()
        {
        }

        /// <summary>
        /// Returns the step name.
        /// </summary>
        /// <returns>Step name.</returns>
        public abstract string Name { get; }

        /// <summary>
        /// Returns the string that has to be sent to the Web Connector.
        /// Return null if your step has nothing to do this time. The next step will be executed immediately.
        /// </summary>
        /// <param name="authenticatedTicket">The authenticated ticket.</param>
        /// <returns>QbXml or null to execute the next step.</returns>
        public virtual async Task<string?> SendXMLAsync(IAuthenticatedTicket authenticatedTicket)
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

        /// <summary>
        /// Gets the holder for all QbRequests.
        /// </summary>
        /// <param name="authenticatedTicket">The authenticated ticket.</param>
        /// <returns>List holding the QbRequests.</returns>
        protected internal virtual Task<IEnumerable<IQbRequest>?> ExecuteRequestAsync(IAuthenticatedTicket authenticatedTicket)
        {
            return Task.FromResult<IEnumerable<IQbRequest>?>(Enumerable.Empty<IQbRequest>());
        }

        /// <summary>
        /// Indicates which error mode to use.
        /// </summary>
        /// <param name="authenticatedTicket">The authenticated ticket.</param>
        /// <returns>Error Mode.</returns>
        protected internal virtual Task<QBXMLMsgsRqOnError> GetOnErrorAttributeAsync(IAuthenticatedTicket authenticatedTicket)
        {
            return Task.FromResult(QBXMLMsgsRqOnError.continueOnError);
        }
    }
}
