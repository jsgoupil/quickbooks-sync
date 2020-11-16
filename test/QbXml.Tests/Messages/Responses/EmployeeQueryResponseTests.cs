using NUnit.Framework;
using QbSync.QbXml.Objects;
using QbSync.QbXml.Tests.Helpers;
using System;
using System.Xml.Serialization;

namespace QbSync.QbXml.Tests.QbXml
{
    [TestFixture]
    class EmployeeQueryResponseTests
    {
        [Test]
        public void EmployeeResponseWithUnknownTagsTest()
        {
			var employeeRet = @"
<EmployeeRet>
    <ListID>1111111-111111111</ListID>
    <EditSequence>1234567890</EditSequence>
    <Name>Daric Teske</Name>
    <IsActive>true</IsActive>
    <FirstName>Daric</FirstName>
    <LastName>Teske</LastName>
    <Phone>5555555555</Phone>
    <Email>tesked@outlook.com</Email>
    <EmployeePayrollInfo>
        <IsCoveredByQualifiedPensionPlan>false</IsCoveredByQualifiedPensionPlan>
        <PayPeriod>Weekly</PayPeriod>
        <PayScheduleRef>
            <ListID>80000001-1286898172</ListID>
            <FullName>Weekly</FullName>
        </PayScheduleRef>
        <Earnings>
            <PayrollItemWageRef>
                <ListID>80000000-0000000001</ListID>
                <FullName>Field</FullName>
            </PayrollItemWageRef>
            <Rate>15.00</Rate>
        </Earnings>
        <Earnings>
            <PayrollItemWageRef>
                <ListID>80000000-0000000002</ListID>
                <FullName>Field Overtime</FullName>
            </PayrollItemWageRef>
            <Rate>25.00</Rate>
        </Earnings>
        <Earnings>
            <PayrollItemWageRef>
                <ListID>80000000-0000000003</ListID>
                <FullName>Field Double Time</FullName>
            </PayrollItemWageRef>
            <Rate>50.00</Rate>
        </Earnings>
        <Earnings>
            <PayrollItemWageRef>
                <ListID>80000000-0000000004</ListID>
                <FullName>Field Holiday</FullName>
            </PayrollItemWageRef>
            <Rate>30.00</Rate>
        </Earnings>
        <UseTimeDataToCreatePaychecks>UseTimeData</UseTimeDataToCreatePaychecks>
        <SickHours>
            <HoursAvailable>PT0H15M0S</HoursAvailable>
            <AccrualPeriod>EveryPaycheck</AccrualPeriod>
            <HoursAccrued>PT0H55M0S</HoursAccrued>
            <MaximumHours>PT48H0M0S</MaximumHours>
            <IsResettingHoursEachNewYear>false</IsResettingHoursEachNewYear>
            <HoursUsed>PT0H0M0S</HoursUsed>
            <YearBeginsDate>2020-01-01</YearBeginsDate>
            <AccrualStartDate>2000-01-01</AccrualStartDate>
        </SickHours>
        <VacationHours>
            <HoursAvailable>PT0H0M0S</HoursAvailable>
            <AccrualPeriod>EveryPaycheck</AccrualPeriod>
            <HoursAccrued>PT0H0M0S</HoursAccrued>
            <MaximumHours>PT0H0M0S</MaximumHours>
            <IsResettingHoursEachNewYear>false</IsResettingHoursEachNewYear>
            <HoursUsed>PT0H0M0S</HoursUsed>
            <YearBeginsDate>2020-01-01</YearBeginsDate>
            <AccrualStartDate>2000-01-01</AccrualStartDate>
        </VacationHours>
        <EmployeeTaxInfo>
            <StateLived>NE</StateLived>
            <StateWorked>NE</StateWorked>
            <IsStandardTaxationRequired>true</IsStandardTaxationRequired>
            <EmployeeTax>
                <IsSubjectToTax>true</IsSubjectToTax>
                <PayrollItemTaxRef>
                    <ListID>D0000-863012697</ListID>
                    <FullName>Federal Withholding</FullName>
                </PayrollItemTaxRef>
                <TaxInfo>
                    <TaxInfoCategory>ALLOWANCES</TaxInfoCategory>
                    <TaxInfoValue>1</TaxInfoValue>
                </TaxInfo>
                <TaxInfo>
                    <TaxInfoCategory>EXTRA_WITHHOLDING</TaxInfoCategory>
                    <TaxInfoValue>0.00</TaxInfoValue>
                </TaxInfo>
                <TaxInfo>
                    <TaxInfoCategory>FILING_STATUS</TaxInfoCategory>
                    <TaxInfoValue>256</TaxInfoValue>
                </TaxInfo>
            </EmployeeTax>
        </EmployeeTaxInfo>
        <EmployeeDirectDepositAccount>
            <BankName>Fake Bank</BankName>
            <RoutingNumber>111111111</RoutingNumber>
            <AccountNumber>111111111111</AccountNumber>
            <BankAccountType>Checking</BankAccountType>
        </EmployeeDirectDepositAccount>
    </EmployeePayrollInfo>
</EmployeeRet>";
            var response = new QbXmlResponse();
            var onUnknownElementCalled = 0;
            XmlDeserializationEvents events = new XmlDeserializationEvents();
            events.OnUnknownElement += (object sender, XmlElementEventArgs e) =>
            {
                onUnknownElementCalled++;
                if (e.Element.Name == nameof(EmployeePayrollInfo.UseTimeDataToCreatePaychecks) && Enum.TryParse(typeof(UseTimeDataToCreatePaychecks), e.Element.InnerText, out object useTimeData))
                {
                    var employeePayrollInfo = (EmployeePayrollInfo)e.ObjectBeingDeserialized;
                    employeePayrollInfo.UseTimeDataToCreatePaychecksSpecified = true;
                    employeePayrollInfo.UseTimeDataToCreatePaychecks = (UseTimeDataToCreatePaychecks)useTimeData;
                }
            };
            var rs = response.GetSingleItemFromResponse<EmployeeQueryRsType>(QuickBooksTestHelper.CreateQbXmlWithEnvelope(employeeRet, "EmployeeQueryRs"), events);

            var employees = rs.EmployeeRet;
            var employee = employees[0];

            Assert.AreEqual(1, employees.Length);
            Assert.AreEqual("1111111-111111111", employee.ListID);

            // Custom event handler was called at least once
            Assert.GreaterOrEqual(onUnknownElementCalled, 1);

            // Custom event handler updated a known property captured by the OnUnknownElement event
            Assert.IsTrue(employee.EmployeePayrollInfo.UseTimeDataToCreatePaychecksSpecified);
            Assert.AreEqual(UseTimeDataToCreatePaychecks.UseTimeData, employee.EmployeePayrollInfo.UseTimeDataToCreatePaychecks);
        }
    }
}
