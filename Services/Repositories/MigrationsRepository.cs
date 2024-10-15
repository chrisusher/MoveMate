using ChrisUsher.MoveMate.API.Services.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace ChrisUsher.MoveMate.API.Services.Repositories;

public class MigrationsRepository
{
    private readonly DatabaseContext _databaseContext;
    private readonly IConfiguration _configuration;

    public DatabaseContext DbContext => _databaseContext;

    public MigrationsRepository(
        DatabaseContext databaseContext,
        IConfiguration configuration)
    {
        _databaseContext = databaseContext;
        _configuration = configuration;
    }

    public async Task<MigrationTable> GetMigrationAsync(string migrationName)
    {
        await using var dbContext = _databaseContext;
        return await dbContext.Migrations.FirstOrDefaultAsync(x => x.MigrationName == migrationName);
    }

    public async Task SetMigrationAppliedAsync(string migrationName, string typeName, DateTime migrationDate)
    {
        await using var dbContext = _databaseContext;

        var migration = new MigrationTable
        {
            MigrationId = Guid.NewGuid(),
            MigrationName = migrationName,
            MigrationTypeName = typeName,
            CreatedOn = DateTime.UtcNow,
            MigrationDate = migrationDate
        };
        dbContext.Migrations.Add(migration);

        await dbContext.SaveChangesAsync();
    }
}
