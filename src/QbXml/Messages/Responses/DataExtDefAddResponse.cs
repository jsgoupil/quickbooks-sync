using QbSync.QbXml.Objects;
using System.Linq;
using System.Xml;

namespace QbSync.QbXml.Messages.Responses
{
    public class DataExtDefAddResponse : QbXmlResponseWithErrorRecovery<DataExtDef>
    {
        public DataExtDefAddResponse()
            : base("DataExtDefAddRs")
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