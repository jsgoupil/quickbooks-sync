using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QbSync.WebConnector
{
    public abstract class QbManagerBase<T>
        where T : IStepQueryResponseBase
    {
        protected internal List<T> Steps
        {
            get;
            set;
        }

        /// <summary>
        /// Registers a step to use.
        /// </summary>
        /// <param name="stepQueryResponse">Step.</param>
        public void RegisterStep(T stepQueryResponse)
        {
            Steps.Add(stepQueryResponse);
        }

        protected internal string[] AuthenticateInternal(AuthenticatedTicket authenticatedTicket)
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
                var waitTime = GetWaitTime(authenticatedTicket);
                if (waitTime == 0)
                {
                    ret[1] = GetCompanyFile(authenticatedTicket);
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
        protected internal virtual int GetWaitTime(AuthenticatedTicket authenticatedTicket)
        {
            return 0;
        }

        /// <summary>
        /// Returns the path where the client company file is located.
        /// Override this method if you wish to open a different file than the current one open on the client.
        /// </summary>
        /// <param name="authenticatedTicket">The ticket.</param>
        /// <returns>Path where company file is located.</returns>
        protected internal virtual string GetCompanyFile(AuthenticatedTicket authenticatedTicket)
        {
            return string.Empty; // Use the company that is opened on the client.
        }

        /// <summary>
        /// Finds the step based on its name.
        /// </summary>
        /// <param name="step">Step name.</param>
        /// <returns>The step associated with the name.</returns>
        protected internal T FindStep(string step)
        {
            if (step == AuthenticatedTicket.InitialStep)
            {
                return Steps.FirstOrDefault();
            }

            return Steps.FirstOrDefault(s => s.Name == step);
        }

        /// <summary>
        /// Finds the next step after the step name.
        /// </summary>
        /// <param name="step">Step name.</param>
        /// <returns>The next step name or null if none.</returns>
        protected internal string FindNextStepName(string step)
        {
            if (step == AuthenticatedTicket.InitialStep)
            {
                if (Steps.Count >= 2)
                {
                    return Steps[1].Name;
                }
            }

            for (var i = 0; i < Steps.Count; i++)
            {
                if (Steps[i].Name == step)
                {
                    if (i + 1 < Steps.Count)
                    {
                        return Steps[i + 1].Name;
                    }
                }
            }

            return null;
        }
    }
}
