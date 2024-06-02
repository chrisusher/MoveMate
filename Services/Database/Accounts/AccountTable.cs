using System.ComponentModel.DataAnnotations;
using ChrisUsher.MoveMate.Shared.DTOs.Accounts;
using MongoDB.Bson;

namespace ChrisUsher.MoveMate.API.Services.Database.Accounts
{
    public class AccountTable
    {
#if RELEASE
        [Key]
#elif DEBUG
        public ObjectId _id { get; set; }
#endif
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