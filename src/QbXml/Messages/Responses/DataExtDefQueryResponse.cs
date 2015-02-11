using QbSync.QbXml.Objects;
using System.Linq;
using System.Xml;

namespace QbSync.QbXml.Messages.Responses
{
    public class DataExtDefQueryResponse : QbXmlResponse<DataExtDefRet[]>
    {
        public DataExtDefQueryResponse()
            : base("DataExtDefQueryRs")
        {
        }

        protected override void ProcessResponse(XmlNode responseNode, QbXmlMsgResponse<DataExtDefRet[]> qbXmlResponse)
        {
            base.ProcessResponse(responseNode, qbXmlResponse);

            var dataExtDefRetList = responseNode.SelectNodes("//DataExtDefRet");
            qbXmlResponse.Object = WalkTypes(typeof(DataExtDefRet), dataExtDefRetList).OfType<DataExtDefRet>().ToArray();
        }
    }
}