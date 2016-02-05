using QbSync.QbXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace QbSync.WebConnector.Synchronous
{
    public class QbManager : QbManagerBase<IStepQueryResponse>
    {
        protected IAuthenticator authenticator;

        public QbManager(IAuthenticator authenticator)
        {
            this.authenticator = authenticator;
            Steps = new List<IStepQueryResponse>();
        }

        public IVersionValidator VersionValidator
        {
            get;
            set;
        }

        /// <summary>
        /// Authenticate a login/password and return important information regarding if more requests
        /// should be executed immediately.
        /// </summary>
        /// <param name="login">Login.</param>
        /// <param name="password">Password.</param>
        /// <returns>Array of 4 strings. 0: ticket; 1: nvu if invalid user, or empty string if valid; 2: time to wait in seconds before coming back; 3: not used</returns>
        public virtual string[] Authenticate(string login, string password)
        {
            try
            {
                var authenticatedTicket = authenticator.GetAuthenticationFromLogin(login, password);
                if (authenticatedTicket == null)
                {
                    throw new Exception("GetAuthenticationFromLogin must return a ticket.");
                }

                try
                {
                    LogMessage(authenticatedTicket, LogMessageType.Authenticate, LogDirection.In, authenticatedTicket.Ticket, login, password);

                    var ret = base.AuthenticateInternal(authenticatedTicket);

                    LogMessage(authenticatedTicket, LogMessageType.Authenticate, LogDirection.Out, authenticatedTicket.Ticket, ret);

                    return ret;
                }
                catch (Exception ex)
                {
                    throw new QbSyncException(authenticatedTicket, ex);
                }
            }
            catch (QbSyncException ex)
            {
                OnException(ex.Ticket, ex);
            }
            catch (Exception ex)
            {
                OnException(null, ex);
            }
            finally
            {
                SaveChanges();
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
        public virtual string SendRequestXML(string ticket, string strHCPResponse, string strCompanyFileName, string qbXMLCountry, int qbXMLMajorVers, int qbXMLMinorVers)
        {
            try
            {
                var authenticatedTicket = authenticator.GetAuthenticationFromTicket(ticket);

                try
                {
                    LogMessage(authenticatedTicket, LogMessageType.Send, LogDirection.In, ticket, strHCPResponse, strCompanyFileName, qbXMLCountry, qbXMLMajorVers.ToString(), qbXMLMinorVers.ToString());

                    if (!string.IsNullOrWhiteSpace(strHCPResponse))
                    {
                        ProcessClientInformation(authenticatedTicket, strHCPResponse);
                    }

                    var result = string.Empty;
                    if (authenticatedTicket != null)
                    {
                        // Check the version, if we can't have the minimum version, we must fail.
                        if (VersionValidator != null && !VersionValidator.ValidateVersion(authenticatedTicket.Ticket, qbXMLCountry, qbXMLMajorVers, qbXMLMinorVers))
                        {
                            result = string.Empty;
                        }
                        else
                        {
                            IStepQueryResponse stepQueryResponse = null;
                            while ((stepQueryResponse = FindStep(authenticatedTicket.CurrentStep)) != null)
                            {
                                stepQueryResponse.SetOptions(GetOptions(authenticatedTicket));
                                result = stepQueryResponse.SendXML(authenticatedTicket);

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
            }
            catch (QbSyncException ex)
            {
                OnException(ex.Ticket, ex);
            }
            catch (Exception ex)
            {
                OnException(null, ex);
            }
            finally
            {
                SaveChanges();
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
        public virtual int ReceiveRequestXML(string ticket, string response, string hresult, string message)
        {
            try
            {
                var authenticatedTicket = authenticator.GetAuthenticationFromTicket(ticket);

                try
                {
                    LogMessage(authenticatedTicket, LogMessageType.Receive, LogDirection.In, ticket, response, hresult, message);

                    var result = -1;

                    if (authenticatedTicket != null)
                    {
                        var stepQueryResponse = FindStep(authenticatedTicket.CurrentStep);
                        if (stepQueryResponse != null)
                        {
                            stepQueryResponse.SetOptions(GetOptions(authenticatedTicket));
                            result = stepQueryResponse.ReceiveXML(authenticatedTicket, response, hresult, message);

                            if (result >= 0)
                            {
                                var stepName = stepQueryResponse.GotoStep();

                                // We go to the next step if we are asked to
                                if (!string.IsNullOrEmpty(stepName))
                                {
                                    authenticatedTicket.CurrentStep = stepName;
                                }
                                else if (stepQueryResponse.GotoNextStep())
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
            }
            catch (QbSyncException ex)
            {
                OnException(ex.Ticket, ex);
            }
            catch (Exception ex)
            {
                OnException(null, ex);
            }
            finally
            {
                SaveChanges();
            }

            return -1;
        }

        /// <summary>
        /// Gets the last error that happened. This method is called only if an error is found.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <returns>Tell the Web Connector what is the error.</returns>
        public virtual string GetLastError(string ticket)
        {
            try
            {
                var authenticatedTicket = authenticator.GetAuthenticationFromTicket(ticket);

                try
                {
                    LogMessage(authenticatedTicket, LogMessageType.GetError, LogDirection.In, ticket);

                    var result = string.Empty;

                    if (authenticatedTicket != null)
                    {
                        if (VersionValidator != null && !VersionValidator.IsValidTicket(authenticatedTicket.Ticket))
                        {
                            result = string.Format("The server requires QbXml version {0}.{1} or higher. Please upgrade QuickBooks.", QbXmlRequest.VERSION.Major, QbXmlRequest.VERSION.Minor);
                        }
                        else
                        {
                            var stepQueryResponse = FindStep(authenticatedTicket.CurrentStep);
                            if (stepQueryResponse != null)
                            {
                                result = stepQueryResponse.LastError(authenticatedTicket);
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
            }
            catch (QbSyncException ex)
            {
                OnException(ex.Ticket, ex);
            }
            catch (Exception ex)
            {
                OnException(null, ex);
            }
            finally
            {
                SaveChanges();
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
        public virtual string ConnectionError(string ticket, string hresult, string message)
        {
            try
            {
                var authenticatedTicket = authenticator.GetAuthenticationFromTicket(ticket);

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
            }
            catch (QbSyncException ex)
            {
                OnException(ex.Ticket, ex);
            }
            catch (Exception ex)
            {
                OnException(null, ex);
            }
            finally
            {
                SaveChanges();
            }

            return null;
        }

        /// <summary>
        /// Closing the conneciton.
        /// </summary>
        /// <param name="ticket">The ticket</param>
        /// <returns>String to display to the user.</returns>
        public virtual string CloseConnection(string ticket)
        {
            try
            {
                var authenticatedTicket = authenticator.GetAuthenticationFromTicket(ticket);

                try
                {
                    LogMessage(authenticatedTicket, LogMessageType.Close, LogDirection.In, ticket);

                    var logMessageType = LogMessageType.Error;
                    var result = "Invalid Ticket";

                    if (authenticatedTicket != null)
                    {
                        result = "Sync Completed";
                        logMessageType = LogMessageType.Close;
                    }

                    LogMessage(authenticatedTicket, logMessageType, LogDirection.Out, ticket, result);

                    return result;
                }
                catch (Exception ex)
                {
                    throw new QbSyncException(authenticatedTicket, ex);
                }
            }
            catch (QbSyncException ex)
            {
                OnException(ex.Ticket, ex);
            }
            catch (Exception ex)
            {
                OnException(null, ex);
            }
            finally
            {
                SaveChanges();
            }

            return null;
        }

        /// <summary>
        /// Called when an exception occurs.
        /// </summary>
        /// <param name="ticket">The ticket if found. It might be null if no ticket has been provided or if the code failed when trying to create the authenticated ticket.</param>
        /// <param name="exception">Exception.</param>
        protected internal virtual void OnException(AuthenticatedTicket ticket, Exception exception)
        {
        }

        /// <summary>
        /// Logs messages to a database.
        /// </summary>
        /// <param name="authenticatedTicket">The ticket which has the ticket string. The ticket could also be not authenticated if an error happened.</param>
        /// <param name="messageType">Type of message.</param>
        /// <param name="direction">Direction of the message (In the WebService, or Out the WebService).</param>
        /// <param name="arguments">Other arguments to save.</param>
        protected internal virtual void LogMessage(AuthenticatedTicket authenticatedTicket, LogMessageType messageType, LogDirection direction, string ticket, params string[] arguments)
        {
        }

        /// <summary>
        /// Implement this function in order to save the states to your database.
        /// </summary>
        protected internal virtual void SaveChanges()
        {
        }

        /// <summary>
        /// Processes the response sent for the first time by the WebConnector.
        /// </summary>
        /// <param name="authenticatedTicket">The ticket.</param>
        /// <param name="response">First Message.</param>
        protected virtual void ProcessClientInformation(AuthenticatedTicket authenticatedTicket, string response)
        {
        }

        /// <summary>
        /// Gets the options for the SyncManager.
        /// </summary>
        /// <param name="authenticatedTicket">The ticket.</param>
        /// <returns>Options.</returns>
        protected internal virtual QbXmlResponseOptions GetOptions(AuthenticatedTicket authenticatedTicket)
        {
            return null;
        }
    }
}