using System.ComponentModel.DataAnnotations;
using ChrisUsher.MoveMate.Shared.DTOs.Savings;

namespace ChrisUsher.MoveMate.API.Database.Savings
{
    public class SavingsTable
    {
        [Key]
        public Guid SavingsId { get; set; } = Guid.NewGuid();

        public Guid AccountId { get; set; }

        public string Name { get; set; }

        public DateTime Created { get; set; } = DateTime.UtcNow;

        public bool IsDeleted { get; set; }

        public List<AccountBalance> Balances { get; set; } = new List<AccountBalance>();

        public double MonthlySavingsAmount { get; set; }
        
        public double SavingsRate { get; set; }
        
        public double InitialBalance { get; set; }

        public SavingsAccount ToSavingsAccount()
        {
            return new SavingsAccount
            {
                AccountId = AccountId,
                SavingsId = SavingsId,
                Name = Name,
                InitialBalance = InitialBalance,
                MonthlySavingsAmount = MonthlySavingsAmount,
                SavingsRate = SavingsRate,
                IsDeleted = IsDeleted,
                Created = Created,
                Balances = Balances
            };
        }
    }
}