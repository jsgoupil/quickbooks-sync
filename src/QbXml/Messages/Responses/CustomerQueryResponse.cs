using QbSync.QbXml.Objects;
using System.Linq;
using System.Xml;

namespace QbSync.QbXml.Messages.Responses
{
    public class CustomerQueryResponse : QbXmlResponse<Customer[]>
    {
        public CustomerQueryResponse()
            : base("CustomerQueryRs")
        {
        }

        protected override void ProcessResponse(XmlNode responseNode, QbXmlMsgResponse<Customer[]> qbXmlResponse)
        {
            base.ProcessResponse(responseNode, qbXmlResponse);

            var customerRetList = responseNode.SelectNodes("//CustomerRet");
            qbXmlResponse.Object = WalkTypes(typeof(Customer), customerRetList).OfType<Customer>().ToArray();
        }
    }
}