using QbSync.QbXml.Objects;
using System.Linq;
using System.Xml;

namespace QbSync.QbXml.Messages
{
    public class CustomerQueryResponse : IteratorResponse<Customer[]>
    {
        public CustomerQueryResponse()
            : base("CustomerQueryRs")
        {
        }

        protected override void ProcessResponse(XmlNode responseNode, QbXmlMsgResponse<Customer[]> qbXmlResponse)
        {
            base.ProcessResponse(responseNode, qbXmlResponse);

            var customerRetList = responseNode.SelectNodes("//CustomerRet"); // XPath Query
            qbXmlResponse.Object = WalkTypes(typeof(Customer), customerRetList).OfType<Customer>().ToArray();
        }
    }
}