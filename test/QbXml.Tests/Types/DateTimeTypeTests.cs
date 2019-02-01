using NUnit.Framework;
using QbSync.QbXml.Objects;
using QbSync.QbXml.Tests.Helpers;
using System;

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

        [Test]
        public void DateTimeLocalToString()
        {
            var utcDateTime = new DateTime(2015, 4, 3, 10, 6, 17, DateTimeKind.Utc);
            var timeZoneInfo = QuickBooksTestHelper.GetPacificStandardTimeZoneInfo();
            var localDateTime = TimeZoneInfo.ConvertTime(utcDateTime, timeZoneInfo);
            localDateTime = DateTime.SpecifyKind(localDateTime, DateTimeKind.Local);

            var dt = new DATETIMETYPE(localDateTime);

            Assert.AreEqual("2015-04-03T03:06:17-07:00", dt.ToString());
        }

        [Test]
        public void DateTimeUtcToString()
        {
            var utcDateTime = new DateTime(2015, 4, 3, 10, 6, 17, DateTimeKind.Utc);
            var dt = new DATETIMETYPE(utcDateTime);

            Assert.AreEqual("2015-04-03T10:06:17+00:00", dt.ToString());
        }
    }
}
