namespace ChrisUsher.MoveMate.Shared.DTOs.Savings
{
    public class AccountBalance
    {
        [JsonPropertyName("created")]
        public DateTime Created { get; set; }

        [JsonPropertyName("balance")]
        public double Balance { get; set; }
    }
}