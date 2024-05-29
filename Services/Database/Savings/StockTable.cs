﻿using System.ComponentModel.DataAnnotations;
using ChrisUsher.MoveMate.Shared.DTOs.Savings.Stocks;

namespace ChrisUsher.MoveMate.API.Services.Database.Savings;

public class StockTable
{
    [Key]
    public Guid SavingsId { get; set; } = Guid.NewGuid();

    public Guid StockId { get; set; }

    [Required]
    public string StockName { get; set; }

    public bool IsActive { get; set; } = true;

    [Required]
    public DateTime StartDate { get; set; }

    public List<StockBalance> Balances { get; set; } = new List<StockBalance>();

    public double MonthlySavingsAmount { get; set; }

    public StocksAndSharesDetails ToStocksAndSharesDetails()
    {
        return new StocksAndSharesDetails
        {
            SavingsId = SavingsId,
            StockId = StockId,
            StockName = StockName,
            IsActive = IsActive,
            StartDate = StartDate,
            Balances = Balances,
            MonthlySavingsAmount = MonthlySavingsAmount
        };
    }
}
