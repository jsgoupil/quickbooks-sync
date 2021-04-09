using NUnit.Framework;
using QbSync.QbXml.Objects;
using QbSync.QbXml.Tests.Helpers;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace QbSync.QbXml.Tests.QbXml
{
    [TestFixture]
    class InvoiceAddRequestTests
    {
        [Test]
        public void BasicInvoiceAddRequestTest()
        {
            var request = new QbXmlRequest();
            var innerRequest = new InvoiceAddRqType
            {
                InvoiceAdd = new InvoiceAdd
                {
                    CustomerRef = new CustomerRef
                    {
                        ListID = "12345"
                    },
                    IsPending = true,
                    InvoiceLineAdd = new InvoiceLineAdd[]
                    {
                        new InvoiceLineAdd
                        {
                             Desc = "Desc1"
                        },
                        new InvoiceLineAdd
                        {
                             Desc = "Desc2",
                             SerialNumber = "123",
                        }
                    },
                }
            };
            request.AddToSingle(innerRequest);
            var xml = request.GetRequest();

            XmlDocument requestXmlDoc = new XmlDocument();
            requestXmlDoc.LoadXml(xml);

            Assert.AreEqual(1, requestXmlDoc.GetElementsByTagName("InvoiceAddRq").Count);

            var node = requestXmlDoc.SelectSingleNode("//InvoiceAddRq/InvoiceAdd");
            Assert.AreEqual(innerRequest.InvoiceAdd.CustomerRef.ListID, node.ReadNode("CustomerRef/ListID"));
            Assert.AreEqual(innerRequest.InvoiceAdd.IsPending.ToString(), node.ReadNode("IsPending"));

            var nodes2 = node.SelectNodes("InvoiceLineAdd");
            Assert.AreEqual(innerRequest.InvoiceAdd.InvoiceLineAdd.Count(), nodes2.Count);
            Assert.AreEqual(innerRequest.InvoiceAdd.InvoiceLineAdd.First().Desc, nodes2[0].ReadNode("Desc"));
            Assert.AreEqual(innerRequest.InvoiceAdd.InvoiceLineAdd.Last().Desc, nodes2[1].ReadNode("Desc"));
            Assert.AreEqual(innerRequest.InvoiceAdd.InvoiceLineAdd.Last().SerialNumber, nodes2[1].ReadNode("SerialNumber"));
            Assert.IsEmpty(QuickBooksTestHelper.GetXmlValidation(xml));
        }

        [Test]
        public void BasicInvoiceAddRequest_TooLong_Test()
        {
            var request = new QbXmlRequest();
            var innerRequest = new InvoiceAddRqType
            {
                InvoiceAdd = new InvoiceAdd
                {
                    CustomerRef = new CustomerRef
                    {
                        ListID = "12345"
                    },
                    IsPending = true,
                    InvoiceLineAdd = new InvoiceLineAdd[]
                    {
                        new InvoiceLineAdd
                        {
                             Desc = "Desc1",
                             LotNumber = "ABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZ" // 52 characters > 40
                        },
                    },
                }
            };
            request.AddToSingle(innerRequest);
            var xml = request.GetRequest();

            XmlDocument requestXmlDoc = new XmlDocument();
            requestXmlDoc.LoadXml(xml);

            Assert.AreEqual(1, requestXmlDoc.GetElementsByTagName("InvoiceAddRq").Count);

            var node = requestXmlDoc.SelectSingleNode("//InvoiceAddRq/InvoiceAdd");
            Assert.AreEqual(innerRequest.InvoiceAdd.CustomerRef.ListID, node.ReadNode("CustomerRef/ListID"));
            Assert.AreEqual(innerRequest.InvoiceAdd.IsPending.ToString(), node.ReadNode("IsPending"));

            var nodes2 = node.SelectNodes("InvoiceLineAdd");
            Assert.AreEqual(innerRequest.InvoiceAdd.InvoiceLineAdd.Count(), nodes2.Count);
            Assert.AreEqual(innerRequest.InvoiceAdd.InvoiceLineAdd.First().Desc, nodes2[0].ReadNode("Desc"));
            Assert.AreEqual("ABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZ".Substring(0, 40), innerRequest.InvoiceAdd.InvoiceLineAdd.First().LotNumber);
            Assert.AreEqual(innerRequest.InvoiceAdd.InvoiceLineAdd.First().LotNumber, nodes2[0].ReadNode("LotNumber"));
            Assert.IsEmpty(QuickBooksTestHelper.GetXmlValidation(xml));
        }

        [Test]
        public void BasicInvoiceAddRequest_Order_Test()
        {
            var request = new QbXmlRequest();
            var innerRequest = new InvoiceAddRqType
            {
                InvoiceAdd = new InvoiceAdd
                {
                    CustomerRef = new CustomerRef
                    {
                        ListID = "12345"
                    },
                    IsPending = true
                }
            };

            var items = new List<InvoiceLineAdd>();

            for (var i = 0; i < 10; i++)
            {
                items.Add(new InvoiceLineAdd
                {
                    Desc = $"Test Item {i} for Test"
                });
                items.Add(new InvoiceLineAdd
                {
                    Desc = $"Another Test {i} item"
                });
            }

            innerRequest.InvoiceAdd.InvoiceLineAdd = items.ToArray();

            request.AddToSingle(innerRequest);
            var xml = request.GetRequest();

            XmlDocument requestXmlDoc = new XmlDocument();
            requestXmlDoc.LoadXml(xml);

            Assert.AreEqual(1, requestXmlDoc.GetElementsByTagName("InvoiceAddRq").Count);

            var node = requestXmlDoc.SelectSingleNode("//InvoiceAddRq/InvoiceAdd");
            Assert.AreEqual(innerRequest.InvoiceAdd.CustomerRef.ListID, node.ReadNode("CustomerRef/ListID"));
            Assert.AreEqual(innerRequest.InvoiceAdd.IsPending.ToString(), node.ReadNode("IsPending"));

            var nodes2 = node.SelectNodes("InvoiceLineAdd");
            Assert.AreEqual(innerRequest.InvoiceAdd.InvoiceLineAdd.Count(), nodes2.Count);

            for (var i = 0; i < 20; i += 2)
            {
                var c = i / 2;
                Assert.AreEqual($"Test Item {c} for Test", nodes2[i].ReadNode("Desc"));
                Assert.AreEqual($"Another Test {c} item", nodes2[i + 1].ReadNode("Desc"));
            }

            Assert.IsEmpty(QuickBooksTestHelper.GetXmlValidation(xml));
        }
    }
}