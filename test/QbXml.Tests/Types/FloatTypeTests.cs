using NUnit.Framework;
using QbSync.QbXml.Objects;

namespace QbSync.QbXml.Tests.Types
{
    [TestFixture]
    public class FloatTypeTests
    {
        [Test]
        public void ToStringTooManyDecimal()
        {
            var floatType = new FLOATTYPE(0.166667M);
            Assert.AreEqual("0.16667", floatType.ToString());
        }

        [Test]
        public void ToStringTooManyDecimal_DoesNotDoBanker()
        {
            var floatType = new FLOATTYPE(0.166665M);
            Assert.AreEqual("0.16667", floatType.ToString());
        }

        [Test]
        public void ToStringFiveDecimalPoints()
        {
            var floatType = new FLOATTYPE(0.16667M);
            Assert.AreEqual("0.16667", floatType.ToString());
        }

        [Test]
        public void ToStringDoesNotAddSignificantZeros()
        {
            var floatType = new FLOATTYPE(0.16M);
            Assert.AreEqual("0.16", floatType.ToString());
        }
    }
}
