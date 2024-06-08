using ChrisUsher.MoveMate.API.Repositories;
using ChrisUsher.MoveMate.Shared.DTOs.Savings.Stocks;
using ChrisUsher.MoveMate.Shared.Enums;

namespace ChrisUsher.MoveMate.API.Services.Savings;

public class StockService
{
    private readonly StockRepository _stockRepo;
    private readonly SavingsService _savingsService;

    public StockService(StockRepository stockRepository,
        SavingsService savingsService)
    {
        _stockRepo = stockRepository;
        _savingsService = savingsService;
    }

    public async Task<StocksAndSharesDetails> AddBalanceToStockAsync(Guid savingsId, Guid stockId, double currentBalance, double additionalInvestment)
    {
        var stock = await _stockRepo.GetStockAsync(savingsId, stockId);

        if (stock == null)
        {
            throw new DataNotFoundException($"No Stock or Share Details found in Stocks '{stockId}' with Savings Id '{savingsId}'");
        }

        var totalInvestment = additionalInvestment;

        if (stock.Balances.Any())
        {
            totalInvestment += stock.Balances.Last().AmountInvested;
        }
        else if(additionalInvestment <= 0)
        {
            throw new InvalidRequestException($"Cannot add a negative or zero amount to Stock '{stockId}' as the Stock has no existing investment.");
        }

        await _stockRepo.AddBalanceToStockAsync(savingsId, stockId, currentBalance, totalInvestment);

        return await GetStockAsync(savingsId, stockId);
    }

    public async Task<StocksAndSharesDetails> CreateStockAsync(Guid accountId, Guid savingsId, CreateStocksAndSharesRequest stockDetailsRequest)
    {
        var savingsAccount = await _savingsService.GetSavingsAccountAsync(accountId, savingsId);

        if (savingsAccount.SavingType != SavingType.StocksAndShares)
        {
            throw new InvalidOperationException($"Savings Account '{savingsAccount.SavingsId}' is not of type Stocks and Shares");
        }

        var stock = await _stockRepo.CreateStockAsync(savingsId, stockDetailsRequest);

        return stock.ToStocksAndSharesDetails();
    }

    public async Task<StocksAndSharesDetails> GetStockAsync(Guid savingsId, Guid stockId)
    {
        var stock = await _stockRepo.GetStockAsync(savingsId, stockId);

        return stock == null
            ? throw new DataNotFoundException($"No Stock or Share Details found in Stocks '{stockId}' with Savings Id '{savingsId}'")
            : stock.ToStocksAndSharesDetails();
    }

    public async Task<List<StocksAndSharesDetails>> GetStocksAsync(Guid savingsId)
    {
        var accounts = await _stockRepo.GetStocksAsync(savingsId);

        return accounts
            .Select(x => x.ToStocksAndSharesDetails())
            .ToList();
    }

    public async Task<StocksAndSharesDetails> UpdateStockAsync(Guid accountId, Guid savingsId, Guid stockId, UpdateStocksAndSharesRequest request)
    {
        await GetStockAsync(savingsId, stockId);

        var stockDetails = request.ToStocksAndShares();
        stockDetails.StockId = stockId;

        var stockTable = await _stockRepo.UpdateStockAsync(stockDetails);

        return stockTable.ToStocksAndSharesDetails();
    }
}
