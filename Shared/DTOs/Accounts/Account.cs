namespace ChrisUsher.MoveMate.Shared.DTOs.Accounts
{
    public class Account : UpdateAccountRequest
    {
        [JsonPropertyName("accountId")]
        public Guid AccountId { get; set; }
    }
}