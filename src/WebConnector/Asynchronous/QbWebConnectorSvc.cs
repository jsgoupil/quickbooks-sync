using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Serialization;

namespace QbSync.WebConnector.Asynchronous
{
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.0.30319.18020")]
    [System.Web.Services.WebServiceAttribute(Namespace = "http://developer.intuit.com/")]
    [System.Web.Services.WebServiceBindingAttribute(Name = "TroubleshootWebServiceFSSoap", Namespace = "http://developer.intuit.com/")]
    public abstract partial class QbWebConnectorSvc : System.Web.Services.WebService
    {
        #region serverVersion
        /// <remarks/>
        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://developer.intuit.com/serverVersion", RequestNamespace = "http://developer.intuit.com/", ResponseNamespace = "http://developer.intuit.com/", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public abstract string serverVersion();
        #endregion

        #region clientVersion
        /// <remarks/>
        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://developer.intuit.com/clientVersion", RequestNamespace = "http://developer.intuit.com/", ResponseNamespace = "http://developer.intuit.com/", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public abstract string clientVersion(string strVersion);
        #endregion

        #region authenticate
        /// <remarks/>
        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://developer.intuit.com/authenticate", RequestNamespace = "http://developer.intuit.com/", ResponseNamespace = "http://developer.intuit.com/", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public abstract IAsyncResult Beginauthenticate(string strUserName, string strPassword, AsyncCallback callback, object state);

        /// <remarks/>
        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://developer.intuit.com/authenticate", RequestNamespace = "http://developer.intuit.com/", ResponseNamespace = "http://developer.intuit.com/", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public abstract string[] Endauthenticate(IAsyncResult result);
        #endregion

        #region connectionError
        /// <remarks/>
        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://developer.intuit.com/connectionError", RequestNamespace = "http://developer.intuit.com/", ResponseNamespace = "http://developer.intuit.com/", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public abstract IAsyncResult BeginconnectionError(string ticket, string hresult, string message, AsyncCallback callback, object state);

        /// <remarks/>
        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://developer.intuit.com/connectionError", RequestNamespace = "http://developer.intuit.com/", ResponseNamespace = "http://developer.intuit.com/", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public abstract string EndconnectionError(IAsyncResult result);
        #endregion

        #region sendRequestXML
        /// <remarks/>
        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://developer.intuit.com/sendRequestXML", RequestNamespace = "http://developer.intuit.com/", ResponseNamespace = "http://developer.intuit.com/", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public abstract IAsyncResult BeginsendRequestXML(string ticket, string strHCPResponse, string strCompanyFileName, string qbXMLCountry, int qbXMLMajorVers, int qbXMLMinorVers, AsyncCallback callback, object state);

        /// <remarks/>
        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://developer.intuit.com/sendRequestXML", RequestNamespace = "http://developer.intuit.com/", ResponseNamespace = "http://developer.intuit.com/", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public abstract string EndsendRequestXML(IAsyncResult result);
        #endregion

        #region receiveResponseXML
        /// <remarks/>
        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://developer.intuit.com/receiveResponseXML", RequestNamespace = "http://developer.intuit.com/", ResponseNamespace = "http://developer.intuit.com/", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public abstract IAsyncResult BeginreceiveResponseXML(string ticket, string response, string hresult, string message, AsyncCallback callback, object state);

        /// <remarks/>
        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://developer.intuit.com/receiveResponseXML", RequestNamespace = "http://developer.intuit.com/", ResponseNamespace = "http://developer.intuit.com/", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public abstract int EndreceiveResponseXML(IAsyncResult result);
        #endregion

        #region getLastError
        /// <remarks/>
        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://developer.intuit.com/getLastError", RequestNamespace = "http://developer.intuit.com/", ResponseNamespace = "http://developer.intuit.com/", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public abstract IAsyncResult BegingetLastError(string ticket, AsyncCallback callback, object state);

        /// <remarks/>
        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://developer.intuit.com/getLastError", RequestNamespace = "http://developer.intuit.com/", ResponseNamespace = "http://developer.intuit.com/", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public abstract string EndgetLastError(IAsyncResult result);
        #endregion

        #region closeConnection
        /// <remarks/>
        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://developer.intuit.com/closeConnection", RequestNamespace = "http://developer.intuit.com/", ResponseNamespace = "http://developer.intuit.com/", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public abstract IAsyncResult BegincloseConnection(string ticket, AsyncCallback callback, object state);

        /// <remarks/>
        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://developer.intuit.com/closeConnection", RequestNamespace = "http://developer.intuit.com/", ResponseNamespace = "http://developer.intuit.com/", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public abstract string EndcloseConnection(IAsyncResult result);
        #endregion
    }
}