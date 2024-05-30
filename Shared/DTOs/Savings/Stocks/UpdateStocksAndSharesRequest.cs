namespace ChrisUsher.MoveMate.Shared.DTOs.Savings.Stocks;

public class UpdateStocksAndSharesRequest : CreateStocksAndSharesRequest
{
    [JsonPropertyName("balances")]
    public List<StockBalance> Balances { get; set; }

    [JsonPropertyName("isDeleted")]
    public bool IsDeleted { get; set; }

    public new StocksAndSharesDetails ToStocksAndShares()
    {
        return new StocksAndSharesDetails
        {
            SavingsId = SavingsId,
            StockName = StockName,
            IsActive = IsActive,
            StartDate = StartDate,
            MonthlySavingsAmount = MonthlySavingsAmount,
            Balances = Balances,
            IsDeleted = IsDeleted
        };
    }
}
