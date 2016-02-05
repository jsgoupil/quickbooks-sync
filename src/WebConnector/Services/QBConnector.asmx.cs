using System;
using System.Web.Services;

namespace QbSync.WebConnector.Synchronous
{
    [WebService(Namespace = "http://developer.intuit.com/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class QBConnector : QBWebConnectorSvc
    {
        public static Func<QBConnector, QBManager> QBManager
        {
            get;
            set;
        }

        public QBConnector()
        {
        }

        [WebMethod]
        public override string[] authenticate(string strUserName, string strPassword)
        {
            return GetQBManager().Authenticate(strUserName, strPassword);
        }

        [WebMethod]
        public override string serverVersion()
        {
            return GetQBManager().ServerVersion();
        }

        [WebMethod]
        public override string clientVersion(string strVersion)
        {
            return GetQBManager().ClientVersion(strVersion);
        }

        [WebMethod]
        public override string connectionError(string ticket, string hresult, string message)
        {
            return GetQBManager().ConnectionError(ticket, hresult, message);
        }

        [WebMethod]
        public override string sendRequestXML(string ticket, string strHCPResponse, string strCompanyFileName, string qbXMLCountry, int qbXMLMajorVers, int qbXMLMinorVers)
        {
            return GetQBManager().SendRequestXML(ticket, strHCPResponse, strCompanyFileName, qbXMLCountry, qbXMLMajorVers, qbXMLMinorVers);
        }

        [WebMethod]
        public override int receiveResponseXML(string ticket, string response, string hresult, string message)
        {
            return GetQBManager().ReceiveRequestXML(ticket, response, hresult, message);
        }

        [WebMethod]
        public override string getLastError(string ticket)
        {
            return GetQBManager().GetLastError(ticket);
        }

        [WebMethod]
        public override string closeConnection(string ticket)
        {
            return GetQBManager().CloseConnection(ticket);
        }

        private QBManager GetQBManager()
        {
            if (QBManager != null)
            {
                return QBManager(this);
            }

            throw new Exception("QBManager has not been specified.");
        }
    }
}


