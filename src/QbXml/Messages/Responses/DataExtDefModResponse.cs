using QbSync.QbXml.Objects;
using System.Xml;

namespace QbSync.QbXml.Messages.Responses
{
    public class DataExtDefModResponse : QbXmlResponseWithErrorRecovery<DataExtDef>
    {
        public DataExtDefModResponse()
            : base("DataExtDefModRs")
        {
        }

        protected override void ProcessResponse(XmlNode responseNode, QbXmlMsgResponse<DataExtDef> qbXmlResponse)
        {
            base.ProcessResponse(responseNode, qbXmlResponse);

            var dataExtDefRet = responseNode.SelectSingleNode("//DataExtDefRet");
            if (dataExtDefRet != null)
            {
                qbXmlResponse.Object = WalkType(typeof(DataExtDef), dataExtDefRet) as DataExtDef;
            }
        }
    }
}