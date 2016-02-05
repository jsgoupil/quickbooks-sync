using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Serialization;

namespace QbSync.WebConnector.Synchronous
{
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.0.30319.18020")]
    [System.Web.Services.WebServiceAttribute(Namespace = "http://developer.intuit.com/")]
    [System.Web.Services.WebServiceBindingAttribute(Name = "TroubleshootWebServiceFSSoap", Namespace = "http://developer.intuit.com/")]
    public abstract partial class QbWebConnectorSvc : System.Web.Services.WebService
    {
        /// <remarks/>
        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://developer.intuit.com/serverVersion", RequestNamespace = "http://developer.intuit.com/", ResponseNamespace = "http://developer.intuit.com/", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public abstract string serverVersion();

        /// <remarks/>
        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://developer.intuit.com/clientVersion", RequestNamespace = "http://developer.intuit.com/", ResponseNamespace = "http://developer.intuit.com/", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public abstract string clientVersion(string strVersion);

        /// <remarks/>
        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://developer.intuit.com/authenticate", RequestNamespace = "http://developer.intuit.com/", ResponseNamespace = "http://developer.intuit.com/", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public abstract string[] authenticate(string strUserName, string strPassword);

        /// <remarks/>
        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://developer.intuit.com/connectionError", RequestNamespace = "http://developer.intuit.com/", ResponseNamespace = "http://developer.intuit.com/", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public abstract string connectionError(string ticket, string hresult, string message);

        /// <remarks/>
        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://developer.intuit.com/sendRequestXML", RequestNamespace = "http://developer.intuit.com/", ResponseNamespace = "http://developer.intuit.com/", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public abstract string sendRequestXML(string ticket, string strHCPResponse, string strCompanyFileName, string qbXMLCountry, int qbXMLMajorVers, int qbXMLMinorVers);

        /// <remarks/>
        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://developer.intuit.com/receiveResponseXML", RequestNamespace = "http://developer.intuit.com/", ResponseNamespace = "http://developer.intuit.com/", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public abstract int receiveResponseXML(string ticket, string response, string hresult, string message);

        /// <remarks/>
        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://developer.intuit.com/getLastError", RequestNamespace = "http://developer.intuit.com/", ResponseNamespace = "http://developer.intuit.com/", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public abstract string getLastError(string ticket);

        /// <remarks/>
        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://developer.intuit.com/closeConnection", RequestNamespace = "http://developer.intuit.com/", ResponseNamespace = "http://developer.intuit.com/", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public abstract string closeConnection(string ticket);
    }
}