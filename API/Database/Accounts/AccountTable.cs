using System.ComponentModel.DataAnnotations;
using ChrisUsher.MoveMate.Shared.DTOs.Accounts;

namespace ChrisUsher.MoveMate.API.Database.Accounts
{
    public class AccountTable
    {
        [Key]
        public Guid AccountId { get; set; } = Guid.NewGuid();

        public string Email { get; set; }

        public DateTime Created { get; set; } = DateTime.UtcNow;

        public bool IsDeleted { get; set; }

        public DateTime? EstimatedSaleDate { get; set; }

        public Account ToAccount()
        {
            return new Account
            {
                AccountId = AccountId,
                Created = Created,
                Email = Email,
                EstimatedSaleDate = EstimatedSaleDate,
                IsDeleted = IsDeleted
            };
        }
    }
}