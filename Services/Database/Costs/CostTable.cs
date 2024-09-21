using System.ComponentModel.DataAnnotations;
using ChrisUsher.MoveMate.Shared.DTOs;
using ChrisUsher.MoveMate.Shared.DTOs.Costs;
using MongoDB.Bson;

namespace ChrisUsher.MoveMate.API.Services.Database.Costs
{
    public class CostTable
    {
#if RELEASE
        [Key]
#elif DEBUG
        public ObjectId _id { get; set; }
#endif
        public Guid CostId { get; set; } = Guid.NewGuid();

        public Guid AccountId { get; set; }

        public string Name { get; set; }

        public DateTime Created { get; set; } = DateTime.UtcNow;

        public bool IsDeleted { get; set; }

        public double FixedCost { get; set; }

        public double? PercentageOfSale { get; set; }

        public Fluctuation Fluctuations { get; set; }

        public bool IncludesVAT { get; set; }

        public Cost ToCost()
        {
            return new Cost
            {
                AccountId = AccountId,
                FixedCost = FixedCost,
                CostId = CostId,
                Created = Created,
                IsDeleted = IsDeleted,
                Fluctuations = Fluctuations,
                Name = Name,
                PercentageOfSale = PercentageOfSale,
                IncludesVAT = IncludesVAT
            };
        }
    }
}