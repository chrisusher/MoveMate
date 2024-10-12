using ChrisUsher.MoveMate.API.Services.Properties;
using ChrisUsher.MoveMate.Shared.DTOs.Properties;

namespace Services.Tests.Properties;

[TestFixture]
public class GetPropertyTests : PropertyServiceTestsBase
{
    public GetPropertyTests() : base()
    {
    }

    [OneTimeSetUp]
    public async Task ClassSetupAsync()
    {
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
    }

    [Test]
    public async Task GetPropertiesAsync_ReturnsProperties()
    {
        var properties = await PropertyService.GetPropertiesAsync(ServiceTestsCommon.DefaultAccount.AccountId, PropertyType.ToPurchase);

        Assert.That(properties.Count, Is.GreaterThan(0), "No properties returned.");
    }
}
