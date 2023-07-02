using System.Text.Json;

namespace ChrisUsher.MoveMate.Shared.DTOs.Savings;

public class ReportSavingsAccount : SavingsAccount
{
    [JsonPropertyName("currentBalance")] 
    public double CurrentBalance { get; set; } = 0;

    public static ReportSavingsAccount FromSavingsAccount(SavingsAccount savingsAccount)
    {
        var savingsAccountJson = JsonSerializer.Serialize(savingsAccount);

        return JsonSerializer.Deserialize<ReportSavingsAccount>(savingsAccountJson);
    }
}