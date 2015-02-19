using QbSync.QbXml.Objects;
using System.Linq;
using System.Xml;

namespace QbSync.QbXml.Messages.Responses
{
    public class InvoiceQueryResponse : QbXmlResponse<Invoice[]>
    {
        public InvoiceQueryResponse()
            : base("InvoiceQueryRs")
        {
        }

        protected override void ProcessResponse(XmlNode responseNode, QbXmlMsgResponse<Invoice[]> qbXmlResponse)
        {
            base.ProcessResponse(responseNode, qbXmlResponse);

            var customerRetList = responseNode.SelectNodes("//InvoiceRet");
            qbXmlResponse.Object = WalkTypes(typeof(Invoice), customerRetList).OfType<Invoice>().ToArray();
        }
    }
}