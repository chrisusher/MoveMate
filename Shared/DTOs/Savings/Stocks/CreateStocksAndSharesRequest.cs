namespace ChrisUsher.MoveMate.Shared.DTOs.Savings.Stocks;

public class CreateStocksAndSharesRequest
{
    [JsonPropertyName("stockName")]
    public string StockName { get; set; }

    [JsonPropertyName("isActive")]
    public bool IsActive { get; set; }

    [JsonPropertyName("startDate")]
    public DateTime StartDate { get; set; }

    [JsonPropertyName("monthlySavingsAmount")]
    public double MonthlySavingsAmount { get; set; }

    public StocksAndSharesDetails ToStocksAndShares()
    {
        return new StocksAndSharesDetails
        {
            StockName = StockName,
            IsActive = IsActive,
            StartDate = StartDate,
            MonthlySavingsAmount = MonthlySavingsAmount,
        };
    }
}
