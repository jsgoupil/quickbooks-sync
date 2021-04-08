using NUnit.Framework;
using QbSync.QbXml.Objects;
using QbSync.QbXml.Tests.Helpers;
using System;

namespace QbSync.QbXml.Tests.Types
{
    [TestFixture]
    public class DateTimeTypeTests
    {
        [TestCase("2015-04-03T10:06:17-08:00", ExpectedResult = "2015-04-03T10:06:17")]
        [TestCase("2015-04-03T10:06:17-07:00", ExpectedResult = "2015-04-03T10:06:17")]
        [TestCase("2015-04-03T10:06:17Z", ExpectedResult = "2015-04-03T10:06:17")]
        [TestCase("2015-04-03T10:06:17", ExpectedResult = "2015-04-03T10:06:17")]
        public string ToStringOffsetExistenceMatchesInputWhenParsingFromString(string input)
        {
            return DATETIMETYPE.Parse(input).ToString();
        }

        [TestCase("2015-04-03T10:06:17-08:00", ExpectedResult = "2015-04-03T10:06:17")]
        [TestCase("2015-04-03T10:06:17-07:00", ExpectedResult = "2015-04-03T10:06:17")]
        [TestCase("2015-04-03T10:06:17Z", ExpectedResult = "2015-04-03T10:06:17")]
        [TestCase("2015-04-03T10:06:17", ExpectedResult = "2015-04-03T10:06:17")]
        public string ToStringOffsetExistenceMatchesInputWhenParsingFromStringWithoutOffset(string input)
        {
            return DATETIMETYPE.Parse(input).ToString(false);
        }

        [TestCase("2015-04-03T10:06:17-08:00", ExpectedResult = "2015-04-03T10:06:17-08:00")]
        [TestCase("2015-04-03T10:06:17-07:00", ExpectedResult = "2015-04-03T10:06:17-07:00")]
        [TestCase("2015-04-03T10:06:17Z", ExpectedResult = "2015-04-03T10:06:17+00:00", Description = "UTC Z offset format will change to +00:00")]
        [TestCase("2015-04-03T10:06:17", ExpectedResult = "2015-04-03T10:06:17", Description = "UTC Offset is optional")]
        public string ToStringOffsetExistenceMatchesInputWhenParsingFromStringWithOffset(string input)
        {
            return DATETIMETYPE.Parse(input).ToString(true);
        }

        [Test]
        public void RoundTripsAsExpectedWithoutOffset()
        {
            // Simulated value returned from QuickBooks, that we want to be sent the exact same way to future queries
            var str = "2019-02-20T10:36:51";
            var date = DATETIMETYPE.Parse(str);

            // DATETIMETYPE > string
            Assert.AreEqual(str, date.ToString());

            // DATETIMETYPE > string > DATETIMETYPE > string
            Assert.AreEqual(str, DATETIMETYPE.Parse(date.ToString()).ToString());

            // DATETIMETYPE > DateTime > DATETIMETYPE > string
            Assert.AreEqual(str, new DATETIMETYPE(date.ToDateTime()).ToString());

            // DATETIMETYPE > DateTime > string > DATETIMETYPE > string
            Assert.AreEqual(str, DATETIMETYPE.Parse(date.ToDateTime().ToString()).ToString());

            // DATETIMETYPE > DateTime > string > DateTime > DATETIMETYPE > string
            Assert.AreEqual(str, new DATETIMETYPE(DateTime.Parse(date.ToDateTime().ToString())).ToString());

            // DATETIMETYPE > DateTime > Ticks > DateTime > DATETIMETYPE > string
            Assert.AreEqual(str, new DATETIMETYPE(new DateTime(date.ToDateTime().Ticks)).ToString());
        }

        [Test]
        public void RoundTripsAsExpectedWithOffset()
        {
            // Simulated value constructed with an offset. Either from previous versions of the library, or a manually constructed value
            var date = DATETIMETYPE.Parse("2019-02-20T10:36:51-08:00");

            // DATETIMETYPE > string
            Assert.AreEqual("2019-02-20T10:36:51", date.ToString());

            // DATETIMETYPE > string
            Assert.AreEqual("2019-02-20T10:36:51-08:00", date.ToString(true));

            // DATETIMETYPE > string
            Assert.AreEqual("2019-02-20T10:36:51", date.ToString(false));

            // DATETIMETYPE > string > DATETIMETYPE > string
            Assert.AreEqual("2019-02-20T10:36:51-08:00", DATETIMETYPE.Parse(date.ToString(true)).ToString(true));


            // *********************************************************************************
            // NOTE:
            // The rest of these tests convert to DateTime. We'll lose offset information.
            // That is OK as long as the time component does not change
            // *********************************************************************************

            // DATETIMETYPE > DateTime > DATETIMETYPE > string
            Assert.AreEqual("2019-02-20T10:36:51", new DATETIMETYPE(date.ToDateTime()).ToString());

            // DATETIMETYPE > DateTime > string > DATETIMETYPE > string
            Assert.AreEqual("2019-02-20T10:36:51", DATETIMETYPE.Parse(date.ToDateTime().ToString()).ToString());

            // DATETIMETYPE > DateTime > string > DateTime > DATETIMETYPE > string
            Assert.AreEqual("2019-02-20T10:36:51", new DATETIMETYPE(DateTime.Parse(date.ToDateTime().ToString())).ToString());
        }

