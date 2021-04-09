using QbSync.QbXml;
using QbSync.QbXml.Objects;
using QbSync.WebConnector.Core;
using System.Threading.Tasks;

namespace QbSync.WebConnector.Impl
{
    /// <summary>
    /// Helper to receive the response from the WebConnector.
    /// </summary>
    /// <typeparam name="QbResponse">The response.</typeparam>
    public abstract class StepQueryResponseBase<QbResponse> : IStepQueryResponse
        where QbResponse : class, IQbResponse, new()
    {
        /// <summary>
        /// Constructs a StepQueryResponseBase.
        /// </summary>
        public StepQueryResponseBase()
        {
        }

        /// <summary>
        /// Returns the step name.
        /// </summary>
        /// <returns>Step name.</returns>
        public abstract string Name { get; }

        /// <summary>
        /// Called when the Web Connector returns some data.
        /// </summary>
        /// <param name="authenticatedTicket">The authenticated ticket.</param>
        /// <param name="response">QbXml.</param>
        /// <param name="hresult">HResult.</param>
        /// <param name="message">Message.</param>
        /// <returns>Message to be returned to the Web Connector.</returns>
        public virtual async Task<int> ReceiveXMLAsync(IAuthenticatedTicket authenticatedTicket, string response, string hresult, string message)
        {
            var responseObject = new QbXmlResponse();

            if (!string.IsNullOrEmpty(response))
            {
                if (responseObject.GetSingleItemFromResponse(response, typeof(QbResponse)) is QbResponse msgResponseObject)
                {
                    await ExecuteResponseAsync(authenticatedTicket, msgResponseObject);
                }

                return 0;
            }

            return -1;
        }

        /// <summary>
        /// After receiving XML from the Web Connector, the step can decide to go to a specific step.
        /// If you return a non null step, we will go to that step.
        /// </summary>
        /// <returns>Step name to go to. Null to continue.</returns>
        public virtual Task<string?> GotoStepAsync()
        {
            return Task.FromResult<string?>(null);
        }

        /// <summary>
        /// After receiving XML from the Web Connector, we check if we should move to the next step
        /// by calling this method. Usually, you want to move to the next step unless your step
        /// has other messages to send to the Web Connector. It is the case when you use an iterator.
        /// </summary>
        /// <returns>False stays on the current step. True goes to the next step.</returns>
        public virtual Task<bool> GotoNextStepAsync()
        {
            return Task.FromResult(true);
        }

        /// <summary>
        /// Called when the Web Connector detected an error.
        /// You can return a message to be displayed to the user.
        /// </summary>
        /// <param name="authenticatedTicket">The authenticated ticket.</param>
        /// <returns>Message to be displayed to the user.</returns>
        public virtual Task<string> LastErrorAsync(IAuthenticatedTicket authenticatedTicket)
        {
            return Task.FromResult(string.Empty);
        }

        /// <summary>
        /// Executes the response, useful to save the data coming from QuickBooks.
        /// </summary>
        /// <param name="authenticatedTicket">The ticket</param>
        /// <param name="response">The QbResponse</param>
        /// <returns>Task.</returns>
        protected virtual Task ExecuteResponseAsync(IAuthenticatedTicket authenticatedTicket, QbResponse response)
        {
            return Task.FromResult<object?>(null);
        }
    }
}
