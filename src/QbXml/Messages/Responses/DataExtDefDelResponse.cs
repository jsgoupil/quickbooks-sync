using QbSync.QbXml.Objects;
using System.Linq;
using System.Xml;

namespace QbSync.QbXml.Messages.Responses
{
    public class DataExtDefDelResponse : QbXmlResponseWithErrorRecovery<DataExtDefDel>
    {
        public DataExtDefDelResponse()
            : base("DataExtDefDelRs")
        {
        }

        protected override void ProcessResponse(XmlNode responseNode, QbXmlMsgResponse<DataExtDefDel> qbXmlResponse)
        {
            base.ProcessResponse(responseNode, qbXmlResponse);

            var dataExtDefRet = responseNode.SelectSingleNode("//DataExtDefDelRet");
            if (dataExtDefRet != null)
            {
                qbXmlResponse.Object = WalkType(typeof(DataExtDefDel), dataExtDefRet) as DataExtDefDel;
            }
        }
    }
}