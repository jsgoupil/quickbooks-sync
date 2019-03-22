using QbSync.WebConnector.Models;

namespace QbSync.WebConnector.Core
{
    /// <summary>
    /// Service handling the generation of the QWC File.
    /// </summary>
    public interface IWebConnectorQwc
    {
        /// <summary>
        /// Gets the QWC file as a string.
        /// </summary>
        /// <param name="model">The options to configure the QWC file.</param>
        /// <returns>XML string.</returns>
        string GetQwcFile(WebConnectorQwcModel model);
    }
}
