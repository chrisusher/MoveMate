using ChrisUsher.MoveMate.Shared.DTOs.Savings.Stocks;

namespace ChrisUsher.MoveMate.Shared.DTOs.Reports;

public class StockInvestmentReport
{
    [JsonPropertyName("stocks")]
    public List<StockReturnDetail> Stocks { get; set; } = new();
}
