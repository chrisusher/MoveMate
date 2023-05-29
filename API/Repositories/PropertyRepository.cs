using ChrisUsher.MoveMate.API.Database;
using ChrisUsher.MoveMate.API.Database.Properties;
using ChrisUsher.MoveMate.Shared.DTOs.Properties;

namespace ChrisUsher.MoveMate.API.Repositories;

public class PropertyRepository
{
    private readonly DatabaseContext _databaseContext;

    public PropertyRepository(DatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    public async Task<PropertyTable> CreatePropertyAsync(Guid accountId, Property property)
    {
        var propertyTable = new PropertyTable
        {
            AccountId = accountId,
            Name = property.Name,
            MaxValue = property.MaxValue,
            MinValue = property.MinValue,
            PropertyType = property.PropertyType
        };

        await _databaseContext.Properties.AddAsync(propertyTable);
        await _databaseContext.SaveChangesAsync();

        return propertyTable;
    }
}