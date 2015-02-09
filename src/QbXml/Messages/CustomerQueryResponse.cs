using QbSync.QbXml.Struct;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using QbSync.QuickbooksDesktopSync.Extensions;
using QbSync.QbXml.Type;
using QbSync.QbXml.Filters;
using QbSync.QbXml.Objects;
using System.Reflection;

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