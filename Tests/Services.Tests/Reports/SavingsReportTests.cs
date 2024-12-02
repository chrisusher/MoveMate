using ChrisUsher.MoveMate.API.Services;
using ChrisUsher.MoveMate.API.Services.Reports;
using ChrisUsher.MoveMate.API.Services.Savings;
using ChrisUsher.MoveMate.Shared;
using ChrisUsher.MoveMate.Shared.DTOs;
using ChrisUsher.MoveMate.Shared.DTOs.Savings;

namespace Services.Tests.Reports;

[TestFixture]
public class SavingsReportTests
{
    private readonly ReportsService _reportService;
    private readonly SavingsService _savingsService;

    public SavingsReportTests()
    {
        _reportService = ServiceTestsCommon.Services.GetService<ReportsService>();
        _savingsService = ServiceTestsCommon.Services.GetService<SavingsService>();
    }

    [OneTimeSetUp]
    public async Task ClassSetupAsync()
    {
        await _savingsService.CreateSavingsAccountAsync(ServiceTestsCommon.DefaultAccount.AccountId, new CreateSavingsAccountRequest
        {
            Name = "Test Account",
            InitialBalance = 10_000,
            SavingsRate = 5,
            SavingType = SavingType.ISA,
            MonthlySavingsAmount = 100
        });

        await _savingsService.CreateSavingsAccountAsync(ServiceTestsCommon.DefaultAccount.AccountId, new CreateSavingsAccountRequest
        {
            Name = "Stocks and Shares Account",
            InitialBalance = 10_000,
            SavingsRate = 0,
            SavingType = SavingType.StocksAndShares,
            MonthlySavingsAmount = 0,
            Fluctuations = new Fluctuation 
            {
                BestCase = 11_500,
                WorstCase = 9_000
            }
        });
    }

    [TestCase(CaseType.WorstCase)]
    [TestCase(CaseType.MiddleCase)]
    [Test]
    public async Task GetSavingReportAsync_ReturnsReport(CaseType caseType)
    {
        var report = await _reportService.GetSavingReportAsync(ServiceTestsCommon.DefaultAccount.AccountId, caseType);

        Assert.That(report.TotalSavings, Is.GreaterThan(0), "Report Total Savings was not greater than 0.");
    }

    [Test]
    public async Task GetSavingReportAsync_TotalSavingsRounded()
    {
        var report = await _reportService.GetSavingReportAsync(ServiceTestsCommon.DefaultAccount.AccountId, CaseType.MiddleCase);

        var decimalPlaces = CurrencyLogic.CountDecimalPlaces(report.TotalSavings);

        Assert.That(decimalPlaces, Is.LessThanOrEqualTo(2), "Total Savings was not rounded to 2 decimal places.");
    }

    [TestCase(CaseType.WorstCase)]
    [TestCase(CaseType.MiddleCase)]
    [Test]
    public async Task GetSavingReportAsync_WithFutureDate_SavingsReturned(CaseType caseType)
    {
        var futureDate = DateTime.Now.AddMonths(6);

        var report = await _reportService.GetSavingReportAsync(ServiceTestsCommon.DefaultAccount.AccountId, caseType, futureDate);

        Assert.That(report.TotalSavings, Is.GreaterThan(0), "Report Total Savings was not greater than 0.");
        Assert.That(report.FutureDate, Is.EqualTo(futureDate), "Future Date was not correct.");
    }
}