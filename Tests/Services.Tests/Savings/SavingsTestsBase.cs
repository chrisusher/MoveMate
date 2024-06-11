using ChrisUsher.MoveMate.API.Services.Savings;
using ChrisUsher.MoveMate.Shared.DTOs.Savings;

namespace Services.Tests.Savings
{
    public class SavingsTestsBase
    {
        private readonly SavingsService _savingsService;

        public SavingsTestsBase()
        {
            _savingsService = ServiceTestsCommon.Services.GetService<SavingsService>();
        }

        protected SavingsService SavingsService => _savingsService;

        protected async Task<SavingsAccount> CreateSavingsAccount(SavingType savingType, CreateSavingsAccountRequest request = null)
        {
            return await SavingsService.CreateSavingsAccountAsync(ServiceTestsCommon.DefaultAccount.AccountId, request ?? new CreateSavingsAccountRequest
            {
                Name = $"{savingType} Saving Account",
                SavingType = savingType
            });
        }

        protected async Task<SavingsAccount> GetNewOrExistingSavingsAccountAsync()
        {
            var savings = await SavingsService.GetSavingsAccountsAsync(ServiceTestsCommon.DefaultAccount.AccountId);

            var saving = savings.FirstOrDefault();

            if (saving == null)
            {
                saving = await SavingsService.CreateSavingsAccountAsync(ServiceTestsCommon.DefaultAccount.AccountId, new CreateSavingsAccountRequest
                {
                    Name = $"Test Account-{DateTime.UtcNow.Ticks}",
                    SavingType = SavingType.CurrentAccount
                });
            }

            return saving;
        }
    }
}