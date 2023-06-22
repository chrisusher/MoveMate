namespace ChrisUsher.MoveMate.Shared.DTOs.Costs
{
    public class UpdateCostRequest : CreateCostRequest
    {
        [JsonPropertyName("created")]
        public DateTime Created { get; set; }

        [JsonPropertyName("isDeleted")]
        public bool IsDeleted { get; set; }

        public Cost ToCost(Guid accountId, Guid costId)
        {
            return new Cost
            {
                AccountId = accountId,
                CostId = costId,
                FixedCost = FixedCost,
                Created = Created,
                IsDeleted = IsDeleted,
                Fluctuations = Fluctuations,
                Name = Name,
                PercentageOfSale = PercentageOfSale,
            };
        }
    }
}