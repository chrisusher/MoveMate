using ChrisUsher.MoveMate.API.Services.Savings;
using ChrisUsher.MoveMate.Shared.DTOs.Savings;

namespace Services.Tests.Savings;

[TestFixture]
public class StockServiceTests
{
    private readonly SavingsService _savingsService;
    private readonly StockService _stockService;
    private SavingsAccount _savingsAccount;

    public StockServiceTests()
    {
        _savingsService = ServiceTestsCommon.Services.GetService<SavingsService>();
        _stockService = ServiceTestsCommon.Services.GetService<StockService>();
    }

    [OneTimeSetUp]
    public async Task ClassSetupAsync()
    {
        _savingsAccount = await _savingsService.CreateSavingsAccountAsync(ServiceTestsCommon.DefaultAccount.AccountId, new CreateSavingsAccountRequest
        {
            Name = "Test Account",
            InitialBalance = 10000,
            SavingType = SavingType.StocksAndShares
        });
    }

    [Test]
    public async Task CreateStocksAccountAsync_ReturnsAccount()
    {
        var stockAccount = await _stockService.CreateStockAsync(_savingsAccount.AccountId, new()
        {
            StockName = "Microsoft",
            SavingsId = _savingsAccount.SavingsId,
            StartDate = new(2020, 1, 1),
        });

        Assert.That(stockAccount, Is.Not.Null, "Stock Account was not created");
        Assert.That(stockAccount.StockId, Is.Not.EqualTo(Guid.Empty), "Stock Id was not created");
    }
}
