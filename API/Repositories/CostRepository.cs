using ChrisUsher.MoveMate.API.Database;
using ChrisUsher.MoveMate.API.Database.Costs;
using ChrisUsher.MoveMate.Shared.DTOs.Costs;
using Microsoft.EntityFrameworkCore;

namespace ChrisUsher.MoveMate.API.Repositories
{
    public class CostRepository
    {
        private readonly DatabaseContext _databaseContext;

        public CostRepository(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public async Task<CostTable> GetCostAsync(Guid accountId, Guid costId)
        {
            return await _databaseContext.Costs
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.AccountId == accountId && x.CostId == costId);
        }

        public async Task<List<CostTable>> GetCostsAsync(Guid accountId)
        {
            return await _databaseContext.Costs
                .Where(x => x.AccountId == accountId
                            && !x.IsDeleted)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<CostTable> CreateCostAsync(Guid accountId, Cost cost)
        {
            var costTable = new CostTable
            {
                AccountId = accountId,
                Name = cost.Name,
                FixedCost = cost.FixedCost,
                Fluctuations = cost.Fluctuations,
                PercentageOfSale = cost.PercentageOfSale,
            };
            await _databaseContext.Costs.AddAsync(costTable);

            await _databaseContext.SaveChangesAsync();

            return costTable;
        }

        public async Task<CostTable> UpdateCostAsync(Cost cost)
        {
            var costTable = await _databaseContext.Costs.FirstAsync(x => x.CostId == cost.CostId);

            costTable.FixedCost = cost.FixedCost;
            costTable.IsDeleted = cost.IsDeleted;
            costTable.Name = cost.Name;
            costTable.Fluctuations = cost.Fluctuations;
            costTable.PercentageOfSale = cost.PercentageOfSale;

            _databaseContext.Costs.Update(costTable);

            await _databaseContext.SaveChangesAsync();

            return costTable;
        }
    }
}