using ChrisUsher.MoveMate.API.Database;
using ChrisUsher.MoveMate.API.Database.Properties;
using ChrisUsher.MoveMate.Shared.DTOs.Properties;
using ChrisUsher.MoveMate.Shared.Enums;
using Microsoft.EntityFrameworkCore;

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

    public async Task<PropertyTable> GetPropertyAsync(Guid accountId, Guid propertyId)
    {
        return await _databaseContext.Properties.FirstOrDefaultAsync(x => x.AccountId == accountId && x.PropertyId == propertyId);
    }

    public async Task<List<PropertyTable>> GetPropertiesAsync(Guid accountId, PropertyType propertyType)
    {
        return await _databaseContext.Properties
            .Where(x => x.AccountId == accountId
                && !x.IsDeleted
                && x.PropertyType == propertyType)
            .ToListAsync();
    }

    public async Task<PropertyTable> UpdatePropertyAsync(Property property)
    {
        var propertyTable = await _databaseContext.Properties.FirstAsync(x => x.PropertyId == property.PropertyId);

        propertyTable.MaxValue = property.MaxValue;
        propertyTable.MinValue = property.MinValue;
        propertyTable.Name = property.Name;
        propertyTable.IsDeleted = property.IsDeleted;
        propertyTable.PropertyType = property.PropertyType;

        _databaseContext.Properties.Update(propertyTable);

        await _databaseContext.SaveChangesAsync();

        return propertyTable;
    }
}