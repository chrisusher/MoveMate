using System.Text.Json;

namespace ChrisUsher.MoveMate.Shared.DTOs.Savings;

public class ReportSavingsAccount : SavingsAccount
{
    [JsonPropertyName("currentBalance")]
    public double CurrentBalance { get; set; } = 0;

    public static ReportSavingsAccount FromSavingsAccount(SavingsAccount savingsAccount)
    {
        var savingsAccountJson = JsonSerializer.Serialize(savingsAccount);

        var reportSavingsAccount = JsonSerializer.Deserialize<ReportSavingsAccount>(savingsAccountJson);

        if (savingsAccount.Balances is not null)
        {
            if (savingsAccount.Balances.Count > 0)
            {
                reportSavingsAccount.CurrentBalance = savingsAccount.Balances.Last().Balance;
            }
        }
        if (reportSavingsAccount.CurrentBalance == 0)
        {
            reportSavingsAccount.CurrentBalance = savingsAccount.InitialBalance;
        }

        return reportSavingsAccount;
    }
}