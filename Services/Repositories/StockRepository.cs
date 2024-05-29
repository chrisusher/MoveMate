﻿using ChrisUsher.MoveMate.API.Database;
using ChrisUsher.MoveMate.API.Services.Database.Savings;
using ChrisUsher.MoveMate.Shared.DTOs.Savings.Stocks;
using Microsoft.EntityFrameworkCore;

namespace ChrisUsher.MoveMate.API.Repositories;

public class StockRepository
{
    private readonly DatabaseContext _databaseContext;

    public StockRepository(DatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    public async Task<StockTable> CreateStockAsync(CreateStocksAndSharesRequest stockDetails)
    {
        var stock = new StockTable
        {
            SavingsId = stockDetails.SavingsId,
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

    public async Task<StockTable> UpdateStockAsync(StocksAndSharesDetails stockDetails)
    {
        var stockTable = await _databaseContext.Stocks.FirstAsync(x => x.StockId == stockDetails.StockId);

        stockTable.IsActive = stockDetails.IsActive;
        stockTable.StartDate = stockDetails.StartDate;
        stockTable.MonthlySavingsAmount = stockDetails.MonthlySavingsAmount;

        stockTable.Balances = stockDetails.Balances;

        _databaseContext.Stocks.Update(stockTable);

        await _databaseContext.SaveChangesAsync();

        return stockTable;
    }
}
