using ChrisUsher.MoveMate.API.Repositories;
using ChrisUsher.MoveMate.Shared.DTOs.Accounts;

namespace ChrisUsher.MoveMate.API.Services.Accounts
{
    public class AccountService
    {
        private readonly AccountRepository _accountRepository;

        public AccountService(AccountRepository repository)
        {
            _accountRepository = repository;
        }

        public async Task<Account> GetAccountAsync(Guid accountId)
        {
            var account = await _accountRepository.GetAccountAsync(accountId);

            return account.ToAccount();
        }

        public async Task<Account> CreateAccountAsync(CreateAccountRequest request)
        {
            var account = request.ToAccount();

            var table = await _accountRepository.CreateAccountAsync(account);
            
            return table.ToAccount();
        }

        public async Task<Account> UpdateAccountAsync(Guid accountId, UpdateAccountRequest updateRequest)
        {
            var account = updateRequest.ToAccount(accountId);

            var table = await _accountRepository.UpdateAccountAsync(account);
            
            return table.ToAccount();
        }
    }
}