        [Test]
        public void ToStringDoesNotIncludeOffsetWhenConstructedFromUnspecifiedDateTime()
        {
            var date = new DateTime(2019, 2, 6, 17, 24, 0, DateTimeKind.Unspecified);
            var dt = new DATETIMETYPE(date);

            Assert.AreEqual("2019-02-06T17:24:00", dt.ToString());
        }

        [Test]
        public void ToStringDoesIncludesOffsetWhenConstructedFromUtcDateTime()
        {
            var date = new DateTime(2019, 2, 6, 17, 24, 0, DateTimeKind.Utc);
            var dt = new DATETIMETYPE(date);

            Assert.AreEqual("2019-02-06T17:24:00+00:00", dt.ToString(true));
        }

        [Test]
        public void ToStringIncludesOffsetWhenConstructedFromLocalDateTime()
        {
            var date = new DateTime(2019, 2, 6, 17, 24, 0, DateTimeKind.Local);

            // Note, this will be +00:00, not Z on a UTC machine, because of DateTimeKind.Local
            var offset = date.ToString(" K").Trim(); 

            var dt = new DATETIMETYPE(date);

            Assert.AreEqual($"2019-02-06T17:24:00{offset}", dt.ToString(true));
        }

        [Test]
        public void MidnightIsAssumedIfTimeComponentMissing()
        {
            var dt = DATETIMETYPE.Parse("2019-02-06");
            var date = dt.ToDateTime();

            Assert.AreEqual("2019-02-06T00:00:00", dt.ToString());
            Assert.AreEqual(new DateTime(2019, 2, 6, 0, 0, 0), date);
        }

        [Test]
        public void UsesOffsetAsSuppliedWhenConstructedFromDateTimeOffset()
        {
            var dt = DATETIMETYPE.FromUncorrectedDate(new DateTimeOffset(2019, 2, 6, 17, 24, 0, TimeSpan.FromHours(-8)));

            Assert.AreEqual("2019-02-06T17:24:00-08:00", dt.ToString(true));
        }

        [Test]
        public void UsesNoOffsetWhenConstructedFromDateComponentsWithoutOffset()
        {
            var dt = new DATETIMETYPE(2019, 2, 6, 17, 24, 0);

            Assert.AreEqual("2019-02-06T17:24:00", dt.ToString());
        }

        [Test]
        public void CompareAccountsForOffset()
        {
            // A's instant (moment in time globally) is later, but its DateTime is earlier
            var a = DATETIMETYPE.FromUncorrectedDate(new DateTimeOffset(2019, 1, 1, 6, 0, 0, 0, TimeSpan.FromHours(-3)));

            // B's instant is earlier, but its DateTime is later
            var b = DATETIMETYPE.FromUncorrectedDate(new DateTimeOffset(2019, 1, 1, 8, 0, 0, 0, TimeSpan.Zero));

            Assert.AreEqual(1, a.CompareTo(b));
        }

        [Test]
        public void CompareIgnoresOffsetWhenOneDoNotHaveOffset()
        {
            var a = DATETIMETYPE.FromUncorrectedDate(new DateTimeOffset(2019, 1, 1, 8, 0, 0, 0, TimeSpan.FromHours(-10)));
            var b = new DATETIMETYPE(new DateTime(2019, 1, 1, 8, 0, 0, 0, DateTimeKind.Unspecified));

            Assert.AreEqual(0, a.CompareTo(b));
        }

        [Test]
        public void EqualsOperatorSameInstance()
        {
            var a = new DATETIMETYPE(2019, 1, 1);
            var b = a;

            Assert.IsTrue(a == b);
        }

