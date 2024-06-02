namespace ChrisUsher.MoveMate.Shared.DTOs.Savings;

public class UpdateSavingsAccountRequest : CreateSavingsAccountRequest
{
    [JsonPropertyName("created")]
    public DateTime Created { get; set; }

    [JsonPropertyName("isDeleted")]
    public bool IsDeleted { get; set; }

    [JsonPropertyName("balances")]
    public List<AccountBalance> Balances { get; set; }

    public SavingsAccount ToSavingsAccount(Guid accountId, Guid savingsId)
    {
        return new SavingsAccount
        {
            AccountId = accountId,
            SavingsId = savingsId,
            Name = Name,
            Created = Created,
            IsDeleted = IsDeleted,
            InitialBalance = InitialBalance,
            MonthlySavingsAmount = MonthlySavingsAmount,
            Balances = Balances,
            Fluctuations = Fluctuations,
            SavingsRate = SavingsRate,
            SavingType = SavingType
        };
    }
}