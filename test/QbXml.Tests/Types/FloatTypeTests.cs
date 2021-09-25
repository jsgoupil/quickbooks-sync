using NUnit.Framework;
using QbSync.QbXml.Objects;
using QbSync.QbXml.Tests.Helpers;
using System;
using System.Xml;


namespace QbSync.QbXml.Tests.Types
{
    [TestFixture]
    public class FloatTypeTests
    {
        [Test]
        public void QUANTypeSupportsFiveDecimalPoints()
        {
            var Q = new QUANTYPE(0.16667M);
            Assert.AreEqual("0.16667", Q.ToString());
        }
        [Test]
        public void AMTTypeLimitedTwoDecimalPoints()
        {
            var Q = new AMTTYPE(0.16667M);
            Assert.AreEqual("0.17", Q.ToString());
        }
    }
}
