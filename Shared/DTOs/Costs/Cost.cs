namespace ChrisUsher.MoveMate.Shared.DTOs.Costs
{
    public class Cost : UpdateCostRequest
    {
        [JsonPropertyName("costId")]
        public Guid CostId { get; set; }

        [JsonPropertyName("accountId")]
        public Guid AccountId { get; set; }
    }
}