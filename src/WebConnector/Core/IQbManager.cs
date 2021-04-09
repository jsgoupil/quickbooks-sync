using System.ServiceModel;
using System.Threading.Tasks;

namespace QbSync.WebConnector.Core
{
    /// <summary>
    /// The main interface for exchanging messages with the WebConnector.
    /// </summary>
    [ServiceContract(Namespace = "http://developer.intuit.com/")]
    public interface IQbManager
    {
        /// <summary>
        /// Authenticates a login/password and return important information regarding if more requests
        /// should be executed immediately.
        /// </summary>
        /// <param name="strUserName">Login.</param>
        /// <param name="strPassword">Password.</param>
        /// <returns>Array of 4 strings. 0: ticket; 1: nvu if invalid user, or empty string if valid; 2: time to wait in seconds before coming back; 3: not used</returns>
        [OperationContract(Action = "http://developer.intuit.com/authenticate", Name = "authenticate")]
        Task<string[]?> AuthenticateAsync(string strUserName, string strPassword);

        /// <summary>
        /// Returns the server version to the Web Connector.
        /// </summary>
        /// <returns>Server version.</returns>
        [OperationContract(Action = "http://developer.intuit.com/serverVersion", Name = "serverVersion")]
        string ServerVersion();

        /// <summary>
        /// Indicates which version the Web Connector is using.
        /// </summary>
        /// <param name="strVersion">Web Connector Client version.</param>
        /// <returns>An empty string if everything is fine, W:&lt;message&gt; if warning; E:&lt;message&gt; if error.</returns>
        [OperationContract(Action = "http://developer.intuit.com/clientVersion", Name = "clientVersion")]
        string ClientVersion(string strVersion);

        /// <summary>
        /// An error happened with the Web Conenctor.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <param name="hresult">Code in case of an error.</param>
        /// <param name="message">Human message in case of an error.</param>
        /// <returns>Tell the Web Connector what is the error.</returns>
        [OperationContract(Action = "http://developer.intuit.com/connectionError", Name = "connectionError")]
        Task<string?> ConnectionErrorAsync(string ticket, string hresult, string message);

        /// <summary>
        /// The Web Connector is asking what has to be done to its database. Return an XML command.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <param name="strHCPResponse">First time use, an XML containing information about the QuickBooks client database.</param>
        /// <param name="strCompanyFileName">The QuickBooks file opened on the client.</param>
        /// <param name="qbXMLCountry">Country code.</param>
        /// <param name="qbXMLMajorVers">QbXml Major Version.</param>
        /// <param name="qbXMLMinorVers">QbXml Minor Version.</param>
        /// <returns>XML command.</returns>
        [OperationContract(Action = "http://developer.intuit.com/sendRequestXML", Name = "sendRequestXML")]
        Task<string?> SendRequestXMLAsync(string ticket, string strHCPResponse, string strCompanyFileName, string qbXMLCountry, int qbXMLMajorVers, int qbXMLMinorVers);

        /// <summary>
        /// Response from the Web Connector based on the previous comamnd sent.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <param name="response">The XML response.</param>
        /// <param name="hresult">Code in case of an error.</param>
        /// <param name="message">Human message in case of an error.</param>
        /// <returns>Message to be returned to the Web Connector.</returns>
        [OperationContract(Action = "http://developer.intuit.com/receiveResponseXML", Name = "receiveResponseXML")]
        Task<int> ReceiveRequestXMLAsync(string ticket, string response, string hresult, string message);

        /// <summary>
        /// Gets the last error that happened. This method is called only if an error is found.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <returns>Tell the Web Connector what is the error.</returns>
        [OperationContract(Action = "http://developer.intuit.com/getLastError", Name = "getLastError")]
        Task<string?> GetLastErrorAsync(string ticket);

        /// <summary>
        /// Closing the conneciton.
        /// </summary>
        /// <param name="ticket">The ticket</param>
        /// <returns>String to display to the user.</returns>
        [OperationContract(Action = "http://developer.intuit.com/closeConnection", Name = "closeConnection")]
        Task<string?> CloseConnectionAsync(string ticket);
    }
}
