using ChrisUsher.MoveMate.API.Services.Repositories;
using ChrisUsher.MoveMate.API.Services.StampDuty;
using ChrisUsher.MoveMate.Shared.DTOs.Costs;
using ChrisUsher.MoveMate.Shared.DTOs.Properties;
using ChrisUsher.MoveMate.Shared.DTOs.StampDuty;
using ChrisUsher.MoveMate.Shared.Enums;

namespace ChrisUsher.MoveMate.API.Services.Costs
{
    public class CostService
    {
        private readonly CostRepository _costRepo;
        private readonly StampDutyService _stampDutyService;

        public CostService(
            CostRepository costRepository,
            StampDutyService stampDutyService)
        {
            _costRepo = costRepository;
            _stampDutyService = stampDutyService;
        }

        public List<Cost> CalculateCosts(List<Cost> costs, CaseType caseType, Property purchaseProperty, Property currentProperty)
        {
            for (int index = 0; index < costs.Count; index++)
            {
                var cost = costs[index];

                if (currentProperty != null && cost.PercentageOfSale.HasValue)
                {
                    var percentage = cost.PercentageOfSale.Value / 100;
                    costs[index].PercentageOfSale = null;

                    switch (caseType)
                    {
                        case CaseType.BestCase:
                            costs[index].FixedCost = Math.Round(currentProperty.MaxValue * percentage, 2);
                            break;
                        case CaseType.WorstCase:
                            costs[index].FixedCost = Math.Round(currentProperty.MinValue * percentage, 2);
                            break;
                        case CaseType.MiddleCase:
                            var middleValue = (currentProperty.MaxValue + currentProperty.MinValue) / 2;
                            costs[index].FixedCost = Math.Round(middleValue * percentage, 2);
                            break;
                    }

                    if (!cost.IncludesVAT)
                    {
                        costs[index].FixedCost = CostCalculations.CalculateVAT(costs[index].FixedCost);
                    }
                }
            }

            costs.Add(new Cost
            {
                Name = "Stamp Duty",
                FixedCost = _stampDutyService.CalculateStampDuty(purchaseProperty, new StampDutyRequest
                {
                    AdditionalProperty = false,
                    ResidentialType = PropertyResidentialType.Residential,
                    Location = UKRegionType.Wales,
                }, caseType).Amount,
                Created = DateTime.UtcNow,
            });

            return costs;
        }

        public async Task<Cost> CreateCostAsync(Guid accountId, CreateCostRequest request)
        {
            var cost = request.ToCost();

            var costTable = await _costRepo.CreateCostAsync(accountId, cost);

            return costTable.ToCost();
        }

        public async Task<Cost> GetCostAsync(Guid accountId, Guid costId)
        {
            var cost = await _costRepo.GetCostAsync(accountId, costId);

            return cost == null
                ? throw new DataNotFoundException($"No Cost found in Account '{accountId}' with Cost Id '{costId}'")
                : cost.ToCost();
        }

        public async Task<List<Cost>> GetCostsAsync(Guid accountId)
        {
            var costs = await _costRepo.GetCostsAsync(accountId);

            return costs
                .Select(x => x.ToCost())
                .ToList();
        }

        public async Task<Cost> UpdateCostAsync(Guid accountId, Guid costId, UpdateCostRequest request)
        {
            var account = request.ToCost(accountId, costId);

            var costTable = await _costRepo.UpdateCostAsync(account);

            return costTable.ToCost();
        }
    }
}