using ChrisUsher.MoveMate.API.Services.Properties;
using ChrisUsher.MoveMate.Shared.DTOs.Properties;

namespace Services.Tests.Properties;

[TestFixture]
public class GetPropertyTests : PropertyServiceTestsBase
{
    private Property _houseFinderProperty;
    private CreatePropertyRequest _houseFinderPropertyRequest;

    public GetPropertyTests() : base()
    {
    }

    [OneTimeSetUp]
    public async Task ClassSetupAsync()
    {
        _houseFinderPropertyRequest = HouseFinderProperty;

        var properties = await PropertyService.GetPropertiesAsync(ServiceTestsCommon.DefaultAccount.AccountId, PropertyType.ToPurchase);

        if (properties.Count == 0)
        {
            await PropertyService.CreatePropertyAsync(ServiceTestsCommon.DefaultAccount.AccountId, new CreatePropertyRequest
            {
                PropertyType = PropertyType.ToPurchase,
                Name = "Test Property",
                MinValue = 300_000,
                MaxValue = 325_000,
            });
        }

        _houseFinderProperty ??= await PropertyService.CreatePropertyAsync(ServiceTestsCommon.DefaultAccount.AccountId, _houseFinderPropertyRequest);
    }

    [Test]
    public async Task GetPropertiesAsync_ReturnsProperties()
    {
        var properties = await PropertyService.GetPropertiesAsync(ServiceTestsCommon.DefaultAccount.AccountId, PropertyType.ToPurchase);

        Assert.That(properties.Count, Is.GreaterThan(0), "No properties returned.");
    }

    [Test]
    public async Task GetProperty_HouseFinder_ReturnsProperty()
    {
        var property = await PropertyService.GetPropertyAsync(ServiceTestsCommon.DefaultAccount.AccountId, _houseFinderProperty.PropertyId);

        Assert.That(property, Is.Not.Null, "Property not created successfully.");

        Assert.That(property.MarketDetails.Id, Is.EqualTo(_houseFinderPropertyRequest.MarketDetails.Id), "Market details Id not set on property.");
        Assert.That(property.MarketDetails.Url, Is.EqualTo(_houseFinderPropertyRequest.MarketDetails.Url), "Market details Url not set on property.");
        Assert.That(property.MarketDetails.FloorSpaceSqFt, Is.EqualTo(_houseFinderPropertyRequest.MarketDetails.FloorSpaceSqFt), "Market details Floor Space not set on property.");
        Assert.That(property.MarketDetails.Tags, Is.EqualTo(_houseFinderPropertyRequest.MarketDetails.Tags), "Market details Tags not set on property.");
    }
}
