namespace ChrisUsher.MoveMate.Shared.DTOs.Savings;

public class SavingsInterestBreakdown
{
    [JsonPropertyName("interestEarned")]
    public double InterestEarned => Math.Round(MonthlyBreakdown.Sum(x => x.Interest), 2);

    [JsonPropertyName("newTotal")]
    public double NewTotal => Math.Round(MonthlyBreakdown.Last().Balance, 2);

    [JsonPropertyName("monthlyBreakdown")]
    public List<MonthlyInterestBreakdown> MonthlyBreakdown { get; set; } = new List<MonthlyInterestBreakdown>();
}
