using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace QbSync.XsdGenerator
{
    class TypeEnhancer
    {
        private CodeNamespace codeNamespace;
        private XmlSchemas xsds;

        public TypeEnhancer(CodeNamespace codeNamespace, XmlSchemas xsds)
        {
            this.codeNamespace = codeNamespace;
            this.xsds = xsds;
        }

        public void Enhance()
        {
            var codeTypes = codeNamespace.Types.OfType<CodeTypeDeclaration>();
            var qbXmlMsgsRq = codeTypes.First(m => m.Name == "QBXMLMsgsRq");
            var qbXmlMsgsRqItems = qbXmlMsgsRq.Members.OfType<CodeMemberProperty>().First(m => m.Name == "Items");
            var requestList = qbXmlMsgsRqItems.CustomAttributes
                .OfType<CodeAttributeDeclaration>()
                .Select(m => ((CodeTypeOfExpression)m.Arguments[1].Value).Type.BaseType)
                .OfType<string>()
                .ToArray();
            var qbXmlMsgsRs = codeTypes.First(m => m.Name == "QBXMLMsgsRs");
            var qbXmlMsgsRsItems = qbXmlMsgsRs.Members.OfType<CodeMemberProperty>().First(m => m.Name == "Items");
            var responseList = qbXmlMsgsRsItems.CustomAttributes
                .OfType<CodeAttributeDeclaration>()
                .Select(m => ((CodeTypeOfExpression)m.Arguments[1].Value).Type.BaseType)
                .OfType<string>()
                .ToArray();
            foreach (CodeTypeDeclaration codeType in codeTypes)
            {
                var memberEnhancer = new MemberEnhancer(codeType, codeNamespace, xsds);
                memberEnhancer.Enhance();

                if (requestList.Contains(codeType.Name))
                {
                    codeType.BaseTypes.Add("QbRequest");
                }
                else if (responseList.Contains(codeType.Name))
                {
                    codeType.BaseTypes.Add("QbResponse");
                }
            }
        }
    }
}
