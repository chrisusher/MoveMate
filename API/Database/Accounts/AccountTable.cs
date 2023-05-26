using System.ComponentModel.DataAnnotations;

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
    }
}