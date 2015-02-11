using QbSync.QbXml.Objects;
using System.Linq;
using System.Xml;

namespace QbSync.QbXml.Messages.Responses
{
    public class DataExtDefModResponse : QbXmlResponseWithErrorRecovery<DataExtDefRet>
    {
        public DataExtDefModResponse()
            : base("DataExtDefModRs")
        {
        }

        protected override void ProcessResponse(XmlNode responseNode, QbXmlMsgResponse<DataExtDefRet> qbXmlResponse)
        {
            base.ProcessResponse(responseNode, qbXmlResponse);

            var dataExtDefRet = responseNode.SelectSingleNode("//DataExtDefRet");
            if (dataExtDefRet != null)
            {
                qbXmlResponse.Object = WalkType(typeof(DataExtDefRet), dataExtDefRet) as DataExtDefRet;
            }
        }
    }
}