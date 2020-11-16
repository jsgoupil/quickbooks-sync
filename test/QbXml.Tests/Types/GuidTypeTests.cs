using NUnit.Framework;
using QbSync.QbXml.Objects;
using QbSync.QbXml.Tests.Helpers;
using System;
using System.Xml;

namespace QbSync.QbXml.Tests.Types
{
    [TestFixture]
    public class GuidTypeTests
    {
        [Test]
        public void GuidToStringWillBeZeroIfConstructedFromZero()
        {
            /*
             * From Intuit's documentation:
             * A GUIDTYPE value can be zero (0), or it can take the form {XXXXXXXX-XXXX-XXXX-XXXX-XXXXXXXXXXXX} where X is a hexadecimal digit. For example: {6B063959-81B0-4622-85D6-F548C8CCB517}
             *
             * Zero requires special handling
             */

            var guid = new GUIDTYPE("0");
            Assert.AreEqual("0", guid.ToString());
        }

        [Test]
        public void GuidToStringWillBeHexadecimalWhenConstructedFromEmptyGuid()
        {
            //Retain hex formatting if constructed from a GUID, to align with previous behavior of this type as much as possible

            var guid = new GUIDTYPE(Guid.Empty);
            Assert.AreEqual("{00000000-0000-0000-0000-000000000000}", guid.ToString());
        }


        [Test]
        public void GuidToStringWillBeHexadecimalWhenConstructedFromHexadecimal()
        {
            //Retain hex formatting if parsed from hex format, to align with previous behavior of this type as much as possible

            var guid = new GUIDTYPE("{00000000-0000-0000-0000-000000000000}");
            Assert.AreEqual("{00000000-0000-0000-0000-000000000000}", guid.ToString());
        }

        [Test]
        public void GuidToStringWillBeZeroInQbXmlResponseWhenParsedFromZero()
        {
            //Deserializing from XML will instantiate the GUIDTYPE with its parameterless constructor, Then use ReadXML to set the value
            //Ensure that reading from XML will also handle the Zero (0) format

            var ret = "<DataExtRet><OwnerID>0</OwnerID><DataExtName>name</DataExtName><DataExtType>STR255TYPE</DataExtType><DataExtValue>value</DataExtValue></DataExtRet>";

            var response = new QbXmlResponse();
            var rs = response.GetSingleItemFromResponse<DataExtAddRsType>(QuickBooksTestHelper.CreateQbXmlWithEnvelope(ret, "DataExtAddRs"));
            var dataExt = rs.DataExtRet;

            Assert.AreEqual("0", dataExt.OwnerID.ToString());
        }

        [Test]
        public void GuidToStringWillBeHexadecimalInQbXmlResponseWhenParsedFromHexadecimalEmptyGuid()
        {
            //Deserializing from XML will instantiate the GUIDTYPE with its parameterless constructor, Then use ReadXML to set the value
            //Retain hex formatting when input is also in hex format

            var ret = "<DataExtRet><OwnerID>{00000000-0000-0000-0000-000000000000}</OwnerID><DataExtName>name</DataExtName><DataExtType>STR255TYPE</DataExtType><DataExtValue>value</DataExtValue></DataExtRet>";

            var response = new QbXmlResponse();
            var rs = response.GetSingleItemFromResponse<DataExtAddRsType>(QuickBooksTestHelper.CreateQbXmlWithEnvelope(ret, "DataExtAddRs"));
            var dataExt = rs.DataExtRet;

            Assert.AreEqual("{00000000-0000-0000-0000-000000000000}", dataExt.OwnerID.ToString());
        }

        [Test]
        public void GuidWillOutputZeroInQbXmlRequestWhenConstructedFromZero()
        {
            var request = new QbXmlRequest();
            var innerRequest = new InvoiceQueryRqType
            {
                OwnerID = new[] { new GUIDTYPE("0") }
            };
            request.AddToSingle(innerRequest);
            var xml = request.GetRequest();

            XmlDocument requestXmlDoc = new XmlDocument();
            requestXmlDoc.LoadXml(xml);

            Assert.AreEqual("0", requestXmlDoc.GetElementsByTagName("OwnerID").Item(0).InnerText);
        }

        [Test]
        public void GuidWillOutputHexadecimalInQbXmlRequestWhenConstructedFromEmptyGuid()
        {
            var request = new QbXmlRequest();
            var innerRequest = new InvoiceQueryRqType
            {
                OwnerID = new[] { new GUIDTYPE(Guid.Empty) }
            };
            request.AddToSingle(innerRequest);
            var xml = request.GetRequest();

            XmlDocument requestXmlDoc = new XmlDocument();
            requestXmlDoc.LoadXml(xml);

            Assert.AreEqual("{00000000-0000-0000-0000-000000000000}", requestXmlDoc.GetElementsByTagName("OwnerID").Item(0).InnerText);
        }

        [Test]
        public void GuidWillOutputHexadecimalInQbXmlRequestWhenConstructedFromHexadecimal()
        {
            var request = new QbXmlRequest();
            var innerRequest = new InvoiceQueryRqType
            {
                OwnerID = new[] { new GUIDTYPE("{00000000-0000-0000-0000-000000000000}") }
            };
            request.AddToSingle(innerRequest);
            var xml = request.GetRequest();

            XmlDocument requestXmlDoc = new XmlDocument();
            requestXmlDoc.LoadXml(xml);

            Assert.AreEqual("{00000000-0000-0000-0000-000000000000}", requestXmlDoc.GetElementsByTagName("OwnerID").Item(0).InnerText);
        }
    }
}