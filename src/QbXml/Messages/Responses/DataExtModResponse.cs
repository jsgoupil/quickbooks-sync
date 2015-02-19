using QbSync.QbXml.Objects;
using System.Linq;
using System.Xml;

namespace QbSync.QbXml.Messages.Responses
{
    public class DataExtModResponse : QbXmlResponseWithErrorRecovery<DataExt>
    {
        public DataExtModResponse()
            : base("DataExtModRs")
        {
        }

        protected override void ProcessResponse(XmlNode responseNode, QbXmlMsgResponse<DataExt> qbXmlResponse)
        {
            base.ProcessResponse(responseNode, qbXmlResponse);

            var dataExtRet = responseNode.SelectSingleNode("//DataExtRet");
            if (dataExtRet != null)
            {
                qbXmlResponse.Object = WalkType(typeof(DataExt), dataExtRet) as DataExt;
            }
        }
    }
}