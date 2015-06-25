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
        private XmlSchemas xsds;
        private IEnumerable<CodeTypeDeclaration> codeNamespaceTypes;
        private IEnumerable<CodeMemberField> codeMemberFields;
        private IEnumerable<CodeMemberProperty> codeMemberProperties;
        private CodeAttributeDeclaration ignoreAttribute = new CodeAttributeDeclaration("System.Xml.Serialization.XmlIgnoreAttribute");
        private CodeAttributeDeclaration editorBrowsableStateNever = new CodeAttributeDeclaration("System.ComponentModel.EditorBrowsable", new CodeAttributeArgument(new CodeSnippetExpression("System.ComponentModel.EditorBrowsableState.Never")));

        public MemberEnhancer(CodeTypeDeclaration codeType, CodeNamespace codeNamespace, XmlSchemas xsds)
        {
            this.codeType = codeType;
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
            AddRestriction();
            AddItemsProperties();
            RemoveProperties(removeProperties);
            RemoveFields(removeFields);

            // We can't change this type if we serialize, but we can do it if we deserialize.
            ChangeType("System.String", "System.Int32", true, stringToIntProperties, stringToIntFields);
            ChangeType(null, "IteratorType", true, new string[] { "iterator" }, new string[] { "iteratorField" });
            InsertInterface();
        }

        private static IEnumerable<XmlSchemaElement> GetXmlSchemaElementFromTypeQuery(XmlSchemas xsds, string elementName)
        {
            var elementValuesQuery = from xsd in xsds
                                     let values = xsd.Elements.Values.Cast<XmlSchemaElement>()
                                     from el in values
                                     where el.QualifiedName.Name == elementName
                                     select el;

            return elementValuesQuery;
        }

        private static IEnumerable<XmlSchemaComplexType> GetXmlSchemaComplexTypeQuery(XmlSchemas xsds, string elementName)
        {
            var schemaTypeQuery = (from xsd in xsds
                                   let el = xsd.SchemaTypes[new XmlQualifiedName(elementName)]
                                   where el != null
                                   select el).OfType<XmlSchemaComplexType>();

            return schemaTypeQuery;
        }

        private static List<XmlSchemaChoice> FindTopLevelSchemaChoices(XmlSchemaObject element)
        {
            var list = new List<XmlSchemaChoice>();

            if (element is XmlSchemaSequence)
            {
                list.AddRange(WalkElements((element as XmlSchemaSequence).Items, FindTopLevelSchemaChoices));
            }
            else if (element is XmlSchemaChoice)
            {
                list.Add(element as XmlSchemaChoice);
            }

            return list;
        }

        private static List<XmlSchemaElement> WalkElement(XmlSchemaObject element)
        {
            var list = new List<XmlSchemaElement>();

            if (element is XmlSchemaSequence)
            {
                list.AddRange(WalkElements((element as XmlSchemaSequence).Items, WalkElement));
            }
            else if (element is XmlSchemaChoice)
            {
                list.AddRange(WalkElements((element as XmlSchemaChoice).Items, WalkElement));
            }
            else if (element is XmlSchemaElement)
            {
                list.Add(element as XmlSchemaElement);
            }

            return list;
        }

        private static List<T> WalkElements<T>(XmlSchemaObjectCollection element, Func<XmlSchemaObject, List<T>> action)
        {
            var list = new List<T>();
            foreach (var el in element as XmlSchemaObjectCollection)
            {
                list.AddRange(action(el));
            }

            return list;
        }

        private static IEnumerable<string> GetXmlSchemaElementNames(IEnumerable<XmlSchemaElement> xmlSchemaElements)
        {
            return xmlSchemaElements.Select(m => m.QualifiedName.Name);
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

            var propertySpecified = new CodeMemberProperty();
            propertySpecified.Attributes = MemberAttributes.Public | MemberAttributes.Final;
            propertySpecified.Name = p + "ValueSpecified";
            propertySpecified.Type = new CodeTypeReference(typeof(bool));
            propertySpecified.CustomAttributes.Add(new CodeAttributeDeclaration("System.Xml.Serialization.XmlIgnore"));
            propertySpecified.CustomAttributes.Add(editorBrowsableStateNever);
            propertySpecified.GetStatements.Add(
                hasValueExpression
            );

            var propertyName = codeAttributeDeclarationName.Contains("XmlAttributeAttribute") ? "AttributeName" : "ElementName";

            var propertyValue = new CodeMemberProperty();
            propertyValue.Attributes = MemberAttributes.Public | MemberAttributes.Final;
            propertyValue.Name = p + "Value";
            propertyValue.Type = new CodeTypeReference(typeName);
            propertyValue.CustomAttributes.Add(new CodeAttributeDeclaration(codeAttributeDeclarationName, new CodeAttributeArgument(propertyName, new CodePrimitiveExpression(p))));
            propertyValue.CustomAttributes.Add(editorBrowsableStateNever);
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

        private XmlSchemaMaxLengthFacet GetStringRestriction(XmlSchemaElement xmlSchemaElement)
        {
            var xmlSchemaSimpleType = xmlSchemaElement.SchemaType as XmlSchemaSimpleType;

            if (xmlSchemaSimpleType != null)
            {
                var xmlSchemaSimpleTypeRestriction = xmlSchemaSimpleType.Content as XmlSchemaSimpleTypeRestriction;

                if (xmlSchemaSimpleTypeRestriction != null)
                {
                    return xmlSchemaSimpleTypeRestriction.Facets.OfType<XmlSchemaMaxLengthFacet>().FirstOrDefault();
                }
            }

            return null;
        }

        private XmlSchemaElement GetSchemaElement(XmlSchemaComplexType xmlSchemaComplexType, string name)
        {
            var xmlSchemaSequence = xmlSchemaComplexType.ContentTypeParticle as XmlSchemaSequence;
            if (xmlSchemaSequence != null)
            {
                var xmlSchemaElementFound = xmlSchemaSequence.Items.OfType<XmlSchemaElement>()
                    .FirstOrDefault(m => m.Name == name);

                // Let's try to search in the choices.
                if (xmlSchemaElementFound == null)
                {
                    foreach (var choice in xmlSchemaSequence.Items.OfType<XmlSchemaChoice>())
                    {
                        xmlSchemaElementFound = choice.Items.OfType<XmlSchemaElement>().FirstOrDefault(m => m.Name == name);
                        if (xmlSchemaElementFound != null)
                        {
                            break;
                        }
                    }
                }

                return xmlSchemaElementFound;
            }

            return null;
        }

        private void AddStringLengthAttribute(CodeMemberProperty codeMemberProperty, int maxLength)
        {
            CodeAttributeDeclaration stringLengthAttribute = new CodeAttributeDeclaration("System.ComponentModel.DataAnnotations.StringLength", new CodeAttributeArgument(new CodePrimitiveExpression(maxLength)));
            codeMemberProperty.CustomAttributes.Add(stringLengthAttribute);
        }

        private CodeVariableReferenceExpression AddSubstringSetStatement(CodeStatementCollection setStatement, int maxLength)
        {
            // var temp = QbNormalizer.NormalizeString(value, maxLength);

            var normalizeString = new CodeMethodInvokeExpression(
                new CodeMethodReferenceExpression(new CodeVariableReferenceExpression("QbNormalizer"), "NormalizeString"),
                new CodeExpression[] {
                    new CodeVariableReferenceExpression("value"),
                    new CodePrimitiveExpression(maxLength)
                }
            );

            var declaration = new CodeVariableDeclarationStatement(typeof(string), "temp", normalizeString);
            var tempVariable = new CodeVariableReferenceExpression("temp");
            setStatement.Add(declaration);

            return tempVariable;
        }

        private void AddRestriction()
        {
            var xmlSchemaComplexType = GetXmlSchemaComplexType();

            if (xmlSchemaComplexType != null)
            {
                foreach (var codeMember in codeMemberProperties)
                {
                    if (codeMember.Type.BaseType == "System.String" && codeMember.Type.ArrayRank == 0)
                    {
                        var schemaElement = GetSchemaElement(xmlSchemaComplexType, codeMember.Name);
                        if (schemaElement != null)
                        {
                            var restriction = GetStringRestriction(schemaElement);
                            if (restriction != null)
                            {
                                var intValue = int.Parse(restriction.Value);
                                AddStringLengthAttribute(codeMember, intValue);
                                var originalLeftStatement = ((System.CodeDom.CodeAssignStatement)codeMember.SetStatements[0]).Left;
                                codeMember.SetStatements.Clear();
                                var tempVariable = AddSubstringSetStatement(codeMember.SetStatements, intValue);
                                codeMember.SetStatements.Add(new CodeAssignStatement(originalLeftStatement, tempVariable));
                            }
                        }
                    }
                }
            }
        }

        private XmlSchemaComplexType GetXmlSchemaComplexType()
        {
            var xmlSchemaComplexType = GetXmlSchemaComplexTypeQuery(xsds, codeType.Name).FirstOrDefault();
            if (xmlSchemaComplexType == null)
            {
                var xmlSchemaElement = GetXmlSchemaElementFromTypeQuery(xsds, codeType.Name).FirstOrDefault();
                if (xmlSchemaElement != null)
                {
                    xmlSchemaComplexType = xmlSchemaElement.SchemaType as XmlSchemaComplexType; // Let's check if we have a complex type within the element
                }
            }

            return xmlSchemaComplexType;
        }

        private void AddItemsProperties()
        {
            var xmlSchemaComplexType = GetXmlSchemaComplexType();
            var i = 0;
            List<XmlSchemaObject> choices = null;
            if (xmlSchemaComplexType != null)
            {
                choices = FindTopLevelSchemaChoices(xmlSchemaComplexType.ContentTypeParticle).Cast<XmlSchemaObject>().ToList();
            }

            var codeMembers = codeMemberProperties.Where(m => m.Name == "Items" || m.Name == "Item" || m.Name == "Item1").ToList();
            foreach (var codeMember in codeMembers)
            {
                IEnumerable<string> nameOrder = null;
                if (choices != null && i < choices.Count) // ValueAdjustment has minOccurs=0 & maxOccurs=0 which will trigger a bogus choice count.
                {
                    nameOrder = GetXmlSchemaElementNames(WalkElement(choices[i]));
                }

                // Let's change the base type to work with our ObjectItems
                if (codeMember.Type.BaseType != "System.Object")
                {
                    var objectType = new CodeTypeReference
                    {
                        ArrayRank = codeMember.Type.ArrayRank,
                        BaseType = "System.Object"
                    };
                    codeMember.Type = objectType;

                    codeMemberFields.First(m => m.Name == codeMember.Name.ToLower() + "Field").Type = objectType;
                }

                AddAppropriateProperties(codeMember, nameOrder, xmlSchemaComplexType);
                i++;
            }
        }

        private void AddAppropriateProperties(CodeMemberProperty codeMemberProperty, IEnumerable<string> nameOrder, XmlSchemaComplexType xmlSchemaComplexType)
        {
            var newMembers = new List<EnhancedProperty>();
            string withEnumChoiceName = null;
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
                    withEnumChoiceName = propertyName;
                }
            }

            string withEnumChoiceType = null;

            // Hide the properties
            codeMemberProperty.CustomAttributes.Add(editorBrowsableStateNever);
            if (withEnumChoiceName != null)
            {
                withEnumChoiceType = GetTypeForMember(codeType, withEnumChoiceName);
                codeType.Members.OfType<CodeMemberProperty>()
                    .First(m => m.Name == withEnumChoiceName)
                    .CustomAttributes.Add(editorBrowsableStateNever);
            }

            foreach (var enhancedProperty in newMembers)
            {
                var isArray = CheckIfPropertyIsArray(codeType.Name, enhancedProperty.Name);
                CodeMemberProperty property = CreateAppropriateProperty(withEnumChoiceType, enhancedProperty, isArray, codeMemberProperty.Name, xmlSchemaComplexType);
                codeType.Members.Add(property);
            }

            AddObjectItems(codeType, withEnumChoiceType, !codeMemberProperty.Name.StartsWith("Items"), codeMemberProperty, nameOrder);
        }

        private CodeMemberProperty CreateAppropriateProperty(string withEnumChoice, EnhancedProperty enhancedProperty, bool isArray, string codeTypeMemberName, XmlSchemaComplexType xmlSchemaComplexType)
        {
            CodeMemberProperty property = null;
            if (string.IsNullOrEmpty(withEnumChoice))
            {
                if (isArray)
                {
                    property = CreateArrayPropertyWithoutChoice(enhancedProperty, codeTypeMemberName);
                }
                else
                {
                    property = CreateSinglePropertyWithoutChoice(enhancedProperty, codeTypeMemberName, xmlSchemaComplexType);
                }
            }
            else
            {
                if (isArray)
                {
                    property = CreateArrayPropertyWithChoice(enhancedProperty, withEnumChoice, codeTypeMemberName);
                }
                else
                {
                    property = CreateSinglePropertyWithChoice(enhancedProperty, withEnumChoice, codeTypeMemberName, xmlSchemaComplexType);
                }
            }

            return property;
        }

        private XmlSchemaElement GetXmlSchemaElementFromPropertyType(string elementName, string propertyName)
        {
            var element = GetXmlSchemaElementFromTypeQuery(xsds, elementName).FirstOrDefault();
            XmlSchemaComplexType schemaType = null;
            if (element != null)
            {
                schemaType = (XmlSchemaComplexType)element.SchemaType;
            }
            else
            {
                var schemaTypeQuery = GetXmlSchemaComplexTypeQuery(xsds, elementName);
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
                // Special case if our parent is a choice, we might have a MaxOccurs on it

                if (xmlSchemaElement.MaxOccurs > 1)
                {
                    isArray = true;
                }
                else if (xmlSchemaElement.Parent as XmlSchemaChoice != null)
                {
                    isArray = (xmlSchemaElement.Parent as XmlSchemaChoice).MaxOccurs > 1;
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

        private CodeMemberProperty CreateArrayPropertyWithChoice(EnhancedProperty enhancedProperty, string choice, string codeTypeMemberName)
        {
            var propertyName = "Object" + codeTypeMemberName;
            var property = CreateProperty(enhancedProperty, true);

            //return ObjectItems.GetItems<type>(choice.name);
            property.GetStatements.Add(
                new CodeMethodReturnStatement(
                    new CodeMethodInvokeExpression(
                        new CodeMethodReferenceExpression(new CodeVariableReferenceExpression(propertyName), "GetItems", new CodeTypeReference(enhancedProperty.Type)),
                        new CodePropertyReferenceExpression(new CodeVariableReferenceExpression(choice), enhancedProperty.Name)
                    )
                )
            );

            //ObjectItems.SetItems(choice.name, value.ToArray());
            property.SetStatements.Add(
                new CodeMethodInvokeExpression(
                    new CodeMethodReferenceExpression(new CodeVariableReferenceExpression(propertyName), "SetItems"),
                    new CodePropertyReferenceExpression(new CodeVariableReferenceExpression(choice), enhancedProperty.Name),
                    new CodeMethodInvokeExpression(new CodeVariableReferenceExpression("value"), "ToArray")
                )
            );

            return property;
        }

        private CodeMemberProperty CreateArrayPropertyWithoutChoice(EnhancedProperty enhancedProperty, string codeTypeMemberName)
        {
            var propertyName = "Object" + codeTypeMemberName;
            var property = CreateProperty(enhancedProperty, true);

            //return ObjectItems.GetItems<type>(name);
            property.GetStatements.Add(
                new CodeMethodReturnStatement(
                    new CodeMethodInvokeExpression(
                        new CodeMethodReferenceExpression(new CodeVariableReferenceExpression(propertyName), "GetItems", new CodeTypeReference(enhancedProperty.Type)),
                        new CodePrimitiveExpression(enhancedProperty.Name)
                    )
                )
            );

            //ObjectItems.SetItems(name, value.ToArray());
            property.SetStatements.Add(
                new CodeMethodInvokeExpression(
                    new CodeMethodReferenceExpression(new CodeVariableReferenceExpression(propertyName), "SetItems"),
                    new CodePrimitiveExpression(enhancedProperty.Name),
                    new CodeMethodInvokeExpression(new CodeVariableReferenceExpression("value"), "ToArray")
                )
            );

            return property;
        }

        private CodeMemberProperty CreateSinglePropertyWithChoice(EnhancedProperty enhancedProperty, string choice, string codeTypeMemberName, XmlSchemaComplexType xmlSchemaComplexType)
        {
            var propertyName = "Object" + codeTypeMemberName;
            var property = CreateProperty(enhancedProperty, false);

            //return ObjectItems.GetItem<type>(choice.name);
            property.GetStatements.Add(
                new CodeMethodReturnStatement(
                    new CodeMethodInvokeExpression(
                        new CodeMethodReferenceExpression(new CodeVariableReferenceExpression(propertyName), "GetItem", new CodeTypeReference(enhancedProperty.Type)),
                        new CodePropertyReferenceExpression(new CodeVariableReferenceExpression(choice), enhancedProperty.Name)
                    )
                )
            );

            var saveReference = new CodeVariableReferenceExpression("value");
            if (enhancedProperty.Type == "System.String")
            {
                var schemaElement = GetSchemaElement(xmlSchemaComplexType, enhancedProperty.Name);
                if (schemaElement != null)
                {
                    var restriction = GetStringRestriction(schemaElement);
                    if (restriction != null)
                    {
                        var intValue = int.Parse(restriction.Value);
                        AddStringLengthAttribute(property, intValue);
                        saveReference = AddSubstringSetStatement(property.SetStatements, intValue);
                    }
                }
            }

            //ObjectItems.SetItem(choice.name, value);
            property.SetStatements.Add(
                new CodeMethodInvokeExpression(
                    new CodeMethodReferenceExpression(new CodeVariableReferenceExpression(propertyName), "SetItem"),
                    new CodePropertyReferenceExpression(new CodeVariableReferenceExpression(choice), enhancedProperty.Name),
                    saveReference
                )
            );

            return property;
        }

        private CodeMemberProperty CreateSinglePropertyWithoutChoice(EnhancedProperty enhancedProperty, string codeTypeMemberName, XmlSchemaComplexType xmlSchemaComplexType)
        {
            var propertyName = "Object" + codeTypeMemberName;
            var property = CreateProperty(enhancedProperty, false);

            //return ObjectItems.GetItem<type>(name);
            property.GetStatements.Add(
                new CodeMethodReturnStatement(
                    new CodeMethodInvokeExpression(
                        new CodeMethodReferenceExpression(new CodeVariableReferenceExpression(propertyName), "GetItem", new CodeTypeReference(enhancedProperty.Type)),
                        new CodePrimitiveExpression(enhancedProperty.Name)
                    )
                )
            );

            var saveReference = new CodeVariableReferenceExpression("value");
            if (enhancedProperty.Type == "System.String")
            {
                var schemaElement = GetSchemaElement(xmlSchemaComplexType, enhancedProperty.Name);
                if (schemaElement != null)
                {
                    var restriction = GetStringRestriction(schemaElement);
                    if (restriction != null)
                    {
                        var intValue = int.Parse(restriction.Value);
                        AddStringLengthAttribute(property, intValue);
                        saveReference = AddSubstringSetStatement(property.SetStatements, intValue);
                    }
                }
            }

            //ObjectItems.SetItem(name, value);
            property.SetStatements.Add(
                new CodeMethodInvokeExpression(
                    new CodeMethodReferenceExpression(new CodeVariableReferenceExpression(propertyName), "SetItem"),
                    new CodePrimitiveExpression(enhancedProperty.Name),
                    saveReference
                )
            );

            return property;
        }
        #endregion

        private void AddObjectItems(CodeTypeDeclaration codeType, string withEnumChoice, bool useSingular, CodeMemberProperty codeTypeMember, IEnumerable<string> nameOrder)
        {
            var propertyName = "Object" + codeTypeMember.Name;
            var fieldName = "object" + codeTypeMember.Name;
            var returnName = "ObjectItem";
            var constructorParameters = new List<CodeExpression> { new CodeThisReferenceExpression(), new CodePrimitiveExpression(codeTypeMember.Name) };
            if (!useSingular)
            {
                returnName += "s";

                if (nameOrder != null)
                {
                    var stringArray = new List<CodeExpression>();
                    stringArray.AddRange(nameOrder.Select(m => new CodePrimitiveExpression(m)));
                    var newOrderArray = new CodeArrayCreateExpression(typeof(string), stringArray.ToArray());
                    constructorParameters.Add(newOrderArray);
                }
            }

            var codeMethodReferenceExpression = new CodeMethodReferenceExpression(null, "ObjectItems");
            var returnType = new CodeTypeReference(returnName);

            List<CodeStatement> trueStatements = null;
            if (string.IsNullOrEmpty(withEnumChoice))
            {
                trueStatements = new List<CodeStatement>();
                trueStatements.Add(
                    new CodeVariableDeclarationStatement(typeof(Dictionary<System.Type, string>), "typeMapping",
                    new CodeObjectCreateExpression(typeof(Dictionary<System.Type, string>))));

                // We need to add a type mapping since we have not access to a choice name.
                foreach (var customAttribute in codeTypeMember.CustomAttributes.OfType<CodeAttributeDeclaration>())
                {
                    if (customAttribute.Name == "System.Xml.Serialization.XmlElementAttribute")
                    {
                        var name = ((CodePrimitiveExpression)customAttribute.Arguments[0].Value).Value;
                        var type = ((CodeTypeOfExpression)customAttribute.Arguments[1].Value).Type.BaseType;
                        trueStatements.Add(
                            new CodeExpressionStatement(
                                new CodeMethodInvokeExpression(
                                    new CodeVariableReferenceExpression("typeMapping"),
                                    "Add",
                                    new CodeSnippetExpression("typeof(" + type + ")"),
                                    new CodePrimitiveExpression(name)
                                )
                            )
                        );
                    }
                }

                constructorParameters.Add(new CodeVariableReferenceExpression("typeMapping"));
            }
            else
            {
                var typeArgument = new CodeTypeReference(new CodeTypeParameter(withEnumChoice));
                codeMethodReferenceExpression.TypeArguments.Add(typeArgument);
                returnType.TypeArguments.Add(typeArgument);
            }

            var property = new CodeMemberProperty
            {
                Name = propertyName,
                Type = returnType
            };
            var assignment1 =
                new CodeConditionStatement(
                    new CodeBinaryOperatorExpression(
                        new CodeVariableReferenceExpression(fieldName),
                        CodeBinaryOperatorType.ValueEquality,
                        new CodePrimitiveExpression()
                    )
                );

            if (trueStatements != null)
            {
                assignment1.TrueStatements.AddRange(trueStatements.ToArray());
            }

            assignment1.TrueStatements.Add(
                new CodeAssignStatement(
                    new CodeVariableReferenceExpression(fieldName),
                    new CodeObjectCreateExpression(
                        returnType,
                        constructorParameters.ToArray()
                    )
                ));

            property.GetStatements.Add(assignment1);
            property.GetStatements.Add(new CodeMethodReturnStatement(new CodeVariableReferenceExpression(fieldName)));

            codeType.Members.Add(property);
            codeType.Members.Add(new CodeMemberField(returnType, fieldName));
        }
    }
}
