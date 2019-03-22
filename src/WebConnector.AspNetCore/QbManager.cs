using Microsoft.Extensions.Logging;
using QbSync.QbXml;
using QbSync.WebConnector.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace QbSync.WebConnector.AspNetCore
{
    public class QbManager : IQbManager
    {
        internal const string FINISHED_STEP = "##FINISHED##";

        protected readonly IAuthenticator authenticator;
        protected readonly IMessageValidator messageValidator;
        protected readonly IWebConnectorHandler webConnectorHandler;
        protected readonly IEnumerable<IStepQueryRequest> stepRequest;
        protected readonly IEnumerable<IStepQueryResponse> stepResponse;
        protected readonly ILogger<QbManager> logger;

        public QbManager(
            IAuthenticator authenticator,
            IMessageValidator messageValidator,
            IWebConnectorHandler webConnectorHandler,
            IEnumerable<IStepQueryRequest> stepRequest,
            IEnumerable<IStepQueryResponse> stepResponse,
            ILogger<QbManager> logger
        )
        {
            this.authenticator = authenticator;
            this.messageValidator = messageValidator;
            this.webConnectorHandler = webConnectorHandler;
            this.stepRequest = stepRequest;
            this.stepResponse = stepResponse;
            this.logger = logger;
        }

        /// <summary>
        /// Authenticate a login/password and return important information regarding if more requests
        /// should be executed immediately.
        /// </summary>
        /// <param name="login">Login.</param>
        /// <param name="password">Password.</param>
        /// <returns>Array of 4 strings. 0: ticket; 1: nvu if invalid user, or empty string if valid; 2: time to wait in seconds before coming back; 3: not used</returns>
        public virtual async Task<string[]> AuthenticateAsync(string login, string password)
        {
            try
            {
                var authenticatedTicket = await authenticator.GetAuthenticationFromLoginAsync(login, password);

                if (authenticatedTicket == null)
                {
                    throw new Exception($"{nameof(IAuthenticator.GetAuthenticationFromLoginAsync)} must return a ticket.");
                }

                try
                {
                    LogMessage(authenticatedTicket, LogMessageType.Authenticate, LogDirection.In, authenticatedTicket.Ticket, login, password);

                    var ret = await AuthenticateInternalAsync(authenticatedTicket);

                    LogMessage(authenticatedTicket, LogMessageType.Authenticate, LogDirection.Out, authenticatedTicket.Ticket, ret);

                    return ret;
                }
                catch (Exception ex)
                {
                    throw new QbSyncException(authenticatedTicket, ex);
                }
                finally
                {
                    await SaveChangesAsync(authenticatedTicket);
                }
            }
            catch (QbSyncException ex)
            {
                await OnExceptionAsync(ex.Ticket, ex);
            }
            catch (Exception ex)
            {
                await OnExceptionAsync(null, ex);
            }

            return null;
        }

        /// <summary>
        /// Returns the server version to the Web Connector.
        /// </summary>
        /// <returns>Server version.</returns>
        public virtual string ServerVersion()
        {
            return Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }

        /// <summary>
        /// Indicates which version the Web Connector is using.
        /// </summary>
        /// <param name="version">Web Connector Client version.</param>
        /// <returns>An empty string if everything is fine, W:&lt;message&gt; if warning; E:&lt;message&gt; if error.</returns>
        public virtual string ClientVersion(string version)
        {
            var requiredVersion = GetMinimumRequiredVersion();
            var receivedVersion = new Version(version);

            if (receivedVersion < requiredVersion)
            {
                return "W:You must update this WebConnector to a newer version.";
            }

            return string.Empty;
        }

        /// <summary>
        /// The Web Connector is asking what has to be done to its database. Return an XML command.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <param name="strHCPResponse">First time use, an XML containing information about the QuickBooks client database.</param>
        /// <param name="strCompanyFileName">The QuickBooks file opened on the client.</param>
        /// <param name="qbXMLCountry">Country code.</param>
        /// <param name="qbXMLMajorVers">QbXml Major Version.</param>
        /// <param name="qbXMLMinorVers">QbXml Minor Version.</param>
        /// <returns></returns>
        public virtual async Task<string> SendRequestXMLAsync(string ticket, string strHCPResponse, string strCompanyFileName, string qbXMLCountry, int qbXMLMajorVers, int qbXMLMinorVers)
        {
            try
            {
                var authenticatedTicket = await authenticator.GetAuthenticationFromTicketAsync(ticket);

                try
                {
                    LogMessage(authenticatedTicket, LogMessageType.Send, LogDirection.In, ticket, strHCPResponse, strCompanyFileName, qbXMLCountry, qbXMLMajorVers.ToString(), qbXMLMinorVers.ToString());

                    if (!string.IsNullOrWhiteSpace(strHCPResponse))
                    {
                        await ProcessClientInformationAsync(authenticatedTicket, strHCPResponse);
                    }

                    var result = string.Empty;
                    if (authenticatedTicket != null)
                    {
                        // Check the version, if we can't have the minimum version, we must fail.
                        if (messageValidator != null && !(await messageValidator.ValidateMessageAsync(authenticatedTicket.Ticket, strCompanyFileName, qbXMLCountry, qbXMLMajorVers, qbXMLMinorVers)))
                        {
                            result = string.Empty;
                        }
                        else
                        {
                            IStepQueryRequest stepQueryRequest = null;
                            while ((stepQueryRequest = FindStepRequest(authenticatedTicket.CurrentStep)) != null)
                            {
                                if (string.IsNullOrEmpty(authenticatedTicket.CurrentStep))
                                {
                                    authenticatedTicket.CurrentStep = stepQueryRequest.Name;
                                }

                                result = await stepQueryRequest.SendXMLAsync(authenticatedTicket);

                                if (result == null)
                                {
                                    authenticatedTicket.CurrentStep = FindNextStepName(authenticatedTicket.CurrentStep);
                                }
                                else
                                {
                                    break;
                                }
                            }

                            // If we don't have more steps to execute, let's return nothing.
                            if (result == null)
                            {
                                result = string.Empty;
                            }
                        }
                    }

                    LogMessage(authenticatedTicket, LogMessageType.Send, LogDirection.Out, ticket, result);

                    return result;
                }
                catch (Exception ex)
                {
                    throw new QbSyncException(authenticatedTicket, ex);
                }
                finally
                {
                    await SaveChangesAsync(authenticatedTicket);
                }
            }
            catch (QbSyncException ex)
            {
                await OnExceptionAsync(ex.Ticket, ex);
            }
            catch (Exception ex)
            {
                await OnExceptionAsync(null, ex);
            }

            return null;
        }

        /// <summary>
        /// Response from the Web Connector based on the previous comamnd sent.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <param name="response">The XML response.</param>
        /// <param name="hresult">Code in case of an error.</param>
        /// <param name="message">Human message in case of an error.</param>
        /// <returns></returns>
        public virtual async Task<int> ReceiveRequestXMLAsync(string ticket, string response, string hresult, string message)
        {
            try
            {
                var authenticatedTicket = await authenticator.GetAuthenticationFromTicketAsync(ticket);

                try
                {
                    LogMessage(authenticatedTicket, LogMessageType.Receive, LogDirection.In, ticket, response, hresult, message);

                    var result = -1;

                    if (authenticatedTicket != null)
                    {
                        var stepQueryResponse = FindStepResponse(authenticatedTicket.CurrentStep);
                        if (stepQueryResponse != null)
                        {
                            result = await stepQueryResponse.ReceiveXMLAsync(authenticatedTicket, response, hresult, message);

                            if (result >= 0)
                            {
                                var stepName = await stepQueryResponse.GotoStepAsync();

                                // We go to the next step if we are asked to
                                if (!string.IsNullOrEmpty(stepName))
                                {
                                    authenticatedTicket.CurrentStep = stepName;
                                }
                                else if (await stepQueryResponse.GotoNextStepAsync())
                                {
                                    authenticatedTicket.CurrentStep = FindNextStepName(authenticatedTicket.CurrentStep);
                                }
                            }
                        }
                    }

                    LogMessage(authenticatedTicket, LogMessageType.Receive, LogDirection.Out, ticket, result.ToString());

                    return result;
                }
                catch (Exception ex)
                {
                    throw new QbSyncException(authenticatedTicket, ex);
                }
                finally
                {
                    await SaveChangesAsync(authenticatedTicket);
                }
            }
            catch (QbSyncException ex)
            {
                await OnExceptionAsync(ex.Ticket, ex);
            }
            catch (Exception ex)
            {
                await OnExceptionAsync(null, ex);
            }

            return -1;
        }

        /// <summary>
        /// Gets the last error that happened. This method is called only if an error is found.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <returns>Tell the Web Connector what is the error.</returns>
        public virtual async Task<string> GetLastErrorAsync(string ticket)
        {
            try
            {
                var authenticatedTicket = await authenticator.GetAuthenticationFromTicketAsync(ticket);

                try
                {
                    LogMessage(authenticatedTicket, LogMessageType.GetError, LogDirection.In, ticket);

                    var result = string.Empty;

                    if (authenticatedTicket != null)
                    {
                        if (messageValidator != null && !(await messageValidator.IsValidTicketAsync(authenticatedTicket.Ticket)))
                        {
                            result = string.Format("The server requires QbXml version {0}.{1} or higher. Please upgrade QuickBooks.", QbXmlRequest.VERSION.Major, QbXmlRequest.VERSION.Minor);
                        }
                        else
                        {
                            var stepQueryResponse = FindStepResponse(authenticatedTicket.CurrentStep);
                            if (stepQueryResponse != null)
                            {
                                result = await stepQueryResponse.LastErrorAsync(authenticatedTicket);
                            }
                        }
                    }

                    LogMessage(authenticatedTicket, LogMessageType.GetError, LogDirection.Out, ticket, result);

                    return result;
                }
                catch (Exception ex)
                {
                    throw new QbSyncException(authenticatedTicket, ex);
                }
                finally
                {
                    await SaveChangesAsync(authenticatedTicket);
                }
            }
            catch (QbSyncException ex)
            {
                await OnExceptionAsync(ex.Ticket, ex);
            }
            catch (Exception ex)
            {
                await OnExceptionAsync(null, ex);
            }

            return null;
        }

        /// <summary>
        /// An error happened with the Web Conenctor.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <param name="hresult">Code in case of an error.</param>
        /// <param name="message">Human message in case of an error.</param>
        /// <returns>Tell the Web Connector what is the error.</returns>
        public virtual async Task<string> ConnectionErrorAsync(string ticket, string hresult, string message)
        {
            try
            {
                var authenticatedTicket = await authenticator.GetAuthenticationFromTicketAsync(ticket);

                try
                {
                    LogMessage(authenticatedTicket, LogMessageType.Error, LogDirection.In, ticket, hresult, message);

                    var result = "done";

                    if (authenticatedTicket != null)
                    {
                    }

                    LogMessage(authenticatedTicket, LogMessageType.Error, LogDirection.Out, ticket, result);

                    return result;
                }
                catch (Exception ex)
                {
                    throw new QbSyncException(authenticatedTicket, ex);
                }
                finally
                {
                    await SaveChangesAsync(authenticatedTicket);
                }
            }
            catch (QbSyncException ex)
            {
                await OnExceptionAsync(ex.Ticket, ex);
            }
            catch (Exception ex)
            {
                await OnExceptionAsync(null, ex);
            }

            return null;
        }

        /// <summary>
        /// Closing the conneciton.
        /// </summary>
        /// <param name="ticket">The ticket</param>
        /// <returns>String to display to the user.</returns>
        public virtual async Task<string> CloseConnectionAsync(string ticket)
        {
            try
            {
                var authenticatedTicket = await authenticator.GetAuthenticationFromTicketAsync(ticket);

                try
                {
                    LogMessage(authenticatedTicket, LogMessageType.Close, LogDirection.In, ticket);

                    var result = "Invalid Ticket";
                    var logMessageType = LogMessageType.Error;

                    if (authenticatedTicket != null)
                    {
                        result = "Sync Completed";
                        logMessageType = LogMessageType.Close;
                    }

                    await webConnectorHandler.CloseConnectionAsync(authenticatedTicket);

                    LogMessage(authenticatedTicket, logMessageType, LogDirection.Out, ticket, result);

                    return result;
                }
                catch (Exception ex)
                {
                    throw new QbSyncException(authenticatedTicket, ex);
                }
                finally
                {
                    await SaveChangesAsync(authenticatedTicket);
                }
            }
            catch (QbSyncException ex)
            {
                await OnExceptionAsync(ex.Ticket, ex);
            }
            catch (Exception ex)
            {
                await OnExceptionAsync(null, ex);
            }

            return null;
        }

        /// <summary>
        /// Called when an exception occurs.
        /// </summary>
        /// <param name="ticket">The ticket if found. It might be null if no ticket has been provided or if the code failed when trying to create the authenticated ticket.</param>
        /// <param name="exception">Exception.</param>
        protected internal virtual Task OnExceptionAsync(IAuthenticatedTicket ticket, Exception exception)
        {
            return webConnectorHandler.OnExceptionAsync(ticket, exception);
        }

        /// <summary>
        /// Logs messages to a database.
        /// </summary>
        /// <param name="authenticatedTicket">The ticket which has the ticket string. The ticket could also be not authenticated if an error happened.</param>
        /// <param name="messageType">Type of message.</param>
        /// <param name="direction">Direction of the message (In the WebService, or Out the WebService).</param>
        /// <param name="arguments">Other arguments to save.</param>
        protected internal virtual void LogMessage(IAuthenticatedTicket authenticatedTicket, LogMessageType messageType, LogDirection direction, string ticket, params string[] arguments)
        {
            logger.LogTrace("Ticket: {TICKET}; Type: {TYPE}; Direction: {DIRECTION}; Arguments: {Arguments}", ticket, messageType, direction, arguments);
        }

        /// <summary>
        /// Implement this function in order to save the states to your database.
        /// </summary>
        protected internal virtual Task SaveChangesAsync(IAuthenticatedTicket authenticatedTicket)
        {
            return authenticator.SaveTicketAsync(authenticatedTicket);
        }

        /// <summary>
        /// Processes the response sent for the first time by the WebConnector.
        /// </summary>
        /// <param name="authenticatedTicket">The ticket.</param>
        /// <param name="response">First Message.</param>
        protected virtual Task ProcessClientInformationAsync(IAuthenticatedTicket authenticatedTicket, string response)
        {
            return webConnectorHandler.ProcessClientInformationAsync(authenticatedTicket, response);
        }
        
        protected internal async Task<string[]> AuthenticateInternalAsync(IAuthenticatedTicket authenticatedTicket)
        {
            var ret = new string[4];
            ret[0] = authenticatedTicket.Ticket;
            ret[2] = string.Empty;
            ret[3] = string.Empty; // Not used

            if (authenticatedTicket.Authenticated == false)
            {
                ret[1] = "nvu"; // Invalid user
            }
            else
            {
                var waitTime = await GetWaitTimeAsync(authenticatedTicket);
                if (waitTime == 0)
                {
                    ret[1] = await GetCompanyFileAsync(authenticatedTicket);
                }
                else
                {
                    ret[1] = "none"; // No work is necessary
                    ret[2] = waitTime.ToString();
                }
            }

            return ret;
        }

        /// <summary>
        /// Returns the minimum version the Web Connector has to support.
        /// </summary>
        /// <returns>Minimum version.</returns>
        protected internal virtual Version GetMinimumRequiredVersion()
        {
            return new Version(2, 1, 0, 30);
        }

        /// <summary>
        /// Indicates if there is anything to be done with the current ticket.
        /// Return 0 if you have work to do immediately. Otherwise return the number of seconds
        /// when you want the Web Connector to come back.
        /// </summary>
        /// <returns>Number of seconds to wait before the Web Connector comes back, or 0 to do some work right now.</returns>
        protected internal virtual Task<int> GetWaitTimeAsync(IAuthenticatedTicket authenticatedTicket)
        {
            return webConnectorHandler.GetWaitTimeAsync(authenticatedTicket);
        }

        /// <summary>
        /// Returns the path where the client company file is located.
        /// Override this method if you wish to open a different file than the current one open on the client.
        /// </summary>
        /// <param name="authenticatedTicket">The ticket.</param>
        /// <returns>Path where company file is located.</returns>
        protected internal virtual Task<string> GetCompanyFileAsync(IAuthenticatedTicket authenticatedTicket)
        {
            return webConnectorHandler.GetCompanyFileAsync(authenticatedTicket);
        }

        protected internal IStepQueryRequest FindStepRequest(string step)
        {
            if (string.IsNullOrEmpty(step))
            {
                return stepRequest.FirstOrDefault();
            }

            return stepRequest.FirstOrDefault(s => s.Name == step);
        }
        
        protected internal IStepQueryResponse FindStepResponse(string step)
        {
            if (string.IsNullOrEmpty(step))
            {
                return stepResponse.FirstOrDefault();
            }

            return stepResponse.FirstOrDefault(s => s.Name == step);
        }

        /// <summary>
        /// Finds the next step after the step name.
        /// </summary>
        /// <param name="step">Step name.</param>
        /// <returns>The next step name or null if none.</returns>
        protected internal string FindNextStepName(string step)
        {
            var hasStep = false;
            using (var enumerator = stepRequest.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    if (string.IsNullOrEmpty(step) || enumerator.Current.Name == step)
                    {
                        hasStep = enumerator.MoveNext();
                        break;
                    }
                }

                if (hasStep)
                {
                    return enumerator.Current.Name;
                }

                return FINISHED_STEP;
            }
        }
    }
}