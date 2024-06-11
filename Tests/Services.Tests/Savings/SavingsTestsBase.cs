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
    }
}