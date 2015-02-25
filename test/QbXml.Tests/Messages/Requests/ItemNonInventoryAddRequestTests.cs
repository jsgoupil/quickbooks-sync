using NUnit.Framework;
using QbSync.QbXml.Extensions;
using QbSync.QbXml.Objects;
using QbSync.QbXml.Tests.Helpers;
using System;
using System.Xml;

namespace QbSync.QbXml.Tests.QbXml
{
    [TestFixture]
    class ItemNonInventoryAddRequestTests
    {
        [Test]
        public void BasicItemNonInventoryAddRequestTest()
        {
            var request = new QbXmlRequest();
            var innerRequest = new ItemNonInventoryAddRqType();
            innerRequest.ItemNonInventoryAdd = new ItemNonInventoryAdd
            {
                Name = "Something here",
                IsActive = true,
                SalesAndPurchase = new SalesAndPurchase()
            };
            request.AddToSingle(innerRequest);
            var xml = request.GetRequest();

            XmlDocument requestXmlDoc = new XmlDocument();
            requestXmlDoc.LoadXml(xml);

            Assert.AreEqual(1, requestXmlDoc.GetElementsByTagName("ItemNonInventoryAddRq").Count);

            var node = requestXmlDoc.SelectSingleNode("//ItemNonInventoryAddRq/ItemNonInventoryAdd");
            Assert.AreEqual(innerRequest.ItemNonInventoryAdd.Name, node.ReadNode("Name"));
            Assert.IsEmpty(QuickBooksTestHelper.GetXmlValidation(xml));
        }
    }
}