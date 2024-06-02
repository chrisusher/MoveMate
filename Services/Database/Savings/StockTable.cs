using System.ComponentModel.DataAnnotations;
using ChrisUsher.MoveMate.Shared.DTOs.Savings.Stocks;
using MongoDB.Bson;

namespace ChrisUsher.MoveMate.API.Services.Database.Savings;

public class StockTable
{
#if RELEASE
        [Key]
#elif DEBUG
    public ObjectId _id { get; set; }
#endif
    public Guid SavingsId { get; set; } = Guid.NewGuid();

    public Guid StockId { get; set; }

    [Required]
    public string StockName { get; set; }

    public bool IsActive { get; set; } = true;

    public bool IsDeleted { get; set; }

    [Required]
    public DateTime StartDate { get; set; }

    public List<StockBalance> Balances { get; set; } = new List<StockBalance>();

    public double MonthlySavingsAmount { get; set; }

    public StocksAndSharesDetails ToStocksAndSharesDetails()
    {
        return new StocksAndSharesDetails
        {
            StockId = StockId,
            StockName = StockName,
            IsActive = IsActive,
            IsDeleted = IsDeleted,
            StartDate = StartDate,
            Balances = Balances,
            MonthlySavingsAmount = MonthlySavingsAmount
        };
    }
}
