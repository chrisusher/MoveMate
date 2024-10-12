using ChrisUsher.MoveMate.API.Services.Properties;
using ChrisUsher.MoveMate.Shared.DTOs.Properties;

namespace Services.Tests.Properties;

[TestFixture]
public class UpdatePropertyTests : PropertyServiceTestsBase
{
    private Property _houseFinderProperty;
    private CreatePropertyRequest _houseFinderPropertyRequest;

    public UpdatePropertyTests() : base()
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
    public async Task UpdatePropertyAsync_HouseFinder_WithNotes_ReturnsPropertyWithNotes()
    {
        List<string> notes = ["No Garden.", "Bad Location"];

        var property = await PropertyService.UpdatePropertyNotesAsync(ServiceTestsCommon.DefaultAccount.AccountId, _houseFinderProperty.PropertyId, notes);

        Assert.That(property.Notes, Is.EqualTo(notes), "Property notes not updated on property.");
    }
}
