using ChrisUsher.MoveMate.API.Database;
using ChrisUsher.MoveMate.API.Database.Accounts;
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

        public async Task<AccountTable> GetAccount(Guid accountId)
        {
            return await _databaseContext.Accounts
                .FirstOrDefaultAsync(x => x.AccountId == accountId);
        }

        public async Task<AccountTable> CreateAccount(AccountTable account)
        {
            _databaseContext.Accounts.Add(account);

            await _databaseContext.SaveChangesAsync();

            return account;
        }
    }
}