using NUnit.Framework;
using QbSync.QbXml.Objects;
using QbSync.QbXml.Tests.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QbSync.QbXml.Tests.Types
{
    [TestFixture]
    public class DateTimeTypeTests
    {
        [Test]
        public void DateTimeWithDSTNothingHappensWhenNoTimzoneSupplied()
        {
            var dt = new DATETIMETYPE("2015-04-03T10:06:17-08:00");

            Assert.AreEqual(18, dt.ToDateTime().ToUniversalTime().Hour);
        }

        [Test]
        public void DateTimeWithDSTIsNotHandledByQuickBooks()
        {
            // This should read 2015-04-03T10:06:17-07:00
            // But QuickBooks does not handle DST. So we need to handle it ourself.
            var dt = new DATETIMETYPE("2015-04-03T10:06:17-08:00", QuickBooksTestHelper.GetPacificStandardTimeZoneInfo());

            Assert.AreEqual(17, dt.ToDateTime().ToUniversalTime().Hour);
        }

        [Test]
        public void DateTimeWithNoDSTIsNotHandledByQuickBooks()
        {
            var dt = new DATETIMETYPE("2015-03-01T10:06:17-08:00", QuickBooksTestHelper.GetPacificStandardTimeZoneInfo());

            Assert.AreEqual(18, dt.ToDateTime().ToUniversalTime().Hour);
        }

        [Test]
        public void DateTimeInvalidSentByQuickBooks()
        {
            var dt = new DATETIMETYPE("0000-00-00", QuickBooksTestHelper.GetPacificStandardTimeZoneInfo());

            Assert.AreEqual(DateTime.MinValue, dt.ToDateTime());
        }
    }
}
