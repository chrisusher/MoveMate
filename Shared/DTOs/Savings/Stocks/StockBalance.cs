namespace ChrisUsher.MoveMate.Shared.DTOs.Savings.Stocks;

public class StockBalance : AccountBalance
{
    [JsonPropertyName("amountInvested")]
    public double AmountInvested { get; set; }
}
