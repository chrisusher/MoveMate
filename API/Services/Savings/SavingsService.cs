using ChrisUsher.MoveMate.API.Repositories;
using ChrisUsher.MoveMate.Shared.DTOs.Savings;

namespace ChrisUsher.MoveMate.API.Services.Savings;

public class SavingsService
{
    private readonly SavingsRepository _savingsRepo;

    public SavingsService(SavingsRepository savingsRepository)
    {
        _savingsRepo = savingsRepository;
    }

    public async Task<SavingsAccount> CreateSavingsAccountAsync(Guid accountId, CreateSavingsAccountRequest request)
    {
        var account = request.ToSavingsAccount();
        
        var accountTable = await _savingsRepo.CreateSavingsAccountAsync(accountId, account);

        return accountTable.ToSavingsAccount();
    }

    public async Task<SavingsAccount> GetSavingsAccountAsync(Guid accountId, Guid savingsId)
    {
        var account = await _savingsRepo.GetSavingsAccountAsync(accountId, savingsId);

        if(account == null)
        {
            throw new DataNotFoundException($"No Savings Account found in Account '{accountId}' with Savings Id '{savingsId}'");
        }

        return account.ToSavingsAccount();
    }
    
    public async Task<List<SavingsAccount>> GetSavingsAccountsAsync(Guid accountId)
    {
        var accounts = await _savingsRepo.GetSavingsAsync(accountId);

        return accounts
            .Select(x => x.ToSavingsAccount())
            .ToList();
    }

    public async Task<SavingsAccount> UpdateSavingsAccountAsync(Guid accountId, Guid savingsId, UpdateSavingsAccountRequest request)
    {
        var account = request.ToSavingsAccount(accountId, savingsId);
        
        var accountTable = await _savingsRepo.UpdateSavingsAccountAsync(account);

        return accountTable.ToSavingsAccount();
    }
}