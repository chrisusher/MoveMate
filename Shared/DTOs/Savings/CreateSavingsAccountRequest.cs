namespace ChrisUsher.MoveMate.Shared.DTOs.Savings;

public class CreateSavingsAccountRequest
{
    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("initialBalance")]
    public double InitialBalance { get; set; }

    [JsonPropertyName("savingsRate")]
    public double SavingsRate { get; set; }

    [JsonPropertyName("monthlySavingsAmount")]
    public double MonthlySavingsAmount { get; set; }

    [JsonPropertyName("fluctuations")]
    public Fluctuation Fluctuations { get; set; }

    public SavingsAccount ToSavingsAccount()
    {
        return new SavingsAccount
        {
            Name = Name,
            InitialBalance = InitialBalance,
            SavingsRate = SavingsRate,
            MonthlySavingsAmount = MonthlySavingsAmount,
            Fluctuations = Fluctuations
        };
    }
}