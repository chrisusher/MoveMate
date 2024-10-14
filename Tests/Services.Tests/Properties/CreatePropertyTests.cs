using ChrisUsher.MoveMate.API.Services.Properties;
using ChrisUsher.MoveMate.Shared.DTOs.Properties;
using ChrisUsher.MoveMate.Shared.Exceptions;

namespace Services.Tests.Properties;

[TestFixture]
public class CreatePropertyServiceTests : PropertyServiceTestsBase
{
    public CreatePropertyServiceTests() : base()
    {
    }

    [OneTimeSetUp]
    public void ClassSetup()
    {
    }

    [Test]
    public async Task CreatePropertyAsync_Current_ReturnsProperty()
    {
        var property = await PropertyService.CreatePropertyAsync(ServiceTestsCommon.DefaultAccount.AccountId, new CreatePropertyRequest
        {
            PropertyType = PropertyType.Current,
            Name = "Test Property",
            MinValue = 200_000,
            MaxValue = 300_000,
        });

        Assert.That(property, Is.Not.Null, "Property not created successfully.");
        Assert.That(property.PropertyId, Is.Not.EqualTo(Guid.Empty), "PropertyId not set on property.");
        Assert.That(property.Name, Is.EqualTo("Test Property"));
        Assert.That(property.PropertyType, Is.EqualTo(PropertyType.Current));
        Assert.That(property.MinValue, Is.EqualTo(200_000));
        Assert.That(property.MaxValue, Is.EqualTo(300_000));
    }

    [Test]
    public async Task CreatePropertyAsync_ToPurchase_ReturnsProperty()
    {
        var property = await PropertyService.CreatePropertyAsync(ServiceTestsCommon.DefaultAccount.AccountId, new CreatePropertyRequest
        {
            PropertyType = PropertyType.ToPurchase,
            Name = "Test Property",
            MinValue = 300_000,
            MaxValue = 325_000,
        });

        Assert.That(property, Is.Not.Null, "Property not created successfully.");
        Assert.That(property.PropertyId, Is.Not.EqualTo(Guid.Empty), "PropertyId not set on property.");
        Assert.That(property.Name, Is.EqualTo("Test Property"));
        Assert.That(property.PropertyType, Is.EqualTo(PropertyType.ToPurchase));
        Assert.That(property.MinValue, Is.EqualTo(300_000));
        Assert.That(property.MaxValue, Is.EqualTo(325_000));
    }

    [Test]
    public async Task CreatePropertyAsync_HouseFinder_ReturnsProperty()
    {
        var houseFinderProperty = HouseFinderProperty;

        var property = await PropertyService.CreatePropertyAsync(ServiceTestsCommon.DefaultAccount.AccountId, houseFinderProperty);

        Assert.That(property, Is.Not.Null, "Property not created successfully.");

        Assert.That(property.MarketDetails.Id, Is.EqualTo(houseFinderProperty.MarketDetails.Id), "Market details Id not set on property.");
        Assert.That(property.MarketDetails.Url, Is.EqualTo(houseFinderProperty.MarketDetails.Url), "Market details Url not set on property.");
        Assert.That(property.MarketDetails.FloorSpaceSqFt, Is.EqualTo(houseFinderProperty.MarketDetails.FloorSpaceSqFt), "Market details Floor Space not set on property.");
        Assert.That(property.MarketDetails.Tags, Is.EqualTo(houseFinderProperty.MarketDetails.Tags), "Market details Tags not set on property.");
    }

    [Test]
    public async Task CreatePropertyAsync_HouseFinder_AlreadyExists_ThrowsAlreadyExistsException()
    {
        var houseFinderProperty = HouseFinderProperty;

        await PropertyService.CreatePropertyAsync(ServiceTestsCommon.DefaultAccount.AccountId, houseFinderProperty);

        Assert.ThrowsAsync<PropertyAlreadyExistsException>(async () =>
        {
            await PropertyService.CreatePropertyAsync(ServiceTestsCommon.DefaultAccount.AccountId, houseFinderProperty);
        });
    }
}
