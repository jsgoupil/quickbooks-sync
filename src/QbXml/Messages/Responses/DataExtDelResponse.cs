using QbSync.QbXml.Objects;
using System.Xml;

namespace QbSync.QbXml.Messages.Responses
{
    public class DataExtDelResponse : QbXmlResponseWithErrorRecovery<DataExtDel>
    {
        public DataExtDelResponse()
            : base("DataExtDelRs")
        {
        }

        protected override void ProcessResponse(XmlNode responseNode, QbXmlMsgResponse<DataExtDel> qbXmlResponse)
        {
            base.ProcessResponse(responseNode, qbXmlResponse);

            var dataExtRet = responseNode.SelectSingleNode("//DataExtDelRet");
            if (dataExtRet != null)
            {
                qbXmlResponse.Object = WalkType(typeof(DataExtDel), dataExtRet) as DataExtDel;
            }
        }
    }
}