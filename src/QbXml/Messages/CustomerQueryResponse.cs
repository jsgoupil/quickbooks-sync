using QBSync.QbXml.Struct;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using QBSync.QuickbooksDesktopSync.Extensions;
using QBSync.QbXml.Type;
using QBSync.QbXml.Filters;
using QBSync.QbXml.Objects;
using System.Reflection;

namespace QBSync.QbXml.Messages
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