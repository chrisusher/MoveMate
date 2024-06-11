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
            MonthlySavingsAmount = 100
        });

        var updateRequest = (UpdateStocksAndSharesRequest)_positiveReturnStock;
        updateRequest.Balances.Add(new()
        {
            AmountInvested = 1200,
            Created = new(2021, 1, 1),
            Balance = 1500
        });

        await _stocksService.UpdateStockAsync(ServiceTestsCommon.DefaultAccount.AccountId, stocksAndSharesAccount.SavingsId, _positiveReturnStock.StockId, updateRequest);

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
    public void GetStockInvestmentReportAsync_PositiveReturnStock_PercentageCorrect()
    {
        var stockFromReport = _report.Stocks.FirstOrDefault(x => x.StockId == _positiveReturnStock.StockId);

        Assert.That(stockFromReport, Is.Not.Null, "Positive Return Stock was null.");

        Assert.That(stockFromReport.PercentageChange, Is.EqualTo(0.25), "Positive Return Stock Percentage Change was not correct.");
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
