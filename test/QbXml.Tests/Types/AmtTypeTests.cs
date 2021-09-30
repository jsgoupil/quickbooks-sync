using NUnit.Framework;
using QbSync.QbXml.Objects;

namespace QbSync.QbXml.Tests.Types
{
    [TestFixture]
    public class AmtTypeTests
    {
        [Test]
        public void ToStringTooManyDecimal()
        {
            var amtType = new AMTTYPE(0.166667m);
            Assert.AreEqual("0.17", amtType.ToString());
        }

        [Test]
        public void ToStringTooManyDecimal_DoesNotDoBanker()
        {
            var amtType = new AMTTYPE(0.165m);
            Assert.AreEqual("0.17", amtType.ToString());
        }

        [Test]
        public void ToStringBigNumber()
        {
            var amtType = new AMTTYPE(123456.16m);
            Assert.AreEqual("123456.16", amtType.ToString());
        }

        [Test]
        public void ToStringAlwaysExactDecimals()
        {
            var amtType = new AMTTYPE(0.1m);
            Assert.AreEqual("0.10", amtType.ToString());
        }
    }
}
