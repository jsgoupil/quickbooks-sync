using QbSync.WebConnector.Core;
using System.Threading.Tasks;

namespace QbSync.WebConnector.Impl
{
    public class MessageValidatorNoop : IMessageValidator
    {
        public Task<bool> ValidateMessageAsync(string ticket, string strCompanyFileName, string qbXMLCountry, int qbXMLMajorVers, int qbXMLMinorVers)
        {
            return Task.FromResult(true);
        }

        public Task<bool> IsValidTicketAsync(string ticket)
        {
            return Task.FromResult(true);
        }
    }
}
