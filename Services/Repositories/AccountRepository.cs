using ChrisUsher.MoveMate.API.Database;
using ChrisUsher.MoveMate.API.Database.Accounts;
using ChrisUsher.MoveMate.Shared.DTOs.Accounts;
using Microsoft.EntityFrameworkCore;

namespace ChrisUsher.MoveMate.API.Repositories
{
    public class AccountRepository
    {
        private DatabaseContext _databaseContext;

        public AccountRepository(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public async Task<AccountTable> GetAccountAsync(Guid accountId)
        {
            return await _databaseContext.Accounts
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.AccountId == accountId);
        }

        public async Task<AccountTable> CreateAccountAsync(Account account)
        {
            var accountTable = new AccountTable
            {
                Email = account.Email,
                EstimatedSaleDate = account.EstimatedSaleDate
            };
            await _databaseContext.Accounts.AddAsync(accountTable);

            await _databaseContext.SaveChangesAsync();

            return accountTable;
        }

        public async Task<AccountTable> UpdateAccountAsync(Account account)
        {
            var accountTable = await _databaseContext.Accounts.FirstAsync(x => x.AccountId == account.AccountId);

            accountTable.Email = account.Email;
            accountTable.EstimatedSaleDate = account.EstimatedSaleDate;
            accountTable.IsDeleted = account.IsDeleted;

            _databaseContext.Accounts.Update(accountTable);

            await _databaseContext.SaveChangesAsync();

            return accountTable;
        }
    }
}