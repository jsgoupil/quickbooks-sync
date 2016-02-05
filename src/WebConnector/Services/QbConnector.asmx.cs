using System;
using System.Web.Services;

namespace QbSync.WebConnector.Synchronous
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

        [WebMethod]
        public override string[] authenticate(string strUserName, string strPassword)
        {
            return GetQbManager().Authenticate(strUserName, strPassword);
        }

        [WebMethod]
        public override string serverVersion()
        {
            return GetQbManager().ServerVersion();
        }

        [WebMethod]
        public override string clientVersion(string strVersion)
        {
            return GetQbManager().ClientVersion(strVersion);
        }

        [WebMethod]
        public override string connectionError(string ticket, string hresult, string message)
        {
            return GetQbManager().ConnectionError(ticket, hresult, message);
        }

        [WebMethod]
        public override string sendRequestXML(string ticket, string strHCPResponse, string strCompanyFileName, string qbXMLCountry, int qbXMLMajorVers, int qbXMLMinorVers)
        {
            return GetQbManager().SendRequestXML(ticket, strHCPResponse, strCompanyFileName, qbXMLCountry, qbXMLMajorVers, qbXMLMinorVers);
        }

        [WebMethod]
        public override int receiveResponseXML(string ticket, string response, string hresult, string message)
        {
            return GetQbManager().ReceiveRequestXML(ticket, response, hresult, message);
        }

        [WebMethod]
        public override string getLastError(string ticket)
        {
            return GetQbManager().GetLastError(ticket);
        }

        [WebMethod]
        public override string closeConnection(string ticket)
        {
            return GetQbManager().CloseConnection(ticket);
        }

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


