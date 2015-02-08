using QBSync.QbXml.Type;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace QBSync.QbXml
{
    public class IteratorResponse<T> : QbXmlResponse<T>
    {
        public IteratorResponse(string rootElementName)
            : base(rootElementName)
        {
        }

        protected override void ProcessResponse(XmlNode responseNode, QbXmlMsgResponse<T> qbXmlResponse)
        {
            base.ProcessResponse(responseNode, qbXmlResponse);

            var rsAttributes = responseNode.Attributes;
            var iteratorId = rsAttributes.GetNamedItem("iteratorID");
            var iteratorRemainingCount = rsAttributes.GetNamedItem("iteratorRemainingCount");

            qbXmlResponse.IteratorID = iteratorId == null ? null : iteratorId.Value;
            qbXmlResponse.IteratorRemainingCount = iteratorRemainingCount == null ? (int?)null : Convert.ToInt32(iteratorRemainingCount.Value);
        }
    }
}
