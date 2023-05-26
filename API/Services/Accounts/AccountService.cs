using ChrisUsher.MoveMate.API.Database.Accounts;
using ChrisUsher.MoveMate.API.Repositories;

namespace ChrisUsher.MoveMate.API.Services.Accounts
{
    public class AccountService
    {
        private readonly AccountRepository _accountRepository;

        public AccountService(AccountRepository repository)
        {
            _accountRepository = repository;
        }

        public async Task<AccountTable> GetAccountAsync(Guid accountId)
        {
            return await _accountRepository.GetAccount(accountId);
        }

        public async Task<AccountTable> CreateAccountAsync(AccountTable account)
        {
            return await _accountRepository.CreateAccount(account);
        }
    }
}