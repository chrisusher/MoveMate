using ChrisUsher.MoveMate.API.Repositories;
using ChrisUsher.MoveMate.Shared.DTOs.Costs;

namespace ChrisUsher.MoveMate.API.Services.Costs
{
    public class CostService
    {
        private readonly CostRepository _costRepo;

        public CostService(CostRepository costRepository)
        {
            _costRepo = costRepository;
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