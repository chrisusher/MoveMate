using ChrisUsher.MoveMate.API.Services.Savings;
using ChrisUsher.MoveMate.Shared.DTOs.Savings;
using ChrisUsher.MoveMate.Shared.DTOs.Savings.Stocks;

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

        Assert.That(stockAccount, Is.Not.Null, "Stock Account was not created");
        Assert.That(stockAccount.StockId, Is.Not.EqualTo(Guid.Empty), "Stock Id was not created");
    }

    [Test]
    public async Task CreateStockAsync_CanCreateMultipleInSameAccount_DoesntError()
    {
        var stockAccount = await _stockService.CreateStockAsync(_savingsAccount.AccountId, _savingsAccount.SavingsId, new CreateStocksAndSharesRequest
        {
            StockName = ServiceTestsCommon.Faker.Company.CompanyName(),
            StartDate = new(2020, 1, 1),
        });

        Assert.That(stockAccount, Is.Not.Null, "Stock Account was not created");
        Assert.That(stockAccount.StockId, Is.Not.EqualTo(Guid.Empty), "Stock Id was not created");

        stockAccount = await _stockService.CreateStockAsync(_savingsAccount.AccountId, _savingsAccount.SavingsId, new CreateStocksAndSharesRequest
        {
            StockName = ServiceTestsCommon.Faker.Company.CompanyName(),
            StartDate = new(2020, 1, 1),
        });

        Assert.That(stockAccount, Is.Not.Null, "Stock Account was not created");
        Assert.That(stockAccount.StockId, Is.Not.EqualTo(Guid.Empty), "Stock Id was not created");
    }

    [Test]
    public async Task GetStocksAsync_ReturnsStocks()
    {
        var stocks = await _stockService.GetStocksAsync(_savingsAccount.SavingsId);

        Assert.That(stocks, Is.Not.Null, "Stocks were not returned");
        Assert.That(stocks, Is.Not.Empty, "Stocks were not returned");
    }

    [Test]
    public async Task GetStockAsync_ReturnsStock()
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

        var stockId = stock.StockId;
        stock = await _stockService.GetStockAsync(_savingsAccount.SavingsId, stockId);

        Assert.That(stock, Is.Not.Null, "Stock was not returned");
        Assert.That(stock.StockId, Is.EqualTo(stockId), "Stock was not returned");
    }

    [Test]
    public async Task UpdateStockAsync_ReturnsStock()
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
        
        var newName = "Nvidia";
        stock = await _stockService.UpdateStockAsync(_savingsAccount.AccountId, _savingsAccount.SavingsId, stock.StockId, new UpdateStocksAndSharesRequest
        {
            StockName = newName,
            StartDate = new(2021, 1, 1),
        });

        Assert.That(stock, Is.Not.Null, "Stock was not returned");
        Assert.That(stock.StockName, Is.EqualTo(newName), "Stock was not returned");
    }
}
