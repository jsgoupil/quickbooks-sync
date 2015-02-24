using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace QbSync.XsdGenerator
{
    class MemberEnhancer
    {
        private CodeTypeDeclaration codeType;
        private CodeNamespace codeNamespace;
        private XmlSchemas xsds;
        private IEnumerable<CodeTypeDeclaration> codeNamespaceTypes;
        private IEnumerable<CodeMemberField> codeMemberFields;
        private IEnumerable<CodeMemberProperty> codeMemberProperties;
        private CodeAttributeDeclaration ignoreAttribute = new CodeAttributeDeclaration("System.Xml.Serialization.XmlIgnoreAttribute");

        public MemberEnhancer(CodeTypeDeclaration codeType, CodeNamespace codeNamespace, XmlSchemas xsds)
        {
            this.codeType = codeType;
            this.codeNamespace = codeNamespace;
            this.xsds = xsds;
            this.codeNamespaceTypes = codeNamespace.Types.Cast<CodeTypeDeclaration>();

            this.codeMemberFields = codeType.Members.OfType<CodeMemberField>();
            this.codeMemberProperties = codeType.Members.OfType<CodeMemberProperty>();
        }

        public void Enhance()
        {
            var removeProperties = new string[] { "iteratorSpecified" };
            var removeFields = new string[] { "iteratorFieldSpecified" };
            var stringToIntProperties = new string[] { "iteratorRemainingCount" };
            var stringToIntFields = new string[] { "iteratorRemainingCountField" };
            AddItemsProperties();
            RemoveProperties(removeProperties);
            RemoveFields(removeFields);

            // We can't change this type if we serialize, but we can do it if we deserialize.
            ChangeType("System.String", "System.Int32", true, stringToIntProperties, stringToIntFields);
            ChangeType(null, "IteratorType", true, new string[] { "iterator" }, new string[] { "iteratorField" });
            InsertInterface();
        }

        private void InsertInterface()
        {
            var iteratorID = codeMemberProperties.FirstOrDefault(m => m.Name == "iteratorID");
            var maxReturned = codeMemberProperties.FirstOrDefault(m => m.Name == "MaxReturned");
            if (iteratorID != null && maxReturned != null)
            {
                codeType.BaseTypes.Add("QbIteratorRequest");
            }

            var iteratorRemainingCount = codeMemberProperties.FirstOrDefault(m => m.Name == "iteratorRemainingCount");
            if (iteratorID != null && iteratorRemainingCount != null)
            {
                codeType.BaseTypes.Add("QbIteratorResponse");
            }
        }

        private void RemoveFields(string[] fields)
        {
            foreach (var field in fields)
            {
                var iteratorFieldSpecified = codeMemberFields.FirstOrDefault(m => m.Name == field);
                if (iteratorFieldSpecified != null)
                {
                    codeType.Members.Remove(iteratorFieldSpecified);
                }
            }
        }

        private void RemoveProperties(string[] properties)
        {
            foreach (var property in properties)
            {
                var iteratorSpecified = codeMemberProperties.FirstOrDefault(m => m.Name == property);
                if (iteratorSpecified != null)
                {
                    codeType.Members.Remove(iteratorSpecified);
                }
            }
        }

        private void ChangeType(string fromType, string toType, bool makeNullable, string[] changeProperties, string[] changeFields)
        {
            foreach (var attribute in changeProperties)
            {
                var codeMemberProperty = codeMemberProperties.FirstOrDefault(m => m.Name == attribute);
                if (codeMemberProperty != null)
                {
                    // We already worked on this one.
                    if (codeMemberProperty.Type.BaseType.StartsWith("Nullable<"))
                    {
                        continue;
                    }

                    var newType = string.Empty;
                    if (fromType == null || codeMemberProperty.Type.BaseType == fromType)
                    {
                        newType = toType;
                    }
                    else
                    {
                        newType = codeMemberProperty.Type.BaseType;
                    }

                    var baseType = newType;
                    if (makeNullable)
                    {
                        baseType = "Nullable<" + newType + ">";
                    }

                    // Removing the DataType="integer", and add the XmlIgnore
                    var initialCodeAttribute = codeMemberProperty.CustomAttributes[0];
                    codeMemberProperty.CustomAttributes.Clear();
                    codeMemberProperty.CustomAttributes.Add(ignoreAttribute);
                    codeMemberProperty.Type.BaseType = baseType;
                    codeType.Members.AddRange(SpecifiedProperty(codeMemberProperty.Name, newType, initialCodeAttribute.Name));
                }
            }

            foreach (var attribute in changeFields)
            {
                var codeMemberField = codeMemberFields.FirstOrDefault(m => m.Name == attribute);
                if (codeMemberField != null)
                {
                    var newType = string.Empty;
                    if (fromType == null || codeMemberField.Type.BaseType == fromType)
                    {
                        newType = toType;
                    }
                    else
                    {
                        newType = codeMemberField.Type.BaseType;
                    }

                    var baseType = newType;
                    if (makeNullable)
                    {
                        baseType = "Nullable<" + newType + ">";
                    }

                    codeMemberField.Type.BaseType = baseType;
                }
            }
        }

        private CodeTypeMember[] SpecifiedProperty(string p, string typeName, string codeAttributeDeclarationName)
        {
            var hasValueExpression =
                new CodeMethodReturnStatement(
                    new CodePropertyReferenceExpression(
                        new CodePropertyReferenceExpression(
                            new CodeThisReferenceExpression(), p
                        ),
                        "HasValue"
                    )
                );

            var getValueExpression =
                new CodeMethodReturnStatement(
                    new CodePropertyReferenceExpression(
                        new CodePropertyReferenceExpression(
                            new CodeThisReferenceExpression(), p
                        ),
                        "Value"
                    )
                );

            var setValueExpression =
                new CodeAssignStatement(
                    new CodePropertyReferenceExpression(
                        new CodeThisReferenceExpression(), p
                    ),
                    new CodeVariableReferenceExpression("value")
                );

            var neverShow = new CodeAttributeDeclaration("System.ComponentModel.EditorBrowsable", new CodeAttributeArgument(new CodeSnippetExpression("System.ComponentModel.EditorBrowsableState.Never")));

            var propertySpecified = new CodeMemberProperty();
            propertySpecified.Attributes = MemberAttributes.Public | MemberAttributes.Final;
            propertySpecified.Name = p + "ValueSpecified";
            propertySpecified.Type = new CodeTypeReference(typeof(bool));
            propertySpecified.CustomAttributes.Add(new CodeAttributeDeclaration("System.Xml.Serialization.XmlIgnore"));
            propertySpecified.CustomAttributes.Add(neverShow);
            propertySpecified.GetStatements.Add(
                hasValueExpression
            );

            var propertyName = codeAttributeDeclarationName.Contains("XmlAttributeAttribute") ? "AttributeName" : "ElementName";

            var propertyValue = new CodeMemberProperty();
            propertyValue.Attributes = MemberAttributes.Public | MemberAttributes.Final;
            propertyValue.Name = p + "Value";
            propertyValue.Type = new CodeTypeReference(typeName);
            propertyValue.CustomAttributes.Add(new CodeAttributeDeclaration(codeAttributeDeclarationName, new CodeAttributeArgument(propertyName, new CodePrimitiveExpression(p))));
            propertyValue.CustomAttributes.Add(neverShow);
            propertyValue.GetStatements.Add(getValueExpression);
            propertyValue.SetStatements.Add(setValueExpression);

            return new CodeTypeMember[] { propertySpecified, propertyValue };
        }

        private void CapitalizeProperties(string[] capitalizeAttributes)
        {
            foreach (var attribute in capitalizeAttributes)
            {
                var codeMemberProperty = codeMemberProperties.FirstOrDefault(m => m.Name == attribute);
                if (codeMemberProperty != null)
                {
                    codeMemberProperty.Name = attribute.Substring(0, 1).ToUpper() + attribute.Substring(1, attribute.Length - 1);

                    var xmlAttribute = codeMemberProperty.CustomAttributes.Cast<CodeAttributeDeclaration>().FirstOrDefault(m => m.Name == "System.Xml.Serialization.XmlAttributeAttribute");
                    if (xmlAttribute != null)
                    {
                        xmlAttribute.Arguments.Add(new CodeAttributeArgument("AttributeName", new CodePrimitiveExpression(attribute)));
                    }
                }
            }
        }

        private void AddItemsProperties()
        {
            var codeMember = codeMemberProperties.FirstOrDefault(m => m.Name == "Items" || m.Name == "Item");
            if (codeMember != null)
            {
                AddAppropriateProperties(codeMember);
            }
        }

        private void AddAppropriateProperties(CodeMemberProperty codeMemberProperty)
        {
            var newMembers = new List<EnhancedProperty>();
            string withEnumChoice = null;
            foreach (CodeAttributeDeclaration attribute in codeMemberProperty.CustomAttributes)
            {
                if (attribute.Name == "System.Xml.Serialization.XmlElementAttribute")
                {
                    var enhancedProperty = GetEnhancedPropertyFromAttributeAndUpdateAttribute(attribute);
                    newMembers.Add(enhancedProperty);
                }
                else if (attribute.Name == "System.Xml.Serialization.XmlChoiceIdentifierAttribute")
                {
                    var propertyName = ((CodePrimitiveExpression)attribute.Arguments[0].Value).Value as string;
                    withEnumChoice = GetTypeForMember(codeType, propertyName);
                }
            }

            foreach (var enhancedProperty in newMembers)
            {
                var isArray = CheckIfPropertyIsArray(codeType.Name, enhancedProperty.Name);
                CodeMemberProperty property = CreateAppropriateProperty(withEnumChoice, enhancedProperty, isArray);
                codeType.Members.Add(property);
            }

            AddObjectItems(codeType, withEnumChoice, codeMemberProperty.Name == "Item");
        }

        private CodeMemberProperty CreateAppropriateProperty(string withEnumChoice, EnhancedProperty enhancedProperty, bool isArray)
        {
            CodeMemberProperty property = null;
            if (string.IsNullOrEmpty(withEnumChoice))
            {
                if (isArray)
                {
                    property = CreateArrayPropertyWithoutChoice(enhancedProperty);
                }
                else
                {
                    property = CreateSinglePropertyWithoutChoice(enhancedProperty);
                }
            }
            else
            {
                if (isArray)
                {
                    property = CreateArrayPropertyWithChoice(enhancedProperty, withEnumChoice);
                }
                else
                {
                    property = CreateSinglePropertyWithChoice(enhancedProperty, withEnumChoice);
                }
            }

            return property;
        }

        private XmlSchemaElement GetXmlSchemaElementFromType(string elementName)
        {
            var elementValuesQuery = from xsd in xsds
                                     let values = xsd.Elements.Values.Cast<XmlSchemaElement>()
                                     from el in values
                                     where el.QualifiedName.Name == elementName
                                     select el;

            return elementValuesQuery.FirstOrDefault();
        }

        private XmlSchemaElement GetXmlSchemaElementFromPropertyType(string elementName, string propertyName)
        {
            var schemaTypeQuery = from xsd in xsds
                                  let el = xsd.SchemaTypes[new XmlQualifiedName(elementName)]
                                  where el != null
                                  select el;

            var element = GetXmlSchemaElementFromType(elementName);
            XmlSchemaComplexType schemaType = null;
            if (element != null)
            {
                schemaType = (XmlSchemaComplexType)element.SchemaType;
            }
            else
            {
                var elementInComplexTypes = schemaTypeQuery.FirstOrDefault();
                if (elementInComplexTypes != null)
                {
                    schemaType = (XmlSchemaComplexType)elementInComplexTypes;
                }
            }

            if (schemaType != null)
            {
                var linqKvpItem = from item in GetAllSchemaElements(schemaType.ContentTypeParticle)
                                  where item.RefName.Name == propertyName || item.Name == propertyName
                                  select item;
                return linqKvpItem.FirstOrDefault();
            }

            return null;
        }

        private bool CheckIfPropertyIsArray(string typeName, string propertyName)
        {
            var isArray = false;
            var xmlSchemaElement = GetXmlSchemaElementFromPropertyType(typeName, propertyName);

            if (xmlSchemaElement != null)
            {
                if (xmlSchemaElement.MaxOccurs > 1)
                {
                    isArray = true;
                }
            }

            return isArray;
        }

        private List<XmlSchemaElement> GetAllSchemaElements(XmlSchemaParticle particule)
        {
            if (particule is XmlSchemaSequence)
            {
                return GetAllSchemaElements((particule as XmlSchemaSequence).Items);
            }
            else if (particule is XmlSchemaChoice)
            {
                return GetAllSchemaElements((particule as XmlSchemaChoice).Items);
            }

            return null;
        }

        private List<XmlSchemaElement> GetAllSchemaElements(XmlSchemaObjectCollection collection)
        {
            var list = new List<XmlSchemaElement>();
            foreach (var item in collection)
            {
                if (item is XmlSchemaElement)
                {
                    list.Add(item as XmlSchemaElement);
                }
                else if (item is XmlSchemaChoice)
                {
                    var itemChoice = item as XmlSchemaChoice;
                    list.AddRange(GetAllSchemaElements(itemChoice.Items));
                }
                else if (item is XmlSchemaSequence)
                {
                    list.AddRange(GetAllSchemaElements((item as XmlSchemaSequence).Items));
                }
            }

            return list;
        }

        private string GetTypeForMember(CodeTypeDeclaration codeType, string memberName)
        {
            foreach (var member in codeType.Members)
            {
                var codeMemberProperty = member as CodeMemberProperty;
                if (codeMemberProperty != null)
                {
                    if (codeMemberProperty.Name == memberName)
                    {
                        return codeMemberProperty.Type.BaseType;
                    }
                }
            }

            return null;
        }

        private EnhancedProperty GetEnhancedPropertyFromAttributeAndUpdateAttribute(CodeAttributeDeclaration attribute)
        {
            var enhancedProperty = new EnhancedProperty();
            for (var i = 0; i < attribute.Arguments.Count; i++)
            {
                if (i == 0)
                {
                    CodePrimitiveExpression arg1 = attribute.Arguments[i].Value as CodePrimitiveExpression;
                    enhancedProperty.Name = arg1.Value as string;
                }
                else if (i == 1)
                {
                    CodeTypeOfExpression arg2 = attribute.Arguments[i].Value as CodeTypeOfExpression;
                    enhancedProperty.Type = arg2.Type.BaseType as string;
                }
                    /* // NOT WORKING with XmlSerializer.
                     * // If we remove the DataType attribute we get a "cannot create temporary class"
                     * // If we keep as int32, it says it doesn't match the type string.
                else if (i == 2)
                {
                    CodePrimitiveExpression arg3 = attribute.Arguments[i].Value as CodePrimitiveExpression;
                    if ((string)arg3.Value == "integer")
                    {
                        enhancedProperty.Type = "System.Int32";

                        // Change the arg2 to be int and remove arg3
                        //(attribute.Arguments[1].Value as CodeTypeOfExpression).Type = new CodeTypeReference("System.Int32");
                        //attribute.Arguments.RemoveAt(2);
                        //break;
                    }
                }
                     * */
            }

            var realType = Type.GetType(enhancedProperty.Type);
            if (realType == null) // We found a type that is part of our special types
            {
                var specialType = codeNamespaceTypes.FirstOrDefault(m => m.Name == enhancedProperty.Type);
                if (specialType != null)
                {
                    if (specialType.IsEnum)
                    {
                        enhancedProperty.IsNullable = true;
                    }
                }
            }
            else if (realType.IsValueType)
            {
                enhancedProperty.IsNullable = true;
            }

            return enhancedProperty;
        }

        #region CreateProperty
        private CodeMemberProperty CreateProperty(EnhancedProperty enhancedProperty, bool array)
        {
            var type = enhancedProperty.Type;
            if (array)
            {
                type = "IEnumerable<" + type + ">";
            }

            if (enhancedProperty.IsNullable && !array)
            {
                type = "Nullable<" + type + ">";
            }

            var codeTypeReference = new CodeTypeReference(type);
            var property = new CodeMemberProperty()
            {
                Type = codeTypeReference,
                Name = enhancedProperty.Name,
                Attributes = MemberAttributes.Public | MemberAttributes.Final
            };

            property.CustomAttributes.Add(ignoreAttribute);

            return property;
        }

        private CodeMemberProperty CreateArrayPropertyWithChoice(EnhancedProperty enhancedProperty, string choice)
        {
            var property = CreateProperty(enhancedProperty, true);

            //return ObjectItems.GetItems<type>(choice.name);
            property.GetStatements.Add(
                new CodeMethodReturnStatement(
                    new CodeMethodInvokeExpression(
                        new CodeMethodReferenceExpression(new CodeVariableReferenceExpression("ObjectItems"), "GetItems", new CodeTypeReference(enhancedProperty.Type)),
                        new CodePropertyReferenceExpression(new CodeVariableReferenceExpression(choice), enhancedProperty.Name)
                    )
                )
            );

            //ObjectItems.SetItems(ItemsChoiceType32.ListID, value.ToArray());
            property.SetStatements.Add(
                new CodeMethodInvokeExpression(
                    new CodeMethodReferenceExpression(new CodeVariableReferenceExpression("ObjectItems"), "SetItems"),
                    new CodePropertyReferenceExpression(new CodeVariableReferenceExpression(choice), enhancedProperty.Name),
                    new CodeMethodInvokeExpression(new CodeVariableReferenceExpression("value"), "ToArray")
                )
            );

            return property;
        }

        private CodeMemberProperty CreateArrayPropertyWithoutChoice(EnhancedProperty enhancedProperty)
        {
            var property = CreateProperty(enhancedProperty, true);

            //return ObjectItems.GetItems<type>();
            property.GetStatements.Add(
                new CodeMethodReturnStatement(
                    new CodeMethodInvokeExpression(
                        new CodeMethodReferenceExpression(new CodeVariableReferenceExpression("ObjectItems"), "GetItems", new CodeTypeReference(enhancedProperty.Type))
                    )
                )
            );

            //ObjectItems.SetItem(value);
            property.SetStatements.Add(
                new CodeMethodInvokeExpression(
                    new CodeMethodReferenceExpression(new CodeVariableReferenceExpression("ObjectItems"), "SetItems"),
                    new CodeMethodInvokeExpression(new CodeVariableReferenceExpression("value"), "ToArray")
                )
            );

            return property;
        }

        private CodeMemberProperty CreateSinglePropertyWithChoice(EnhancedProperty enhancedProperty, string choice)
        {
            var property = CreateProperty(enhancedProperty, false);

            //return ObjectItems.GetItem<type>(choice.name);
            property.GetStatements.Add(
                new CodeMethodReturnStatement(
                    new CodeMethodInvokeExpression(
                        new CodeMethodReferenceExpression(new CodeVariableReferenceExpression("ObjectItems"), "GetItem", new CodeTypeReference(enhancedProperty.Type)),
                        new CodePropertyReferenceExpression(new CodeVariableReferenceExpression(choice), enhancedProperty.Name)
                    )
                )
            );

            //ObjectItems.SetItem(ItemsChoiceType32.ListID, value);
            property.SetStatements.Add(
                new CodeMethodInvokeExpression(
                    new CodeMethodReferenceExpression(new CodeVariableReferenceExpression("ObjectItems"), "SetItem"),
                    new CodePropertyReferenceExpression(new CodeVariableReferenceExpression(choice), enhancedProperty.Name),
                    new CodeVariableReferenceExpression("value")
                )
            );

            return property;
        }

        private CodeMemberProperty CreateSinglePropertyWithoutChoice(EnhancedProperty enhancedProperty)
        {
            var property = CreateProperty(enhancedProperty, false);

            //return ObjectItems.GetItem<type>();
            property.GetStatements.Add(
                new CodeMethodReturnStatement(
                    new CodeMethodInvokeExpression(
                        new CodeMethodReferenceExpression(new CodeVariableReferenceExpression("ObjectItems"), "GetItem", new CodeTypeReference(enhancedProperty.Type))
                    )
                )
            );

            //ObjectItems.SetItem(value);
            property.SetStatements.Add(
                new CodeMethodInvokeExpression(
                    new CodeMethodReferenceExpression(new CodeVariableReferenceExpression("ObjectItems"), "SetItem"),
                    new CodeVariableReferenceExpression("value")
                )
            );

            return property;
        }
        #endregion

        private void AddObjectItems(CodeTypeDeclaration codeType, string withEnumChoice, bool useSingular)
        {
            var returnName = "ObjectItem";
            if (!useSingular)
            {
                returnName += "s";
            }

            var codeMethodReferenceExpression = new CodeMethodReferenceExpression(null, "ObjectItems");
            var returnType = new CodeTypeReference(returnName);

            if (!string.IsNullOrEmpty(withEnumChoice))
            {
                var typeArgument = new CodeTypeReference(new CodeTypeParameter(withEnumChoice));
                codeMethodReferenceExpression.TypeArguments.Add(typeArgument);
                returnType.TypeArguments.Add(typeArgument);
            }

            var property = new CodeMemberProperty
            {
                Name = "ObjectItems",
                Type = returnType
            };
            var assignment1 =
                new CodeConditionStatement(
                    new CodeBinaryOperatorExpression(
                        new CodeVariableReferenceExpression("objectItems"),
                        CodeBinaryOperatorType.ValueEquality,
                        new CodePrimitiveExpression()
                    )
                );
            assignment1.TrueStatements.Add(
                new CodeAssignStatement(
                    new CodeVariableReferenceExpression("objectItems"),
                    new CodeObjectCreateExpression(
                        returnType,
                        new CodeExpression[] { new CodeThisReferenceExpression() }
                    )
                ));

            property.GetStatements.Add(assignment1);
            property.GetStatements.Add(new CodeMethodReturnStatement(new CodeVariableReferenceExpression("objectItems")));

            codeType.Members.Add(property);
            codeType.Members.Add(new CodeMemberField(returnType, "objectItems"));
        }
    }
}
