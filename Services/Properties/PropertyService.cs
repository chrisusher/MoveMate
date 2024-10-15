using ChrisUsher.MoveMate.API.Services.Repositories;
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

        if (property.PropertyType == PropertyType.ToPurchase)
        {
            if (property.MarketDetails != null)
            {
                try
                {
                    var houseFinderProperty = await GetHouseFinderPropertyAsync(accountId, property.MarketDetails.Id);
                    throw new PropertyAlreadyExistsException($"Property with Id '{property.MarketDetails.Id}' already exists in Account '{accountId}'");
                }
                catch (DataNotFoundException)
                {
                }
            }
        }

        var propertyTable = await _propertyRepo.CreatePropertyAsync(accountId, property);

        return propertyTable.ToProperty();
    }

    public async Task<Property> GetHouseFinderPropertyAsync(Guid accountId, long houseFinderId)
    {
        var property = await _propertyRepo.GetHouseFinderPropertyAsync(accountId, houseFinderId);

        return property == null
            ? throw new DataNotFoundException($"No Property found in Account '{accountId}' with HouseFinder Id '{houseFinderId}'")
            : property.ToProperty();
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

    public async Task<Property> UpdatePropertyNotesAsync(Guid accountId, Guid propertyId, List<string> notes)
    {
        var property = await GetPropertyAsync(accountId, propertyId);
        property.Notes = notes;

        return await UpdatePropertyAsync(accountId, propertyId, property);
    }

    public async Task<Property> GetCurrentPropertyAsync(Guid accountId)
    {
        var propertyTable = await _propertyRepo.GetPropertyAsync(accountId, PropertyType.Current);

        return propertyTable == null
            ? throw new DataNotFoundException($"No Current Property found in Account '{accountId}'")
            : propertyTable.ToProperty();
    }
}