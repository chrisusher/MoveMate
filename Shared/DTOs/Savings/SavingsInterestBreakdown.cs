namespace ChrisUsher.MoveMate.Shared.DTOs.Savings;

public class SavingsInterestBreakdown
{
    [JsonPropertyName("interestEarned")]
    public double InterestEarned => MonthlyBreakdown.Sum(x => x.Interest);

    [JsonPropertyName("newTotal")]
    public double NewTotal => MonthlyBreakdown.Last().Balance;

    [JsonPropertyName("monthlyBreakdown")]
    public List<MonthlyInterestBreakdown> MonthlyBreakdown { get; set; } = new List<MonthlyInterestBreakdown>();
}
