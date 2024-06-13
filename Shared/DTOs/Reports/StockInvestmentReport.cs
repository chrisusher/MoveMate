using ChrisUsher.MoveMate.Shared.DTOs.Savings.Stocks;

namespace ChrisUsher.MoveMate.Shared.DTOs.Reports;

public class StockInvestmentReport
{
    [JsonPropertyName("stocks")]
    public List<StockReturnDetail> Stocks { get; set; } = new();

    [JsonPropertyName("overallChange")]
    public double OverallChange
    {
        get
        {
            if (Stocks.Count == 0)
            {
                return 0;
            }

            return Stocks.Sum(x => x.AmountChange);
        }
    }

    [JsonPropertyName("overallChange")]
    public double OverallPercentageChange
    {
        get
        {
            if(Stocks.Count == 0)
            {
                return 0;
            }

            return Math.Round(Stocks.Sum(x => x.PercentageChange) / Stocks.Count, 5);
        }
    }
}
