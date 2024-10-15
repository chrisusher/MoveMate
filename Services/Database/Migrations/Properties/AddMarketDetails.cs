using ChrisUsher.MoveMate.API.Services.Database.Properties;
using ChrisUsher.MoveMate.Shared.Interfaces;
using Microsoft.Azure.Cosmos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace ChrisUsher.MoveMate.API.Services.Database.Migrations.Properties
{
    public class AddMarketDetails : IMigration
    {
        private readonly CosmosClient _cosmosClient;
        private readonly DatabaseFacade _database;

        public AddMarketDetails(DatabaseContext databaseContext)
        {
            _cosmosClient = databaseContext.Database.GetCosmosClient();
            _database = databaseContext.Database;
        }

        public string MigrationName => "Properties.AddMarketDetails";

        public DateTime CreatedOn => new(2024, 10, 15);

        public async Task ApplyAsync()
        {
            var container = _cosmosClient.GetContainer(_database.GetCosmosDatabaseId(), "Properties");

            var query = new QueryDefinition("SELECT * from c");
            var iterator = container.GetItemQueryIterator<PropertyTable>(query);

            var patchOperations = new List<PatchOperation>
            {
                PatchOperation.Add<object>("/MarketDetails", null)
            };

            var results = new List<PropertyTable>();

            while (iterator.HasMoreResults)
            {
                var result = await iterator.ReadNextAsync();
                results.AddRange(result.Resource);
            }

            foreach (var result in results)
            {
                try
                {
                    var partitionKey = new PartitionKey(result.PropertyId.ToString());

                    await container.PatchItemAsync<PropertyTable>(id: $"PropertyTable|{result.PropertyId}", partitionKey, patchOperations: patchOperations);
                }
                catch
                { }
            }
        }
    }
}