using QbSync.WebConnector.Core;
using System.Threading.Tasks;

namespace QbSync.WebConnector.Impl
{
    /// <summary>
    /// Message validator that does nothing, and accept all messages.
    /// </summary>
    public class MessageValidatorNoop : IMessageValidator
    {
        /// <summary>
        /// Verifies if the version and filename is allowed to start exchanging information.
        /// If the version is incorrect, return false.
        /// Another message will be sent to "IsValidTicket" with the ticket only to check if the
        /// version is allowed.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <param name="strCompanyFileName">Company File Name.</param>
        /// <param name="qbXMLCountry">Country code.</param>
        /// <param name="qbXMLMajorVers">QbXml Major Version.</param>
        /// <param name="qbXMLMinorVers">QbXml Minor Version.</param>
        /// <returns>False if the version is too low.</returns>
        public Task<bool> ValidateMessageAsync(string ticket, string strCompanyFileName, string qbXMLCountry, int qbXMLMajorVers, int qbXMLMinorVers)
        {
            return Task.FromResult(true);
        }

        /// <summary>
        /// Returns false if the ticket has been marked as invalid version.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <returns>False if the version is too low for the ticket.</returns>
        public Task<bool> IsValidTicketAsync(string ticket)
        {
            return Task.FromResult(true);
        }
    }
}
