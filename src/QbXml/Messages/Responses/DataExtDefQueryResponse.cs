using QbSync.QbXml.Objects;
using System.Linq;
using System.Xml;

namespace QbSync.QbXml.Messages.Responses
{
    public class DataExtDefQueryResponse : QbXmlResponse<DataExtDef[]>
    {
        public DataExtDefQueryResponse()
            : base("DataExtDefQueryRs")
        {
        }

        protected override void ProcessResponse(XmlNode responseNode, QbXmlMsgResponse<DataExtDef[]> qbXmlResponse)
        {
            base.ProcessResponse(responseNode, qbXmlResponse);

            var dataExtDefRetList = responseNode.SelectNodes("//DataExtDefRet");
            qbXmlResponse.Object = WalkTypes(typeof(DataExtDef), dataExtDefRetList).OfType<DataExtDef>().ToArray();
        }
    }
}