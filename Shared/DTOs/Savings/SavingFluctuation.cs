namespace ChrisUsher.MoveMate.Shared.DTOs.Savings
{
    public class SavingFluctuation
    {
        [JsonPropertyName("worstCase")]
        public double WorstCase { get; set; }

        [JsonPropertyName("bestCase")]
        public double BestCase { get; set; }
    }
}