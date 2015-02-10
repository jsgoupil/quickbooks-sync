using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace QbSync.WebConnector
{
    public class SyncManager
    {
        public delegate StepQueryResponse StepInvoker(AuthenticatedTicket authenticatedTicket);

        protected IAuthenticator authenticator;

        public SyncManager(IAuthenticator authenticator)
        {
            this.authenticator = authenticator;
            StepSync = new Dictionary<int, Type>();
        }

        internal IDictionary<int, Type> StepSync
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
            var authenticatedTicket = authenticator.GetAuthenticationFromLogin(login, password);
            if (authenticatedTicket == null)
            {
                throw new Exception("GetAuthenticationFromLogin must return a ticket.");
            }

            LogMessage(authenticatedTicket, LogMessageType.Authenticate, LogDirection.In, authenticatedTicket.Ticket, login, password);

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
                var waitTime = GetWaitTime(authenticatedTicket);
                if (waitTime == 0)
                {
                    ret[1] = string.Empty; // Use the company that is opened on the client.
                }
                else
                {
                    ret[1] = "none"; // No work is necessary
                    ret[2] = waitTime.ToString();
                }
            }

            LogMessage(authenticatedTicket, LogMessageType.Authenticate, LogDirection.Out, authenticatedTicket.Ticket, ret);
            SaveChanges();
            return ret;
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
            var authenticatedTicket = authenticator.GetAuthenticationFromTicket(ticket);
            LogMessage(authenticatedTicket, LogMessageType.Send, LogDirection.In, ticket, strHCPResponse, strCompanyFileName, qbXMLCountry, qbXMLMajorVers.ToString(), qbXMLMinorVers.ToString());

            var result = string.Empty;

            if (authenticatedTicket != null)
            {
                Type objectType = null;
                while (StepSync.TryGetValue(authenticatedTicket.CurrentStep, out objectType))
                {
                    var stepQueryResponse = Invoke(objectType, authenticatedTicket);
                    result = stepQueryResponse.SendXML();

                    if (result == null)
                    {
                        authenticatedTicket.CurrentStep++;
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

            LogMessage(authenticatedTicket, LogMessageType.Send, LogDirection.Out, ticket, result);
            SaveChanges();

            return result;
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
            var authenticatedTicket = authenticator.GetAuthenticationFromTicket(ticket);
            LogMessage(authenticatedTicket, LogMessageType.Receive, LogDirection.In, ticket, response, hresult, message);

            var result = -1;

            if (authenticatedTicket != null)
            {
                Type objectType = null;
                if (StepSync.TryGetValue(authenticatedTicket.CurrentStep, out objectType))
                {
                    var stepQueryResponse = Invoke(objectType, authenticatedTicket);
                    result = stepQueryResponse.ReceiveXML(response, hresult, message);

                    if (result >= 0)
                    {
                        // We go to the next step if we are asked to
                        if (stepQueryResponse.GotoNextStep())
                        {
                            authenticatedTicket.CurrentStep++;
                        }
                    }
                }
            }

            LogMessage(authenticatedTicket, LogMessageType.Receive, LogDirection.Out, ticket, result.ToString());
            SaveChanges();

            return result;
        }

        /// <summary>
        /// Gets the last error that happened. This method is called only if an error is found.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <returns>Tell the Web Connector what is the error.</returns>
        public virtual string GetLastError(string ticket)
        {
            var authenticatedTicket = authenticator.GetAuthenticationFromTicket(ticket);
            LogMessage(authenticatedTicket, LogMessageType.GetError, LogDirection.In, ticket);

            var result = string.Empty;

            if (authenticatedTicket != null)
            {
                Type objectType = null;
                if (StepSync.TryGetValue(authenticatedTicket.CurrentStep, out objectType))
                {
                    var stepQueryResponse = Invoke(objectType, authenticatedTicket);
                    result = stepQueryResponse.LastError();
                }

            }

            LogMessage(authenticatedTicket, LogMessageType.GetError, LogDirection.Out, ticket, result);
            SaveChanges();

            return result;
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
            var authenticatedTicket = authenticator.GetAuthenticationFromTicket(ticket);
            LogMessage(authenticatedTicket, LogMessageType.Error, LogDirection.In, ticket, hresult, message);

            var result = "done";

            if (authenticatedTicket != null)
            {
            }

            LogMessage(authenticatedTicket, LogMessageType.Error, LogDirection.Out, ticket, result);
            SaveChanges();

            return result;
        }

        /// <summary>
        /// Closing the conneciton.
        /// </summary>
        /// <param name="ticket">The ticket</param>
        /// <returns>String to display to the user.</returns>
        public virtual string CloseConnection(string ticket)
        {
            var authenticatedTicket = authenticator.GetAuthenticationFromTicket(ticket);
            LogMessage(authenticatedTicket, LogMessageType.Close, LogDirection.In, ticket);

            var result = "Invalid Ticket";

            if (authenticatedTicket != null)
            {
                result = "Sync Completed";
            }

            LogMessage(authenticatedTicket, LogMessageType.Error, LogDirection.Out, ticket, result);
            SaveChanges();

            return result;
        }

        /// <summary>
        /// Registers a step to use.
        /// </summary>
        /// <param name="stepNumber">Step Number.</param>
        /// <param name="type">Type to create.</param>
        public void RegisterStep(int stepNumber, Type type)
        {
            if (!type.GetInterfaces().Contains(typeof(StepQueryResponse)))
            {
                throw new Exception("The type " + type.FullName + " does not implement StepQueryResponse.");
            }

            StepSync[stepNumber] = type;
        }

        /// <summary>
        /// Invoke a step based on the previously registered type.
        /// </summary>
        /// <param name="type">Type.</param>
        /// <param name="authenticatedTicket">The authenticated ticket.</param>
        /// <returns>Created step.</returns>
        protected internal virtual StepQueryResponse Invoke(Type type, AuthenticatedTicket authenticatedTicket)
        {
            return Activator.CreateInstance(type, authenticatedTicket) as StepQueryResponse;
        }

        /// <summary>
        /// Indicates if there is anything to be done with the current ticket.
        /// Return 0 if you have work to do immediately. Otherwise return the number of seconds
        /// when you want the Web Connector to come back.
        /// </summary>
        /// <returns>Number of seconds to wait before the Web Connector comes back, or 0 to do some work right now.</returns>
        protected internal virtual int GetWaitTime(AuthenticatedTicket authenticatedTicket)
        {
            return 0;
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
        /// Returns the minimum version the Web Connector has to support.
        /// </summary>
        /// <returns>Minimum version.</returns>
        protected internal virtual Version GetMinimumRequiredVersion() {
            return new Version(2, 1, 0, 30);
        }
    }
}
