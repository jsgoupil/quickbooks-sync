using QbSync.QbXml;
using QbSync.QbXml.Objects;
using QbSync.WebConnector.Core;
using System.Threading.Tasks;

namespace QbSync.WebConnector.Impl
{
    /// <summary>
    /// Helper to send a request to the WebConnector.
    /// </summary>
    /// <typeparam name="QbRequest">The request.</typeparam>
    public abstract class StepQueryRequestBase<QbRequest> : IStepQueryRequest
        where QbRequest : class, IQbRequest, new()
    {
        /// <summary>
        /// Constructs a StepQueryRequestBase.
        /// </summary>
        public StepQueryRequestBase()
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

        /// <summary>
        /// Creates a QbRequest instance.
        /// </summary>
        /// <param name="authenticatedTicket">The ticket.</param>
        /// <returns>The QbRequest.</returns>
        protected virtual Task<QbRequest?> CreateRequestAsync(IAuthenticatedTicket authenticatedTicket)
        {
            return Task.FromResult<QbRequest?>(new QbRequest());
        }

        /// <summary>
        /// Applies properties to the request.
        /// </summary>
        /// <param name="authenticatedTicket">The ticket</param>
        /// <param name="request">The QbRequest</param>
        /// <returns>True if we should continue with the step. False to not execute it.</returns>
        protected virtual Task<bool> ExecuteRequestAsync(IAuthenticatedTicket authenticatedTicket, QbRequest request)
        {
            return Task.FromResult(true);
        }
    }
}
