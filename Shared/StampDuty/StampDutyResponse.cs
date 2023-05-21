namespace ChrisUsher.MoveMate.Shared.StampDuty
{
    public class StampDutyResponse
    {
        [JsonPropertyName("calculationDate")]
        public DateTime CalculationDate { get; set; }

        [JsonPropertyName("stampDuty")]
        public double Amount { get; set; }

        [JsonPropertyName("breakdown")]
        public List<StampDutyBreakdown> Breakdown { get; set; } = new List<StampDutyBreakdown>();
    }
}