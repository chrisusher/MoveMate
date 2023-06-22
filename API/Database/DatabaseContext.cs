using ChrisUsher.MoveMate.API.Database.Accounts;
using ChrisUsher.MoveMate.API.Database.Costs;
using ChrisUsher.MoveMate.API.Database.Properties;
using ChrisUsher.MoveMate.API.Database.Savings;
using Microsoft.EntityFrameworkCore;

namespace ChrisUsher.MoveMate.API.Database;

public class DatabaseContext : DbContext
{
    public DatabaseContext()
    {
    }

    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<AccountTable>()
            .ToContainer("Accounts")
            .HasPartitionKey("AccountId");

        modelBuilder.Entity<PropertyTable>()
            .ToContainer("Properties")
            .HasPartitionKey("AccountId");

        modelBuilder.Entity<SavingsTable>()
            .ToContainer("Savings")
            .HasPartitionKey("AccountId");

        modelBuilder.Entity<CostTable>()
            .ToContainer("Costs")
            .HasPartitionKey("AccountId");
    }

    public DbSet<AccountTable> Accounts { get; set; }

    public DbSet<CostTable> Costs { get; set; }

    public DbSet<PropertyTable> Properties { get; set; }

    public DbSet<SavingsTable> Savings { get; set; }
}