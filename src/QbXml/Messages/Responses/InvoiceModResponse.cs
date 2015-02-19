using QbSync.QbXml.Objects;
using System.Xml;

namespace QbSync.QbXml.Messages.Responses
{
    public class InvoiceModResponse : QbXmlResponseWithErrorRecovery<Invoice>
    {
        public InvoiceModResponse()
            : base("InvoiceModRs")
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