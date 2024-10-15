using ChrisUsher.MoveMate.API.Services.Database;
using ChrisUsher.MoveMate.API.Services.Database.Savings;
using ChrisUsher.MoveMate.Shared.DTOs.Savings.Stocks;
using Microsoft.EntityFrameworkCore;

namespace ChrisUsher.MoveMate.API.Services.Repositories;

public class StockRepository
{
    private readonly DatabaseContext _databaseContext;

    public StockRepository(DatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    public async Task<StockTable> AddBalanceToStockAsync(Guid savingsId, Guid stockId, double currentBalance, double totalInvestment)
    {
        var stockTable = await _databaseContext.Stocks.FirstAsync(x => x.StockId == stockId);

        stockTable.Balances.Add(new StockBalance
        {
            AmountInvested = totalInvestment,
            Balance = currentBalance,
            Created = DateTime.UtcNow
        });

        _databaseContext.Stocks.Update(stockTable);

        await _databaseContext.SaveChangesAsync();

        return stockTable;
    }

    public async Task<StockTable> CreateStockAsync(Guid savingsId, CreateStocksAndSharesRequest stockDetails)
    {
        var stock = new StockTable
        {
            SavingsId = savingsId,
            StockId = Guid.NewGuid(),
            StockName = stockDetails.StockName,
            IsActive = stockDetails.IsActive,
            StartDate = stockDetails.StartDate,
            MonthlySavingsAmount = stockDetails.MonthlySavingsAmount
        };

        await _databaseContext.Stocks.AddAsync(stock);
        await _databaseContext.SaveChangesAsync();

        return stock;
    }

    public async Task<StockTable> GetStockAsync(Guid savingsId, Guid stockId)
    {
        return await _databaseContext.Stocks
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.SavingsId == savingsId && x.StockId == stockId);
    }

    public async Task<List<StockTable>> GetStocksAsync(Guid savingsId)
    {
        return await _databaseContext.Stocks
            .Where(x => x.SavingsId == savingsId
                        && !x.IsDeleted)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<StockTable> UpdateStockAsync(StocksAndSharesDetails stockDetails)
    {
        var stockTable = await _databaseContext.Stocks.FirstAsync(x => x.StockId == stockDetails.StockId);

        stockTable.StockName = stockDetails.StockName;
        stockTable.IsActive = stockDetails.IsActive;
        stockTable.StartDate = stockDetails.StartDate;
        stockTable.MonthlySavingsAmount = stockDetails.MonthlySavingsAmount;

        stockTable.Balances = stockDetails.Balances;

        _databaseContext.Stocks.Update(stockTable);

        await _databaseContext.SaveChangesAsync();

        return stockTable;
    }
}
