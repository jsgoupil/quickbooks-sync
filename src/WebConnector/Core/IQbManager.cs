using System.ServiceModel;
using System.Threading.Tasks;

namespace QbSync.WebConnector.Core
{
    [ServiceContract(Namespace = "http://developer.intuit.com/")]
    public interface IQbManager
    {
        [OperationContract(Action = "http://developer.intuit.com/authenticate", Name = "authenticate")]
        Task<string[]> AuthenticateAsync(string strUserName, string strPassword);

        [OperationContract(Action = "http://developer.intuit.com/serverVersion", Name = "serverVersion")]
        string ServerVersion();

        [OperationContract(Action = "http://developer.intuit.com/clientVersion", Name = "clientVersion")]
        string ClientVersion(string strVersion);

        [OperationContract(Action = "http://developer.intuit.com/connectionError", Name = "connectionError")]
        Task<string> ConnectionErrorAsync(string ticket, string hresult, string message);

        [OperationContract(Action = "http://developer.intuit.com/sendRequestXML", Name = "sendRequestXML")]
        Task<string> SendRequestXMLAsync(string ticket, string strHCPResponse, string strCompanyFileName, string qbXMLCountry, int qbXMLMajorVers, int qbXMLMinorVers);

        [OperationContract(Action = "http://developer.intuit.com/receiveResponseXML", Name = "receiveResponseXML")]
        Task<int> ReceiveRequestXMLAsync(string ticket, string response, string hresult, string message);

        [OperationContract(Action = "http://developer.intuit.com/getLastError", Name = "getLastError")]
        Task<string> GetLastErrorAsync(string ticket);

        [OperationContract(Action = "http://developer.intuit.com/closeConnection", Name = "closeConnection")]
        Task<string> CloseConnectionAsync(string ticket);
    }
}
