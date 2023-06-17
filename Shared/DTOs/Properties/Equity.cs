namespace ChrisUsher.MoveMate.Shared.DTOs.Properties;

public class Equity
{
    [JsonPropertyName("remainingMortgage")]
    public double RemainingMortgage { get; set; }

    [JsonPropertyName("updated")]
    public DateTime Updated { get; set; }
}
