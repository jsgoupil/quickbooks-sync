using NUnit.Framework;
using QbSync.QbXml.Objects;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Xml.Serialization;

namespace QbSync.QbXml.Tests.Types
{
    [TestFixture]
    public class ObjectTests
    {
        [Test]
        public void PropertyHasOrder()
        {
            var salesOrderAddType = typeof(SalesOrderAdd);
            var properties = salesOrderAddType.GetProperties();
            var customerRefProperty = properties.First(m => m.Name == "CustomerRef");

            Assert.AreEqual(1, customerRefProperty.CustomAttributes.Count());
            Assert.AreEqual(1, customerRefProperty.CustomAttributes.First().NamedArguments.Count);
            Assert.AreEqual("Order", customerRefProperty.CustomAttributes.First().NamedArguments[0].MemberName);
            Assert.AreEqual(0, customerRefProperty.CustomAttributes.First().NamedArguments[0].TypedValue.Value);
        }

        [Test]
        public void ItemsHasOrder()
        {
            var salesOrderAddType = typeof(SalesOrderAdd);
            var properties = salesOrderAddType.GetProperties();
            var itemsProperty = properties.First(m => m.Name == "Items");

            Assert.AreEqual(3, itemsProperty.CustomAttributes.Count());
            var xmlElementAttributes = itemsProperty.CustomAttributes.Where(m => m.AttributeType == typeof(XmlElementAttribute)).ToList();
            Assert.AreEqual(2, xmlElementAttributes.Count);
            Assert.AreEqual(1, xmlElementAttributes.First().NamedArguments.Count);
            Assert.AreEqual("Order", xmlElementAttributes.First().NamedArguments[0].MemberName);
            Assert.AreEqual(25, xmlElementAttributes.First().NamedArguments[0].TypedValue.Value);

            Assert.AreEqual(1, xmlElementAttributes.Skip(1).First().NamedArguments.Count);
            Assert.AreEqual("Order", xmlElementAttributes.Skip(1).First().NamedArguments[0].MemberName);
            Assert.AreEqual(25, xmlElementAttributes.Skip(1).First().NamedArguments[0].TypedValue.Value);
        }

        [Test]
        public void HasRestriction()
        {
            var salesOrderAddType = typeof(SalesOrderAdd);
            var properties = salesOrderAddType.GetProperties();
            var otherProperty = properties.First(m => m.Name == "Other");

            Assert.AreEqual(2, otherProperty.CustomAttributes.Count());
            var stringLengthAttribute = otherProperty.CustomAttributes.First(m => m.AttributeType == typeof(StringLengthAttribute));
            Assert.AreEqual(29, stringLengthAttribute.ConstructorArguments[0].Value);
        }
    }
}