        [Test]
        public void EqualsOperatorSameValue()
        {
            var a = new DATETIMETYPE(2019, 1, 1);
            var b = new DATETIMETYPE(2019, 1, 1);

            Assert.IsTrue(a == b);
        }

        [Test]
        public void EqualsOperatorSameValueDifferentConstruction()
        {
            var a = new DATETIMETYPE(2019, 1, 1);
            var b = new DATETIMETYPE(new DateTime(2019, 1, 1));

            Assert.IsTrue(a == b);
        }

        [Test]
        public void EqualsOperatorBothNull()
        {
            DATETIMETYPE a = null;
            DATETIMETYPE b = null;

            Assert.IsTrue(a == b);
        }

        [Test]
        public void EqualsOperatorLeftNull()
        {
            DATETIMETYPE a = null;
            DATETIMETYPE b = new DATETIMETYPE(2019, 1, 1);

            Assert.IsFalse(a == b);
        }

        [Test]
        public void EqualsOperatorRightNull()
        {
            DATETIMETYPE a = new DATETIMETYPE(2019, 1, 1);
            DATETIMETYPE b = null;

            Assert.IsFalse(a == b);
        }

        [Test]
        public void EqualsSameInstantDifferentOffsets()
        {
            // Even though these are two different offsets, they represent the same moment in time and both have offsets supplied, so should be equal

            var a = DATETIMETYPE.FromUncorrectedDate(new DateTimeOffset(2019, 1, 1, 12, 0, 0, TimeSpan.FromHours(-1)));
            var b = DATETIMETYPE.FromUncorrectedDate(new DateTimeOffset(2019, 1, 1, 11, 0, 0, TimeSpan.FromHours(-2)));

            Assert.AreEqual(0, a.CompareTo(b));
            Assert.IsTrue(a.Equals(b));
        }

        [Test]
        public void EqualsOperatorSameInstantDifferentOffsets()
        {
            // Even though these are two different offsets, they represent the same moment in time and both have offsets supplied, so should be equal

            var a = DATETIMETYPE.FromUncorrectedDate(new DateTimeOffset(2019, 1, 1, 12, 0, 0, TimeSpan.FromHours(-1)));
            var b = DATETIMETYPE.FromUncorrectedDate(new DateTimeOffset(2019, 1, 1, 11, 0, 0, TimeSpan.FromHours(-2)));

            Assert.AreEqual(0, a.CompareTo(b));
            Assert.IsTrue(a == b);
        }

        [Test]
        public void EqualsOperatorSameTimeOneMissingOffset()
        {
            // While these will compare the same, they should not be considered equal

            var a = DATETIMETYPE.FromUncorrectedDate(new DateTimeOffset(2019, 1, 1, 8, 0, 0, 0, TimeSpan.Zero));
            var b = new DATETIMETYPE(new DateTime(2019, 1, 1, 8, 0, 0, 0, DateTimeKind.Unspecified));

            Assert.AreEqual(0, a.CompareTo(b));
            Assert.IsFalse(a == b);
        }

        private static DATETIMETYPE[] ToDateTimeIsUtcForZeroOffsetsInput()
        {
            return new[]
            {
                DATETIMETYPE.Parse("2019-02-06T17:24:00Z"),
                DATETIMETYPE.Parse("2019-02-06T17:24:00+00:00"),
                new DATETIMETYPE(new DateTime(2019, 1, 1, 8, 0, 0, 0, DateTimeKind.Utc)),
                DATETIMETYPE.FromUncorrectedDate(new DateTimeOffset(2019, 2, 6, 17, 24, 0, TimeSpan.Zero)),
            };
        }

        [Test, TestCaseSource(nameof(ToDateTimeIsUtcForZeroOffsetsInput))]
        public void ToDateTimeIsUtcForZeroOffsets(DATETIMETYPE input)
        {
            Assert.AreEqual(DateTimeKind.Utc, input.ToDateTime().Kind);
        }


        private static DATETIMETYPE[] ToDateTimeIsUnspecifiedForNonZeroAndEmptyOffsetsInputs()
        {
            return new[]
            {
                DATETIMETYPE.Parse("2019-02-06T17:24:00"),
                DATETIMETYPE.Parse("2019-02-07T17:24:00-08:00"),
                new DATETIMETYPE(new DateTime(2019, 1, 1, 8, 0, 0, 0, DateTimeKind.Unspecified)),
                DATETIMETYPE.FromUncorrectedDate(new DateTimeOffset(2019, 2, 9, 17, 24, 0, TimeSpan.FromHours(-8)))
            };
        }

