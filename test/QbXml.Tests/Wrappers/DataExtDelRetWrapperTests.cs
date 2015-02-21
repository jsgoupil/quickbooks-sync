using NUnit.Framework;
using QbSync.QbXml.Objects;
using QbSync.QbXml.Type;
using QbSync.QbXml.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QbSync.QbXml.Tests.Wrappers
{
    [TestFixture]
    class DataExtDelRetWrapperTests
    {
        [Test]
        public void TestProperties()
        {
            var item = new DataExtDelRetWrapper();
            var guid = new GuidType(Guid.NewGuid());

            item.DataExtName = "name";
            item.ListDataExt = ListDataExtType.Employee;
            item.ListObjRef = new ListObjRef
            {
                ListID = "123"
            };
            item.OwnerID = guid;
            item.TxnID = "456";

            Assert.AreEqual("name", item.DataExtName);
            Assert.AreEqual(ListDataExtType.Employee, item.ListDataExt);
            Assert.AreEqual("123", item.ListObjRef.ListID);
            Assert.AreEqual(guid, item.OwnerID);
            Assert.AreEqual("456", item.TxnID);
        }
    }
}
