namespace ChrisUsher.MoveMate.Shared.DTOs.Savings;

public class SavingsAccount : UpdateSavingsAccountRequest
{
    [JsonPropertyName("savingsId")]
    public Guid SavingsId { get; set; }
    
    [JsonPropertyName("accountId")]
    public Guid AccountId { get; set; }
    
    [JsonPropertyName("balances")]
    public List<AccountBalance> Balances { get; set; }
}