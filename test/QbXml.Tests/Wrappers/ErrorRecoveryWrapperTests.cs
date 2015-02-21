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
    class ErrorRecoveryWrapperTests
    {
        [Test]
        public void TestProperties()
        {
            var item = new ErrorRecoveryWrapper(new ErrorRecovery());
            var guid = new GuidType(Guid.NewGuid());
            item.TxnNumber = "number";
            item.ListID = "test";
            item.ExternalGUID = guid;
            item.EditSequence = "123";

            Assert.AreEqual("number", item.TxnNumber);
            Assert.AreEqual("test", item.ListID);
            Assert.AreEqual(guid, item.ExternalGUID);
            Assert.AreEqual("123", item.EditSequence);
        }
    }
}
