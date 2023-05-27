namespace ChrisUsher.MoveMate.Shared.DTOs.Accounts
{
    public class CreateAccountRequest
    {
        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("estimatedSaleDate")]
        public DateTime? EstimatedSaleDate { get; set; }

        public Account ToAccount()
        {
            return new Account
            {
                Email = Email,
                EstimatedSaleDate = EstimatedSaleDate
            };
        }
    }
}