        [Test, TestCaseSource(nameof(ToDateTimeIsUnspecifiedForNonZeroAndEmptyOffsetsInputs))]
        public void ToDateTimeIsUnspecifiedForNonZeroAndEmptyOffsets(DATETIMETYPE input)
        {
            Assert.AreEqual(DateTimeKind.Unspecified, input.ToDateTime().Kind);
        }

        [Test]
        public void ToDateTimeIsLocalKindWhenConstructedFromLocalDateTime()
        {
            // This situation only covers when a consumer of this library is converting back to DateTime after initializing from DateTime
            // No QuickBooks parsed DATETIMETYPE will ever have a Local kind

            var dt = new DATETIMETYPE(new DateTime(2019, 1, 1, 8, 0, 0, 0, DateTimeKind.Local));
            Assert.AreEqual(DateTimeKind.Local, dt.ToDateTime().Kind);
        }

        [Test]
        public void ToDateTimeEqualsInputWhenConstructedFromLocalDateTime()
        {
            var input = new DateTime(2018, 8, 1, 0, 0, 0, DateTimeKind.Local);
            var dt = new DATETIMETYPE(input);
            var output = dt.ToDateTime();

            Assert.AreEqual(input, output);
        }

        [Test]
        public void OffsetMatchesLocalMachineZoneWhenConstructedFromLocalDateTime()
        {
            var localDate = new DateTime(2019, 1, 1, 8, 0, 0, 0, DateTimeKind.Local);
            var zone = TimeZoneInfo.Local;
            var offset = zone.GetUtcOffset(localDate);

            var dt = new DATETIMETYPE(new DateTime(2019, 1, 1, 8, 0, 0, 0, DateTimeKind.Local));

            Assert.AreEqual(offset, dt.UncorrectedDate.Value.Offset);
        }

        [Test]
        public void LessThanOperator()
        {
            var a = DATETIMETYPE.FromUncorrectedDate(new DateTimeOffset(2019, 1, 1, 8, 0, 0, TimeSpan.Zero));
            var b = DATETIMETYPE.FromUncorrectedDate(new DateTimeOffset(2019, 1, 1, 9, 0, 0, TimeSpan.Zero));

            Assert.IsTrue(a < b);
        }

        [Test]
        public void GreaterThanOperator()
        {
            var a = DATETIMETYPE.FromUncorrectedDate(new DateTimeOffset(2019, 1, 1, 8, 0, 0, TimeSpan.Zero));
            var b = DATETIMETYPE.FromUncorrectedDate(new DateTimeOffset(2019, 1, 1, 9, 0, 0, TimeSpan.Zero));

            Assert.IsTrue(b > a);
        }


