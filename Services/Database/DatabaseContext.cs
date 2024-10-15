using ChrisUsher.MoveMate.API.Services.Database.Accounts;
using ChrisUsher.MoveMate.API.Services.Database.Costs;
using ChrisUsher.MoveMate.API.Services.Database.Properties;
using ChrisUsher.MoveMate.API.Services.Database.Savings;
using Microsoft.EntityFrameworkCore;
using MongoDB.EntityFrameworkCore;
using MongoDB.EntityFrameworkCore.Extensions;

namespace ChrisUsher.MoveMate.API.Services.Database;

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

#if RELEASE

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

        modelBuilder.Entity<StockTable>()
            .ToContainer("Stocks")
            .HasPartitionKey("SavingsId");

        modelBuilder.Entity<MigrationTable>
            .ToContainer("Migrations")
            .HasPartitionKey("MigrationId");

#elif DEBUG

        modelBuilder.Entity<AccountTable>()
            .ToCollection("Accounts");

        modelBuilder.Entity<PropertyTable>()
            .ToCollection("Properties");

        modelBuilder.Entity<SavingsTable>()
            .ToCollection("Savings");

        modelBuilder.Entity<CostTable>()
            .ToCollection("Costs");

        modelBuilder.Entity<StockTable>()
            .ToCollection("Stocks");
#endif
    }

    public DbSet<AccountTable> Accounts { get; set; }

    public DbSet<CostTable> Costs { get; set; }

    public DbSet<MigrationTable> Migrations { get; set; }

    public DbSet<PropertyTable> Properties { get; set; }

    public DbSet<SavingsTable> Savings { get; set; }

    public DbSet<StockTable> Stocks { get; set; }
}