using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace QBSync.WebConnector
{
    public class SyncManager
    {
        public delegate StepQueryResponse StepInvoker(AuthenticatedTicket authenticatedTicket);

        private IAuthenticator authenticator;

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
                    ret[2] = waitTime.ToString(); // Come back in 1h
                }
            }

            LogMessage(authenticatedTicket, LogMessageType.Authenticate, LogDirection.Out, authenticatedTicket.Ticket, ret);
            SaveChanges();
            return ret;
        }

        public virtual string ServerVersion()
        {
            return Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }

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

        public void RegisterStep(int stepNumber, Type type)
        {
            if (!type.GetInterfaces().Contains(typeof(StepQueryResponse)))
            {
                throw new Exception("The type " + type.FullName + " does not implement StepQueryResponse.");
            }

            StepSync[stepNumber] = type;
        }

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
