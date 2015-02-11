using NUnit.Framework;
using QbSync.QbXml.Extensions;
using QbSync.QbXml.Messages.Requests;
using QbSync.QbXml.Struct;
using QbSync.QbXml.Tests.Helpers;
using QbSync.QbXml.Type;
using System;
using System.Collections.Generic;
using System.Xml;

namespace QbSync.QbXml.Tests.QbXml
{
    [TestFixture]
    class DataExtDefDelRequestTests
    {
        [Test]
        public void BasicDataExtDefDelRequestTest()
        {
            var dataExtDefDelRequest = new DataExtDefDelRequest();
            dataExtDefDelRequest.OwnerID = Guid.NewGuid();
            dataExtDefDelRequest.DataExtName = "name";

            var request = dataExtDefDelRequest.GetRequest();

            XmlDocument requestXmlDoc = new XmlDocument();
            requestXmlDoc.LoadXml(request);

            Assert.AreEqual(1, requestXmlDoc.GetElementsByTagName("DataExtDefDelRq").Count);

            var node = requestXmlDoc.SelectSingleNode("//DataExtDefDelRq");
            QBAssert.AreEqual(dataExtDefDelRequest.OwnerID, node.ReadNode("OwnerID"));
            QBAssert.AreEqual(dataExtDefDelRequest.DataExtName, node.ReadNode("DataExtName"));
        }
    }
}