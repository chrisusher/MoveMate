using System.ComponentModel.DataAnnotations;

namespace ChrisUsher.MoveMate.API.Database.Savings
{
    public class SavingsTable
    {
        [Key]
        public Guid SavingsId { get; set; } = Guid.NewGuid();

        public Guid AccountId { get; set; }

        public string Name { get; set; }

        public DateTime Created { get; set; }

        public bool IsDeleted { get; set; }

        public List<AccountBalance> Balances { get; set; } = new List<AccountBalance>();

        public double MonthlySavingsAmount { get; set; }
    }
}