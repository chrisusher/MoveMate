using Bogus;
using ChrisUsher.MoveMate.API.Services.Savings;
using ChrisUsher.MoveMate.Shared.DTOs.Savings;
using ChrisUsher.MoveMate.Shared.DTOs.Savings.Stocks;
using ChrisUsher.MoveMate.Shared.Exceptions;

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
    public async Task CreateStockAsync_ReturnsStock()
    {
        var stockAccount = await _stockService.CreateStockAsync(_savingsAccount.AccountId, _savingsAccount.SavingsId, new CreateStocksAndSharesRequest
        {
            StockName = "Microsoft",
            StartDate = new(2020, 1, 1),
        });

        Assert.That(stockAccount, Is.Not.Null, "Stock Account was not created.");
        Assert.That(stockAccount.StockId, Is.Not.EqualTo(Guid.Empty), "Stock Id was not created.");
    }

    [Test]
    public async Task CreateStockAsync_CanCreateMultipleInSameAccount_DoesntError()
    {
        var stockAccount = await _stockService.CreateStockAsync(_savingsAccount.AccountId, _savingsAccount.SavingsId, new CreateStocksAndSharesRequest
        {
            StockName = ServiceTestsCommon.Faker.Company.CompanyName(),
            StartDate = new(2020, 1, 1),
        });

        Assert.That(stockAccount, Is.Not.Null, "Stock Account was not created.");
        Assert.That(stockAccount.StockId, Is.Not.EqualTo(Guid.Empty), "Stock Id was not created.");

        stockAccount = await _stockService.CreateStockAsync(_savingsAccount.AccountId, _savingsAccount.SavingsId, new CreateStocksAndSharesRequest
        {
            StockName = ServiceTestsCommon.Faker.Company.CompanyName(),
            StartDate = new(2020, 1, 1),
        });

        Assert.That(stockAccount, Is.Not.Null, "Stock Account was not created.");
        Assert.That(stockAccount.StockId, Is.Not.EqualTo(Guid.Empty), "Stock Id was not created.");
    }

    [Test]
    public async Task AddBalanceToStockAsync_ReturnsStock()
    {
        var stock = await GetNewOrExistingStockAsync();

        var stockBalances = stock.Balances.Count;
        var currentBalance = 1000;

        var stockId = stock.StockId;
        var updatedStock = await _stockService.AddBalanceToStockAsync(_savingsAccount.SavingsId, stockId, currentBalance, 0);

        Assert.That(updatedStock, Is.Not.Null, "Stock was not returned.");
        Assert.That(updatedStock.StockId, Is.EqualTo(stock.StockId), "Stock was not returned.");
        Assert.That(updatedStock.Balances.Count, Is.EqualTo(stockBalances + 1), "Balance was not added.");
    }

    [TestCase(0)]
    [TestCase(-1)]
    [Test]
    public async Task AddBalanceToStockAsync_HasNoExistingBalance_AdditionalInvestment_CantBe0OrLess(double additionalInvestment)
    {
        var stock = await _stockService.CreateStockAsync(ServiceTestsCommon.DefaultAccount.AccountId, _savingsAccount.SavingsId, new()
        {
            StockName = ServiceTestsCommon.Faker.Company.CompanyName(),
            StartDate = new(2020, 1, 1),
        });

        Assert.ThrowsAsync<InvalidRequestException>(() => _stockService.AddBalanceToStockAsync(_savingsAccount.SavingsId, stock.StockId, 1000, additionalInvestment: additionalInvestment), "Exception was not thrown if trying to add 0 as additional investment when there is no existing balance.");
    }

    [Test]
    public async Task AddBalanceToStockAsync_HasNoExistingBalance_CreatesNewBalance()
    {
        var stock = await _stockService.CreateStockAsync(ServiceTestsCommon.DefaultAccount.AccountId, _savingsAccount.SavingsId, new()
        {
            StockName = ServiceTestsCommon.Faker.Company.CompanyName(),
            StartDate = new(2020, 1, 1),
        });

        var updatedStock = await _stockService.AddBalanceToStockAsync(_savingsAccount.SavingsId, stock.StockId, 1000, 1000);

        Assert.That(updatedStock, Is.Not.Null, "Stock was not returned.");
        Assert.That(updatedStock.StockId, Is.EqualTo(stock.StockId), "Stock was not returned.");
        Assert.That(updatedStock.Balances.Count, Is.EqualTo(1), "Balance was not added.");

        var firstBalance = updatedStock.Balances.First();

        Assert.That(firstBalance.AmountInvested, Is.EqualTo(1000), "Amount Invested was not added.");
        Assert.That(firstBalance.Balance, Is.EqualTo(1000), "Balance was not added.");
        Assert.That(firstBalance.Created, Is.Not.EqualTo(default), "Created Date was not added to new Balance.");
    }

    [TestCase(0)]
    [TestCase(100)]
    [Test]
    public async Task AddBalanceToStockAsync_HasExistingBalance_AdditionalInvestment_AddedToExistingBalance(double additionalInvestment)
    {
        var stock = await _stockService.CreateStockAsync(ServiceTestsCommon.DefaultAccount.AccountId, _savingsAccount.SavingsId, new()
        {
            StockName = ServiceTestsCommon.Faker.Company.CompanyName(),
            StartDate = new(2020, 1, 1),
        });

        await _stockService.AddBalanceToStockAsync(_savingsAccount.SavingsId, stock.StockId, 1000, 1000);

        var currentBalance = 1250;

        var stockId = stock.StockId;
        var updatedStock = await _stockService.AddBalanceToStockAsync(_savingsAccount.SavingsId, stockId, currentBalance, additionalInvestment);

        Assert.That(stock, Is.Not.Null, "Stock was not returned");
        Assert.That(updatedStock.StockId, Is.EqualTo(stock.StockId), "Stock was not returned");
        Assert.That(updatedStock.Balances.Count, Is.EqualTo(2), "Balance was not added when existing balance was present");
        Assert.That(updatedStock.Balances.Last().AmountInvested, Is.EqualTo(additionalInvestment + 1000), "Balance was not added when existing balance was present");
    }

    [Test]
    public void AddBalanceToStockAsync_StockDoesntExist_ThrowsDataNotFoundException()
    {
        Assert.ThrowsAsync<DataNotFoundException>(() => _stockService.AddBalanceToStockAsync(_savingsAccount.SavingsId, Guid.NewGuid(), 1000, 10), "No exception was thrown when Stock was not found.");
    }

    [Test]
    public async Task GetStocksAsync_ReturnsStocks()
    {
        var stocks = await _stockService.GetStocksAsync(_savingsAccount.SavingsId);

        Assert.That(stocks, Is.Not.Null, "Stocks were not returned.");
        Assert.That(stocks, Is.Not.Empty, "Stocks were not returned.");
    }

    [Test]
    public async Task GetStockAsync_ReturnsStock()
    {
        StocksAndSharesDetails stock = await GetNewOrExistingStockAsync();

        var stockId = stock.StockId;
        stock = await _stockService.GetStockAsync(_savingsAccount.SavingsId, stockId);

        Assert.That(stock, Is.Not.Null, "Stock was not returned");
        Assert.That(stock.StockId, Is.EqualTo(stockId), "Stock was not returned");
    }

    [Test]
    public void GetStockAsync_NotFound_ThrowsDataNotFoundException()
    {
        Assert.ThrowsAsync<DataNotFoundException>(() => _stockService.GetStockAsync(_savingsAccount.SavingsId, Guid.NewGuid()), "No exception was thrown when Stock was not found.");
    }

    [Test]
    public async Task UpdateStockAsync_ReturnsStock()
    {
        var stock = await GetNewOrExistingStockAsync();

        var newName = "Nvidia";
        stock = await _stockService.UpdateStockAsync(_savingsAccount.AccountId, _savingsAccount.SavingsId, stock.StockId, new UpdateStocksAndSharesRequest
        {
            StockName = newName,
            StartDate = new(2021, 1, 1),
        });

        Assert.That(stock, Is.Not.Null, "Stock was not returned.");
        Assert.That(stock.StockName, Is.EqualTo(newName), "Stock was not returned.");
    }

    #region Private Methods

    private async Task<StocksAndSharesDetails> GetNewOrExistingStockAsync()
    {
        var stocks = await _stockService.GetStocksAsync(_savingsAccount.SavingsId);

        var stock = stocks.FirstOrDefault();

        if (stock == null)
        {
            stock = await _stockService.CreateStockAsync(_savingsAccount.AccountId, _savingsAccount.SavingsId, new CreateStocksAndSharesRequest
            {
                StockName = "Microsoft",
                StartDate = new(2020, 1, 1),
            });
        }

        return stock;
    }

    #endregion
}
