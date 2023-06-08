using ChrisUsher.MoveMate.API.Repositories;
using ChrisUsher.MoveMate.Shared.DTOs.Properties;
using ChrisUsher.MoveMate.Shared.Enums;

namespace ChrisUsher.MoveMate.API.Services.Properties;

public class PropertyService
{
    private readonly PropertyRepository _propertyRepo;

    public PropertyService(PropertyRepository propertyRepository)
    {
        _propertyRepo = propertyRepository;
    }

    public async Task<Property> CreatePropertyAsync(Guid accountId, CreatePropertyRequest request)
    {
        var property = request.ToProperty();
        
        var propertyTable = await _propertyRepo.CreatePropertyAsync(accountId, property);

        return propertyTable.ToProperty();
    }

    public async Task<Property> GetPropertyAsync(Guid accountId, Guid propertyId)
    {
        var propertyTable = await _propertyRepo.GetPropertyAsync(accountId, propertyId);

        return propertyTable == null
            ? throw new DataNotFoundException($"No Property found in Account '{accountId}' with Property Id '{propertyId}'")
            : propertyTable.ToProperty();
    }

    public async Task<List<Property>> GetPropertiesAsync(Guid accountId, PropertyType propertyType)
    {
        var properties = await _propertyRepo.GetPropertiesAsync(accountId, propertyType);

        return properties
            .Select(x => x.ToProperty())
            .ToList();
    }

    public async Task<Property> UpdatePropertyAsync(Guid accountId, Guid propertyId, UpdatePropertyRequest request)
    {
        var property = request.ToProperty(accountId, propertyId);
        
        var propertyTable = await _propertyRepo.UpdatePropertyAsync(property);

        return propertyTable.ToProperty();
    }
}