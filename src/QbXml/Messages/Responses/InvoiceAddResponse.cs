using QbSync.QbXml.Objects;
using System.Xml;

namespace QbSync.QbXml.Messages.Responses
{
    public class InvoiceAddResponse : QbXmlResponseWithErrorRecovery<Invoice>
    {
        public InvoiceAddResponse()
            : base("InvoiceAddRs")
        {
        }

        protected override void ProcessResponse(XmlNode responseNode, QbXmlMsgResponse<Invoice> qbXmlResponse)
        {
            base.ProcessResponse(responseNode, qbXmlResponse);

            var invoice = responseNode.SelectSingleNode("//InvoiceRet");
            if (invoice != null)
            {
                qbXmlResponse.Object = WalkType(typeof(Invoice), invoice) as Invoice;
            }
        }
    }
}