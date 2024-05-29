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

    public async Task<StocksAndSharesDetails> CreateStockAsync(Guid accountId, CreateStocksAndSharesRequest stockDetailsRequest)
    {
        var savingsAccount = await _savingsService.GetSavingsAccountAsync(accountId, stockDetailsRequest.SavingsId);

        if(savingsAccount.SavingType != SavingType.StocksAndShares)
        {
            throw new InvalidOperationException($"Savings Account '{savingsAccount.SavingsId}' is not of type Stocks and Shares");
        }

        var stock = await _stockRepo.CreateStockAsync(stockDetailsRequest);

        return stock.ToStocksAndSharesDetails();
    }

    public async Task<StocksAndSharesDetails> GetStockAsync(Guid savingsId, Guid stockId)
    {
        var stock = await _stockRepo.GetStockAsync(savingsId, savingsId);

        return stock == null
            ? throw new DataNotFoundException($"No Stock or Share Details found in Stocks '{stockId}' with Savings Id '{savingsId}'")
            : stock.ToStocksAndSharesDetails();
    }

    public async Task<StocksAndSharesDetails> UpdateSavingsAccountAsync(Guid accountId, Guid savingsId, Guid stockId, UpdateStocksAndSharesRequest request)
    {
        var stockDetails = request.ToStocksAndShares();

        var stockTable = await _stockRepo.UpdateStockAsync(stockDetails);

        return stockTable.ToStocksAndSharesDetails();
    }
}
