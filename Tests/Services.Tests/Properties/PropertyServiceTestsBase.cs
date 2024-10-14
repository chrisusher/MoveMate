using ChrisUsher.MoveMate.API.Services.Properties;
using ChrisUsher.MoveMate.Shared.DTOs.Properties;

namespace Services.Tests.Properties;

public class PropertyServiceTestsBase
{
    private readonly PropertyService _propertyService;

    public PropertyServiceTestsBase()
    {
        _propertyService = ServiceTestsCommon.Services.GetService<PropertyService>();
    }

    protected PropertyService PropertyService => _propertyService;

    protected CreatePropertyRequest HouseFinderProperty => new()
    {
        PropertyType = PropertyType.ToPurchase,
        Name = "Test Property",
        MinValue = 300_000,
        MaxValue = 325_000,
        MarketDetails = new()
        {
            Id = ServiceTestsCommon.Faker.Random.Number(1_000_000, 999_999_999),
            Heading = "4 bedroom property for sale",
            SubHeading = "St. Helens Road, Swansea",
            Url = "https://www.onthemarket.com/details/11830919/",
            ListPrice = 300_000,
            FloorSpaceSqFt = 2000,
            Tags = ["Study"]
        }
    };
}