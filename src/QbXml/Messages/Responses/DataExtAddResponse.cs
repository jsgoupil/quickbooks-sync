using QbSync.QbXml.Objects;
using System.Linq;
using System.Xml;

namespace QbSync.QbXml.Messages.Responses
{
    public class DataExtAddResponse : QbXmlResponseWithErrorRecovery<DataExtRet>
    {
        public DataExtAddResponse()
            : base("DataExtAddRs")
        {
        }

        protected override void ProcessResponse(XmlNode responseNode, QbXmlMsgResponse<DataExtRet> qbXmlResponse)
        {
            base.ProcessResponse(responseNode, qbXmlResponse);

            var dataExtRet = responseNode.SelectSingleNode("//DataExtRet");
            if (dataExtRet != null)
            {
                qbXmlResponse.Object = WalkType(typeof(DataExtRet), dataExtRet) as DataExtRet;
            }
        }
    }
}