        [Test]
        public void ThrowsWhenConstructedWithDateTimeAfterEpoch()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                var date = new DATETIMETYPE(new DateTime(2038, 2, 1, 0, 0, 0));
            });
        }

        [Test]
        public void ThrowsWhenConstructedWithDateTimeBefore1970()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                var date = new DATETIMETYPE(new DateTime(1969, 12, 31, 23, 59, 59));
            });
        }

        [Test]
        public void ThrowsWhenConstructedWithDateTimeOffsetAfterEpoch()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                var date = DATETIMETYPE.FromUncorrectedDate(new DateTimeOffset(2038, 2, 1, 0, 0, 0, TimeSpan.Zero));
            });
        }

        [Test]
        public void ThrowsWhenConstructedWithDateTimeOffsetBefore1970()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                var date = DATETIMETYPE.FromUncorrectedDate(new DateTimeOffset(1969, 12, 31, 23, 59, 59, TimeSpan.Zero));
            });
        }

        [Test]
        public void ThrowsWhenConstructedWithComponentsOneSecondLaterThanEpoch()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                var date = DATETIMETYPE.FromUncorrectedDate(new DateTimeOffset(2038, 1, 19, 3, 14, 8, TimeSpan.Zero));
            });
        }

        [Test]
        public void ThrowsWhenConstructedWithYearBefore1970()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                var date = new DATETIMETYPE(1969, 12, 31, 23, 59, 59);
            });
        }

        [Test]
        public void DoesNotThrowWhenConstructedWithLastDayOf2037()
        {
            Assert.DoesNotThrow(() =>
            {
                var date = new DATETIMETYPE(2037, 12, 31, 23, 59, 59);
            });
        }

        [Test]
        public void DoesNotThrowWhenConstructedWithFirstDayOf1970()
        {
            Assert.DoesNotThrow(() =>
            {
                var date = new DATETIMETYPE(1970, 01, 01, 0, 0, 0);
            });
        }

        [Test]
        public void DoesNotThrowWhenConstructedWithFirstDayOf1970AndOffset()
        {
            Assert.DoesNotThrow(() =>
            {
                var date = DATETIMETYPE.FromUncorrectedDate(new DateTimeOffset(1970, 01, 01, 0, 0, 0, TimeSpan.Zero));
            });
        }

        [Test]
        public void DoesNotThrowWhenConstructedWithDateTimeOffsetOnEpoch()
        {
            Assert.DoesNotThrow(() =>
            {
                var date = DATETIMETYPE.FromUncorrectedDate(new DateTimeOffset(2038, 1, 19, 3, 14, 7, 0, TimeSpan.Zero));
            });
        }

        [Test]
        public void ThrowsWhenParsedFromStringAfterQbEpoch()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                var dt = DATETIMETYPE.Parse("2038-01-19T03:14:08+00:00");
            });
        }

        [Test]
        public void DoesNotThrowWhenParsedFromStringOnQbEpoch()
        {
            Assert.DoesNotThrow(() =>
            {
                var dt = DATETIMETYPE.Parse("2038-01-19T03:14:07+00:00");
            });
        }

        [TestCase("0000-00-00")]
        [TestCase("0000-00-00T00:00:00")]
        [TestCase("")]
        public void ThrowsFormatExceptionWhenInvalidStringIsParsed(string dateString)
        {
            Assert.Throws<FormatException>(() =>
            {
                var dt = DATETIMETYPE.Parse(dateString);
            });
        }

        [Test]
        public void ThrowsArgumentNullExceptionWhenNullStringIsParsed()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                var dt = DATETIMETYPE.Parse(null);
            });
        }



        #region QuickBooks parsing and fixes

        private static CustomerRet CreateAndParseCustomerQueryXml(string timeCreated, string timeModified)
        {
            var ret = $"<CustomerRet><ListID>80000001-1422671082</ListID><TimeCreated>{timeCreated}</TimeCreated><TimeModified>{timeModified}</TimeModified><EditSequence>1422671082</EditSequence><Name>Chris Curtis</Name><FullName>Christopher Curtis</FullName><IsActive>true</IsActive></CustomerRet>";

            var response = new QbXmlResponse();
            var rs = response.GetSingleItemFromResponse<CustomerQueryRsType>(QuickBooksTestHelper.CreateQbXmlWithEnvelope(ret, "CustomerQueryRs"));
            return rs.CustomerRet[0];
        }


        [Test]
        public void OffsetIsIgnoreWhenXmlParsed()
        {
            // This tests the QuickBooks fix that simply ignores the returned offset portion of the date [when no fix applied]

            var time = "2015-04-03T10:06:17-07:00";

            var customer = CreateAndParseCustomerQueryXml(time, time);

            Assert.AreEqual("2015-04-03T10:06:17", customer.TimeModified.ToString());
        }

        [Test]
        public void IncorrectOffsetReturnedFromQuickBooksDoesNotAlterParsedTime()
        {
            var incorrectOffset = "2015-04-03T10:06:17-08:00";
            var correctOffset = "2015-04-03T10:06:17-07:00";

            var customer = CreateAndParseCustomerQueryXml(timeCreated: incorrectOffset, timeModified: correctOffset);

            Assert.AreEqual("2015-04-03T10:06:17", customer.TimeCreated.ToString());
            Assert.AreEqual("2015-04-03T10:06:17", customer.TimeModified.ToString());
        }

        [Test]
        public void DateTimeOnXmlParsedDateIsUnspecifiedKind()
        {
            var time = "2015-04-03T10:06:17-08:00";

            var customer = CreateAndParseCustomerQueryXml(time, time);

            Assert.AreEqual(DateTimeKind.Unspecified, customer.TimeModified.ToDateTime().Kind);
        }

        [Test]
        public void MinValueUsedWhenXmlParsedWithInvalidDate()
        {
            // Dates have been seen with 0000-00-00. Need to use default date instead of throwing

            var time = "0000-00-00";

            var customer = CreateAndParseCustomerQueryXml(time, time);

            Assert.AreEqual("1970-01-01T00:00:00", customer.TimeModified.ToString());
        }

        [Test]
        public void MinValueUsedWhenXmlParsedWithEmptyDate()
        {
            var time = "";

            var customer = CreateAndParseCustomerQueryXml(time, time);

            // TODO: Consider if Min Value should actually be used in this situation. Return empty string instead?
            Assert.AreEqual("1970-01-01T00:00:00", customer.TimeModified.ToString());
        }


        #endregion
    }
}
