namespace ChrisUsher.MoveMate.Shared.DTOs.Savings;

public class MonthlyInterestBreakdown
{
    [JsonPropertyName("interest")]
    public double Interest { get; set; }

    [JsonPropertyName("balance")]
    public double Balance { get; set; }

    [JsonPropertyName("deposits")]
    public double Deposits { get; set; }

    [JsonPropertyName("date")]
    public DateTime Date { get; set; }
}
