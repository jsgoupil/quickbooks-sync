using QbSync.QbXml.Extensions;
using QbSync.QbXml.Objects;
using QbSync.QbXml.Struct;
using QbSync.QbXml.Type;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml;

namespace QbSync.QbXml
{
    public class QbXmlResponseWithErrorRecovery<T> : QbXmlResponse<T>
    {
        public QbXmlResponseWithErrorRecovery(string rootElementName)
            : base(rootElementName)
        {
        }

        protected override void ProcessResponse(XmlNode responseNode, QbXmlMsgResponse<T> qbXmlResponse)
        {
            var qbXmlMsgResponseWithErrorRecovery = qbXmlResponse as QbXmlMsgResponseWithErrorRecovery<T>;

            var errorRecovery = responseNode.SelectSingleNode("//ErrorRecovery");
            if (errorRecovery != null)
            {
                qbXmlMsgResponseWithErrorRecovery.ErrorRecovery = WalkType(typeof(ErrorRecovery), errorRecovery) as ErrorRecovery;
            }

            base.ProcessResponse(responseNode, qbXmlResponse);
        }

        protected override QbXmlMsgResponse<T> CreateQbXmlMsgResponse()
        {
            return new QbXmlMsgResponseWithErrorRecovery<T>();
        }
    }
}
