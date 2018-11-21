using QbSync.WebConnector.Core;
using System.Threading.Tasks;

namespace QbSync.WebConnector.Impl
{
    public class VersionValidatorNoop : IVersionValidator
    {
        public Task<bool> IsValidTicketAsync(string ticket)
        {
            return Task.FromResult(true);
        }

        public Task<bool> ValidateVersionAsync(string ticket, string qbXMLCountry, int qbXMLMajorVers, int qbXMLMinorVers)
        {
            return Task.FromResult(true);
        }
    }
}
