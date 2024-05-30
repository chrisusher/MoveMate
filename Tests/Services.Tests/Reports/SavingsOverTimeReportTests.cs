using ChrisUsher.MoveMate.API.Services.Reports;
using ChrisUsher.MoveMate.API.Services.Savings;

namespace Services.Tests.Reports
{
    [TestFixture]
    public class SavingsOverTimeReportTests
    {
        private ReportsService ReportService => ServiceTestsCommon.Services.GetService<ReportsService>();
        private SavingsService SavingsService => ServiceTestsCommon.Services.GetService<SavingsService>();

        [OneTimeSetUp]
        public async Task ClassSetupAsync()
        {
            await SavingsService.CreateSavingsAccountAsync(ServiceTestsCommon.DefaultAccount.AccountId, new()
            {
               Name = "Test Account",
               SavingType = SavingType.ISA,
               InitialBalance = 10_000,
               MonthlySavingsAmount = 100,
               SavingsRate = 5
            });

            await ReportService.GetSavingReportAsync(ServiceTestsCommon.DefaultAccount.AccountId, CaseType.MiddleCase);
        }

        [Test]
        public async Task GetSavingsOverTimeReportAsync_ReturnsReport()
        {
            var report = await ReportService.GetSavingsOverTimeReportAsync(ServiceTestsCommon.DefaultAccount.AccountId);

            Assert.That(report.Summaries, Is.Not.Empty, "Saving Report Summaries did not return any entries.");
            Assert.That(report.Summaries[0].TotalSavings, Is.GreaterThan(0), "Report Total Savings was not greater than 0.");
        }
    }
}