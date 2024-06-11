namespace ChrisUsher.MoveMate.Shared.DTOs.Savings.Stocks;

public class StockReturnDetail
{
    [JsonPropertyName("stockId")]
    public Guid StockId { get; set; }

    [JsonPropertyName("stockName")]
    public string StockName { get; set; }

    [JsonPropertyName("amountInvested")]
    public double AmountInvested { get; set; }

    [JsonPropertyName("currentValue")]
    public double CurrentValue { get; set; }

    [JsonPropertyName("amountChange")]
    public double AmountChange
    {
        get
        {
            if(CurrentValue == 0 || AmountInvested == 0)
            {
                return 0;
            }

            return CurrentValue - AmountInvested;
        }
    }

    [JsonPropertyName("percentageChange")]
    public double PercentageChange
    {
        get
        {
            var amountChange = AmountChange;
            if(amountChange == 0)
            {
                return 0;
            }

            return amountChange / AmountInvested;
        }
    }
}
