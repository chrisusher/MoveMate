using ChrisUsher.MoveMate.API.Services.Repositories;
using ChrisUsher.MoveMate.Shared.Interfaces;
using Microsoft.Extensions.Logging;

namespace ChrisUsher.MoveMate.API.Services;

public class MigrationsService
{
    private readonly MigrationsRepository _migrationsRepository;
    private readonly ILogger<MigrationsService> _logger;

    public MigrationsService(
        MigrationsRepository repository,
        ILoggerFactory loggerFactory)
    {
        _migrationsRepository = repository;
        _logger = loggerFactory.CreateLogger<MigrationsService>();
    }

    public async Task ApplyMigrationsAsync()
    {
        var migrations = GetMigrations();

        foreach (var migration in migrations)
        {
            var typeName = migration.GetType().FullName;

            if (string.IsNullOrEmpty(migration.MigrationName))
            {
                _logger.LogWarning("Skipping migration as {typeName} does not have a name", typeName);
                continue;
            }

            try
            {
                if (await IsMigrationAppliedAsync(migration.MigrationName))
                {
                    _logger.LogInformation("Skipping migration '{migration.MigrationName}' as it has already been applied", migration.MigrationName);
                    continue;
                }

                await migration.ApplyAsync();

                await SetMigrationAppliedAsync(migration.MigrationName, typeName, migration.CreatedOn);

                _logger.LogInformation("Migration '{migration.MigrationName}' applied successfully!", migration.MigrationName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unable to apply Migration '{migration.MigrationName}' due to errors.", migration.MigrationName);
            }
        }
    }

    private async Task SetMigrationAppliedAsync(string migrationName, string typeName, DateTime createdOn)
    {
        await _migrationsRepository.SetMigrationAppliedAsync(migrationName, typeName, createdOn);
    }

    private async Task<bool> IsMigrationAppliedAsync(string migrationName)
    {
        try
        {
            return await _migrationsRepository.GetMigrationAsync(migrationName) != null;
        }
        catch
        {
            return false;
        }
    }

    private List<IMigration> GetMigrations()
    {
        var migrations = new List<IMigration>();

        var migrationTypes = AppDomain.CurrentDomain.GetAssemblies()
                                .SelectMany(x => x.GetTypes())
                                .Where(x => typeof(IMigration).IsAssignableFrom(x) && x.IsClass);

        var migrationConstructors = new List<object>
        {
            _migrationsRepository.DbContext
        }.ToArray();

        foreach (var migrationType in migrationTypes)
        {
            migrations.Add((IMigration)Activator.CreateInstance(migrationType, migrationConstructors));
        }

        migrations = migrations
            .OrderBy(x => x.CreatedOn)
            .ToList();

        return migrations;
    }
}