namespace ChrisUsher.MoveMate.Shared.DTOs.Costs
{
    public class CreateCostRequest
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("fixedCost")]
        public double FixedCost { get; set; }

        [JsonPropertyName("percentageOfSale")]
        public double? PercentageOfSale { get; set; }

        [JsonPropertyName("fluctuations")]
        public Fluctuation Fluctuations { get; set; }

        public Cost ToCost()
        {
            return new Cost
            {
                FixedCost = FixedCost,
                Fluctuations = Fluctuations,
                Name = Name,
                PercentageOfSale = PercentageOfSale,
            };
        }
    }
}