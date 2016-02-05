using Nito.AsyncEx;
using System;
using System.Threading.Tasks;
using System.Web.Services;

namespace QbSync.WebConnector.Asynchronous
{
    [WebService(Namespace = "http://developer.intuit.com/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class QbConnector : QbWebConnectorSvc
    {
        public static Func<QbConnector, QbManager> QbManager
        {
            get;
            set;
        }

        public QbConnector()
        {
        }

        #region authenticate
        private Task<string[]> authenticateAsync(string strUserName, string strPassword)
        {
            return GetQbManager().AuthenticateAsync(strUserName, strPassword);
        }

        [WebMethod]
        public override IAsyncResult Beginauthenticate(string strUserName, string strPassword, AsyncCallback callback, object state)
        {
            return AsyncFactory<string[]>.ToBegin(authenticateAsync(strUserName, strPassword), callback, state);
        }

        [WebMethod]
        public override string[] Endauthenticate(IAsyncResult result)
        {
            return AsyncFactory<string[]>.ToEnd(result);
        }
        #endregion

        #region serverVersion
        [WebMethod]
        public override string serverVersion()
        {
            return GetQbManager().ServerVersion();
        }
        #endregion

        #region clientVersion
        [WebMethod]
        public override string clientVersion(string strVersion)
        {
            return GetQbManager().ClientVersion(strVersion);
        }
        #endregion

        #region connectionError
        private Task<string> connectionErrorAsync(string ticket, string hresult, string message)
        {
            return GetQbManager().ConnectionErrorAsync(ticket, hresult, message);
        }

        [WebMethod]
        public override IAsyncResult BeginconnectionError(string ticket, string hresult, string message, AsyncCallback callback, object state)
        {
            return AsyncFactory<string>.ToBegin(connectionErrorAsync(ticket, hresult, message), callback, state);
        }

        [WebMethod]
        public override string EndconnectionError(IAsyncResult result)
        {
            return AsyncFactory<string>.ToEnd(result);
        }
        #endregion

        #region sendRequestXML
        private Task<string> sendRequestXMLAsync(string ticket, string strHCPResponse, string strCompanyFileName, string qbXMLCountry, int qbXMLMajorVers, int qbXMLMinorVers)
        {
            return GetQbManager().SendRequestXMLAsync(ticket, strHCPResponse, strCompanyFileName, qbXMLCountry, qbXMLMajorVers, qbXMLMinorVers);
        }

        [WebMethod]
        public override IAsyncResult BeginsendRequestXML(string ticket, string strHCPResponse, string strCompanyFileName, string qbXMLCountry, int qbXMLMajorVers, int qbXMLMinorVers, AsyncCallback callback, object state)
        {
            return AsyncFactory<string>.ToBegin(sendRequestXMLAsync(ticket, strHCPResponse, strCompanyFileName, qbXMLCountry, qbXMLMajorVers, qbXMLMinorVers), callback, state);
        }

        [WebMethod]
        public override string EndsendRequestXML(IAsyncResult result)
        {
            return AsyncFactory<string>.ToEnd(result);
        }
        #endregion

        #region receiveResponseXML
        private Task<int> receiveResponseXMLAsync(string ticket, string response, string hresult, string message)
        {
            return GetQbManager().ReceiveRequestXMLAsync(ticket, response, hresult, message);
        }

        [WebMethod]
        public override IAsyncResult BeginreceiveResponseXML(string ticket, string response, string hresult, string message, AsyncCallback callback, object state)
        {
            return AsyncFactory<int>.ToBegin(receiveResponseXMLAsync(ticket, response, hresult, message), callback, state);
        }

        [WebMethod]
        public override int EndreceiveResponseXML(IAsyncResult result)
        {
            return AsyncFactory<int>.ToEnd(result);
        }
        #endregion

        #region getLastError
        private Task<string> getLastErrorAsync(string ticket)
        {
            return GetQbManager().GetLastErrorAsync(ticket);
        }

        [WebMethod]
        public override IAsyncResult BegingetLastError(string ticket, AsyncCallback callback, object state)
        {
            return AsyncFactory<string>.ToBegin(getLastErrorAsync(ticket), callback, state);
        }

        [WebMethod]
        public override string EndgetLastError(IAsyncResult result)
        {
            return AsyncFactory<string>.ToEnd(result);
        }
        #endregion

        #region closeConnection
        private Task<string> closeConnectionAsync(string ticket)
        {
            return GetQbManager().CloseConnectionAsync(ticket);
        }

        [WebMethod]
        public override IAsyncResult BegincloseConnection(string ticket, AsyncCallback callback, object state)
        {
            return AsyncFactory<string>.ToBegin(closeConnectionAsync(ticket), callback, state);
        }

        [WebMethod]
        public override string EndcloseConnection(IAsyncResult result)
        {
            return AsyncFactory<string>.ToEnd(result);
        }
        #endregion

        private QbManager GetQbManager()
        {
            if (QbManager != null)
            {
                return QbManager(this);
            }

            throw new Exception("QbManager has not been specified.");
        }
    }
}


