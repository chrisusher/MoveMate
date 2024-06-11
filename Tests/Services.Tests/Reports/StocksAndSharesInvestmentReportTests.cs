using ChrisUsher.MoveMate.API.Services.Reports;
using ChrisUsher.MoveMate.API.Services.Savings;
using ChrisUsher.MoveMate.Shared.DTOs.Reports;
using ChrisUsher.MoveMate.Shared.DTOs.Savings.Stocks;
using ChrisUsher.MoveMate.Shared.Exceptions;
using Services.Tests.Savings;

namespace Services.Tests.Reports;

[TestFixture]
public class StocksAndSharesInvestmentReportTests : SavingsTestsBase
{
    private readonly ReportsService _reportService;
    private readonly StockService _stocksService;
    private StockInvestmentReport _report;
    private StocksAndSharesDetails _positiveReturnStock;
    private StocksAndSharesDetails _stockWithBalance3Dps;

    public StocksAndSharesInvestmentReportTests()
    {
        _reportService = ServiceTestsCommon.Services.GetService<ReportsService>();
        _stocksService = ServiceTestsCommon.Services.GetService<StockService>();
    }

    [OneTimeSetUp]
    public async Task ClassSetupAsync()
    {
        var stocksAndSharesAccount = await SavingsService.CreateSavingsAccountAsync(ServiceTestsCommon.DefaultAccount.AccountId, new()
        {
            Name = "Test Account",
            InitialBalance = 10_000,
            SavingsRate = 5,
            SavingType = SavingType.StocksAndShares,
            MonthlySavingsAmount = 100
        });

        _positiveReturnStock = await _stocksService.CreateStockAsync(ServiceTestsCommon.DefaultAccount.AccountId, stocksAndSharesAccount.SavingsId, new()
        {
            StockName = "Test Stock",
            IsActive = true,
            StartDate = new(2020, 1, 1),
            MonthlySavingsAmount = 0
        });

        _stockWithBalance3Dps = await _stocksService.CreateStockAsync(ServiceTestsCommon.DefaultAccount.AccountId, stocksAndSharesAccount.SavingsId, new()
        {
            StockName = "Stock With Balance to 3 DPs",
            IsActive = true,
            StartDate = new(2020, 1, 1),
            MonthlySavingsAmount = 0
        });

        await _stocksService.AddBalanceToStockAsync(stocksAndSharesAccount.SavingsId, _positiveReturnStock.StockId, 1500, 1200);
        
        await _stocksService.AddBalanceToStockAsync(stocksAndSharesAccount.SavingsId, _stockWithBalance3Dps.StockId, 1500.123, 1200.567);

        _report = await _reportService.GetStockInvestmentReportAsync(ServiceTestsCommon.DefaultAccount.AccountId, stocksAndSharesAccount.SavingsId);
    }

    [Test]
    public void GetStockInvestmentReportAsync_ReturnsStocks()
    {
        Assert.That(_report.Stocks, Is.Not.Null, "Report Stocks was null.");
        Assert.That(_report.Stocks, Is.Not.Empty, "Report Stocks was empty.");
    }

    [Test]
    public void GetStockInvestmentReportAsync_PositiveReturnStock_HasBalance()
    {
        var stockFromReport = _report.Stocks.FirstOrDefault(x => x.StockId == _positiveReturnStock.StockId);

        Assert.That(stockFromReport, Is.Not.Null, "Positive Return Stock was null.");

        Assert.That(stockFromReport.AmountInvested, Is.EqualTo(1200), "Positive Return Stock Balance was not correct.");
        Assert.That(stockFromReport.CurrentValue, Is.EqualTo(1500), "Positive Return Stock Current Value was not correct.");
    }

    [Test]
    public void GetStockInvestmentReportAsync_PositiveReturnStock_AmountChangeCorrect()
    {
        var stockFromReport = _report.Stocks.FirstOrDefault(x => x.StockId == _positiveReturnStock.StockId);

        Assert.That(stockFromReport.AmountChange, Is.EqualTo(300), "Positive Return Stock Percentage Change was not correct.");
    }

    [Test]
    public void GetStockInvestmentReportAsync_PositiveReturnStock_PercentageCorrect()
    {
        var stockFromReport = _report.Stocks.FirstOrDefault(x => x.StockId == _positiveReturnStock.StockId);

        Assert.That(stockFromReport, Is.Not.Null, "Positive Return Stock was null.");

        Assert.That(stockFromReport.PercentageChange, Is.EqualTo(0.25), "Positive Return Stock Percentage Change was not correct.");
    }

    [Test]
    public void GetStockInvestmentReportAsync_AmountChange_RoundedTo2DPs()
    {
        Assert.That(_report.Stocks.All(x => x.AmountChange == Math.Round(x.AmountChange, 2)), "Amount Change was not rounded to 2 DPs.");
    }

    [Test]
    public void GetStockInvestmentReportAsync_PercentageChange_RoundedTo5DPs()
    {
        Assert.That(_report.Stocks.All(x => x.PercentageChange == Math.Round(x.PercentageChange, 5)), "Amount Change was not rounded to 5 DPs.");
    }

    [Test]
    public void GetStockInvestmentReportAsync_AccountDoesntExist_ThrowsDataNotFoundException()
    {
        Assert.ThrowsAsync<DataNotFoundException>(() => _reportService.GetStockInvestmentReportAsync(Guid.NewGuid(), Guid.NewGuid()), "No exception was thrown when Account was not found.");
    }

    [Test]
    public void GetStockInvestmentReportAsync_SavingsDoesntExist_ThrowsDataNotFoundException()
    {
        Assert.ThrowsAsync<DataNotFoundException>(() => _reportService.GetStockInvestmentReportAsync(ServiceTestsCommon.DefaultAccount.AccountId, Guid.NewGuid()), "No exception was thrown when Savings was not found.");
    }

    [Test]
    public async Task GetStockInvestmentReportAsync_NotStocksAndSharesSavings_ThrowsDataNotFoundException()
    {
        var savingAccount = await CreateSavingsAccount(SavingType.CurrentAccount);

        Assert.ThrowsAsync<InvalidRequestException>(() => _reportService.GetStockInvestmentReportAsync(ServiceTestsCommon.DefaultAccount.AccountId, savingAccount.SavingsId), "No exception was thrown when Saving Id was associated with a non Stocks and Shares Savings Account.");
    }
}
