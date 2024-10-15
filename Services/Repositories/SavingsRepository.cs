using ChrisUsher.MoveMate.API.Services.Database;
using ChrisUsher.MoveMate.API.Services.Database.Savings;
using ChrisUsher.MoveMate.Shared.DTOs.Savings;
using Microsoft.EntityFrameworkCore;

namespace ChrisUsher.MoveMate.API.Services.Repositories;

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
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.AccountId == accountId && x.SavingsId == savingsId);
    }

    public async Task<List<SavingsTable>> GetSavingsAsync(Guid accountId)
    {
        return await _databaseContext.Savings
            .Where(x => x.AccountId == accountId
                        && !x.IsDeleted)
            .AsNoTracking()
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
            MonthlySavingsAmount = account.MonthlySavingsAmount,
            Fluctuations = account.Fluctuations,
            SavingType = account.SavingType
        };
        await _databaseContext.Savings.AddAsync(savingsTable);

        await _databaseContext.SaveChangesAsync();

        return savingsTable;
    }

    public async Task<SavingsTable> CreateNewBalanceAsync(Guid accountId, Guid savingsId, double balance)
    {
        var savings = await _databaseContext.Savings.FirstAsync(x => x.AccountId == accountId && x.SavingsId == savingsId);

        savings.Balances.Add(new AccountBalance
        {
            Created = DateTime.UtcNow,
            Balance = balance
        });

        await _databaseContext.SaveChangesAsync();

        return savings;
    }

    public async Task<SavingsTable> UpdateSavingsAccountAsync(SavingsAccount account)
    {
        var accountTable = await _databaseContext.Savings.FirstAsync(x => x.SavingsId == account.SavingsId);

        accountTable.InitialBalance = account.InitialBalance;
        accountTable.SavingsRate = account.SavingsRate;
        accountTable.MonthlySavingsAmount = account.MonthlySavingsAmount;
        accountTable.IsDeleted = account.IsDeleted;
        accountTable.Name = account.Name;
        accountTable.Fluctuations = account.Fluctuations;
        accountTable.SavingType = account.SavingType;

        _databaseContext.Savings.Update(accountTable);

        await _databaseContext.SaveChangesAsync();

        return accountTable;
    }
}