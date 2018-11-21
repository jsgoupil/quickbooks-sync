using QbSync.WebConnector.Models;
using System.Threading.Tasks;

namespace QbSync.WebConnector.Core
{
    public interface IWebConnectorQwc
    {
        string GetQwcFile(WebConnectorQwcModel model);
    }
}
