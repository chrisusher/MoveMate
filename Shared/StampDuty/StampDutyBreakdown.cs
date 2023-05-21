namespace ChrisUsher.MoveMate.Shared.StampDuty
{
    public class StampDutyBreakdown
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("percentage")]
        public double Percentage { get; set; }

        [JsonPropertyName("amount")]
        public double Amount { get; set; }

        [JsonPropertyName("taxDue")]
        public double TaxDue { get; set; }
    }
}