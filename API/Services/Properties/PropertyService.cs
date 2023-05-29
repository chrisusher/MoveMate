using ChrisUsher.MoveMate.API.Repositories;
using ChrisUsher.MoveMate.Shared.DTOs.Properties;

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
}