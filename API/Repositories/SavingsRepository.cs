using ChrisUsher.MoveMate.API.Database;
using ChrisUsher.MoveMate.API.Database.Savings;
using ChrisUsher.MoveMate.Shared.DTOs.Savings;
using Microsoft.EntityFrameworkCore;

namespace ChrisUsher.MoveMate.API.Repositories;

public class SavingsRepository
{
    private readonly DatabaseContext _databaseContext;

    public SavingsRepository(DatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    public async Task<SavingsTable> GetSavingsAccountAsync(Guid accountId, Guid savingsId)
    {
        return await _databaseContext.Savings
            .FirstOrDefaultAsync(x => x.AccountId == accountId && x.SavingsId == savingsId);
    }

    public async Task<List<SavingsTable>> GetSavingsAsync(Guid accountId)
    {
        return await _databaseContext.Savings
            .Where(x => x.AccountId == accountId
                        && !x.IsDeleted)
            .ToListAsync();
    }
    
    public async Task<SavingsTable> CreateSavingsAccountAsync(Guid accountId, SavingsAccount account)
    {
        var savingsTable = new SavingsTable
        {
            AccountId = accountId,
            Name = account.Name,
            InitialBalance = account.InitialBalance,
            SavingsRate = account.SavingsRate,
            MonthlySavingsAmount = account.MonthlySavingsAmount
        };
        _databaseContext.Savings.Add(savingsTable);

        await _databaseContext.SaveChangesAsync();

        return savingsTable;
    }

    public async Task<SavingsTable> UpdateSavingsAccountAsync(SavingsAccount account)
    {
        var accountTable = await _databaseContext.Savings.FirstAsync(x => x.SavingsId == account.SavingsId);

        accountTable.InitialBalance = account.InitialBalance;
        accountTable.SavingsRate = account.SavingsRate;
        accountTable.MonthlySavingsAmount = account.MonthlySavingsAmount;
        accountTable.IsDeleted = account.IsDeleted;
        accountTable.Name = account.Name;

        _databaseContext.Savings.Update(accountTable);

        await _databaseContext.SaveChangesAsync();

        return accountTable;
    }
}