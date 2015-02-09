using System;
using System.Web.Services;

namespace QbSync.WebConnector
{
    [WebService(Namespace = "http://developer.intuit.com/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class QBConnectorSync : QBWebConnectorSvc
    {
        public static Func<QBConnectorSync, SyncManager> SyncManager
        {
            get;
            set;
        }

        public QBConnectorSync()
        {
        }

        [WebMethod]
        public override string[] authenticate(string strUserName, string strPassword)
        {
            return GetSyncManager().Authenticate(strUserName, strPassword);
        }

        [WebMethod]
        public override string serverVersion()
        {
            return GetSyncManager().ServerVersion();
        }

        [WebMethod]
        public override string clientVersion(string strVersion)
        {
            return GetSyncManager().ClientVersion(strVersion);
        }

        [WebMethod]
        public override string connectionError(string ticket, string hresult, string message)
        {
            return GetSyncManager().ConnectionError(ticket, hresult, message);
        }

        [WebMethod]
        public override string sendRequestXML(string ticket, string strHCPResponse, string strCompanyFileName, string qbXMLCountry, int qbXMLMajorVers, int qbXMLMinorVers)
        {
            return GetSyncManager().SendRequestXML(ticket, strHCPResponse, strCompanyFileName, qbXMLCountry, qbXMLMajorVers, qbXMLMinorVers);
        }

        [WebMethod]
        public override int receiveResponseXML(string ticket, string response, string hresult, string message)
        {
            return GetSyncManager().ReceiveRequestXML(ticket, response, hresult, message);
        }

        [WebMethod]
        public override string getLastError(string ticket)
        {
            return GetSyncManager().GetLastError(ticket);
        }

        [WebMethod]
        public override string closeConnection(string ticket)
        {
            return GetSyncManager().CloseConnection(ticket);
        }

        private SyncManager GetSyncManager()
        {
            if (SyncManager != null)
            {
                return SyncManager(this);
            }

            throw new Exception("SyncManager has not been specified.");
        }
    }
}


