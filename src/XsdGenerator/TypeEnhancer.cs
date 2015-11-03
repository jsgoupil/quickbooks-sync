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
            AddQbNormalizer();

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

        private void AddQbNormalizer()
        {
            /*
             *     internal static class QbNormalizer
             *     {
             *         internal static System.Text.RegularExpressions.Regex newline_pattern = new System.Text.RegularExpressions.Regex(@"\r\n|\r|\n", System.Text.RegularExpressions.RegexOptions.Compiled);
             *
             *         internal static string NormalizeString(string input, int length)
             *         {
             *             if (input != null)
             *             {
             *                 input = newline_pattern.Replace(input, "\r\n");
             *                 return input.Substring(0, Math.Min(length, input.Length));
             *             }
             *
             *             return null;
             *         }
             *     }
             */

            var qbNormalizer = new CodeTypeDeclaration("QbNormalizer");
            qbNormalizer.Attributes = MemberAttributes.Static | MemberAttributes.Family | MemberAttributes.Private;

            var newlinePattern = new CodeMemberField(typeof(System.Text.RegularExpressions.Regex), "newlinePattern");
            newlinePattern.Attributes = MemberAttributes.Static | MemberAttributes.FamilyAndAssembly;
            var newRegexExpression = new CodeObjectCreateExpression(
                        new CodeTypeReference(typeof(System.Text.RegularExpressions.Regex)),
                        new CodeExpression[] {
                            new CodePrimitiveExpression("\\r\\n|\\r|\\n"),
                            new CodeFieldReferenceExpression(
                                new CodeTypeReferenceExpression("System.Text.RegularExpressions.RegexOptions"),
                                "Compiled"
                            )
                        }
                    );
            newlinePattern.InitExpression = newRegexExpression;

            var inputParameter = new CodeParameterDeclarationExpression(typeof(string), "input");
            var lengthParameter = new CodeParameterDeclarationExpression(typeof(int), "length");
            var normalizeString = new CodeMemberMethod();
            normalizeString.Name = "NormalizeString";
            normalizeString.Parameters.Add(inputParameter);
            normalizeString.Parameters.Add(lengthParameter);
            normalizeString.Attributes = MemberAttributes.Static | MemberAttributes.FamilyAndAssembly;
            normalizeString.ReturnType = new CodeTypeReference(typeof(string));

            var inputReference = new CodeVariableReferenceExpression("input");
            var lengthReference = new CodeVariableReferenceExpression("length");
            var newlinePatternReference = new CodeVariableReferenceExpression("newlinePattern");
            var ifStatement =
                new CodeConditionStatement(
                    new CodeBinaryOperatorExpression(
                        inputReference,
                        CodeBinaryOperatorType.IdentityInequality,
                        new CodePrimitiveExpression()
                    )
                );

            var mathMin = new CodeMethodInvokeExpression(
                new CodeMethodReferenceExpression(new CodeVariableReferenceExpression("Math"), "Min"),
                lengthReference,
                new CodePropertyReferenceExpression(inputReference, "Length")
                );

            var tempSubstring = new CodeMethodInvokeExpression(
                    new CodeMethodReferenceExpression(inputReference, "Substring"),
                    new CodePrimitiveExpression(0),
                    mathMin
                );

            var tempReplace = new CodeMethodInvokeExpression(
                    new CodeMethodReferenceExpression(newlinePatternReference, "Replace"),
                    inputReference,
                    new CodePrimitiveExpression("\r\n")
                );

            var trueStatement1 = new CodeAssignStatement(
                inputReference,
                tempReplace
            );

            var trueStatement2 = new CodeAssignStatement(
                inputReference,
                tempSubstring
            );

            var returnStatement1 = new CodeMethodReturnStatement(inputReference);

            ifStatement.TrueStatements.Add(trueStatement1);
            ifStatement.TrueStatements.Add(trueStatement2);
            ifStatement.TrueStatements.Add(returnStatement1);

            normalizeString.Statements.Add(ifStatement);
            normalizeString.Statements.Add(new CodeMethodReturnStatement(new CodePrimitiveExpression()));

            qbNormalizer.Members.Add(newlinePattern);
            qbNormalizer.Members.Add(normalizeString);

            codeNamespace.Types.Add(qbNormalizer);
        }
    }
}
