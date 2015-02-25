using NUnit.Framework;
using QbSync.QbXml.Objects;
using QbSync.QbXml.Tests.Helpers;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace QbSync.QbXml.Tests.QbXml
{
    [TestFixture]
    class ItemSitesQueryRequestTests
    {
        [Test]
        public void BasicItemSitesQueryRequestTests()
        {
            var request = new QbXmlRequest();
            var innerRequest = new ItemSitesQueryRqType();
            innerRequest.ItemSiteFilter = new ItemSiteFilter
            {
                ItemFilter = new ItemFilter
                {
                    FullName = new List<string> { "NameHere" }
                }
            };

            request.AddToSingle(innerRequest);
            var xml = request.GetRequest();

            XmlDocument requestXmlDoc = new XmlDocument();
            requestXmlDoc.LoadXml(xml);

            Assert.AreEqual(1, requestXmlDoc.GetElementsByTagName("ItemSitesQueryRq").Count);

            Assert.AreEqual(innerRequest.ItemSiteFilter.ItemFilter.FullName.First(), requestXmlDoc.GetElementsByTagName("FullName").Item(0).InnerText);
            Assert.IsEmpty(QuickBooksTestHelper.GetXmlValidation(xml));
        }
    }
}