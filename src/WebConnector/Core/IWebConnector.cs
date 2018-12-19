using QbSync.WebConnector.Models;

namespace QbSync.WebConnector.Core
{
    public interface IWebConnectorQwc
    {
        string GetQwcFile(WebConnectorQwcModel model);
    }
}
