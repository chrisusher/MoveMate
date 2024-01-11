namespace ChrisUsher.MoveMate.Shared.DTOs.Savings
{
    public class SavingsSummary
    {
        [JsonPropertyName("currentBalance")]
        public double CurrentBalance { get; set; } = 0;

        [JsonPropertyName("name")]
        public string Name { get; set; }
    }
}