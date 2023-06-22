namespace ChrisUsher.MoveMate.Shared.DTOs
{
    public class Fluctuation
    {
        [JsonPropertyName("worstCase")]
        public double WorstCase { get; set; }

        [JsonPropertyName("bestCase")]
        public double BestCase { get; set; }
    }
}