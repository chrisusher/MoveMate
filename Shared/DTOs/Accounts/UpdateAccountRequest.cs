namespace ChrisUsher.MoveMate.Shared.DTOs.Accounts
{
    public class UpdateAccountRequest : CreateAccountRequest
    {
        [JsonPropertyName("created")]
        public DateTime Created { get; set; }

        [JsonPropertyName("isDeleted")]
        public bool IsDeleted { get; set; }

        public Account ToAccount(Guid accountId)
        {
            return new Account
            {
                AccountId = accountId,
                Created = Created,
                Email = Email,
                EstimatedSaleDate = EstimatedSaleDate,
                IsDeleted = IsDeleted
            };
        }
    }
}