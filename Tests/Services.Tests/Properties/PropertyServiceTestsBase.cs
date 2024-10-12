using ChrisUsher.MoveMate.API.Services.Properties;

namespace Services.Tests.Properties;

public class PropertyServiceTestsBase
{
    private readonly PropertyService _propertyService;

    public PropertyServiceTestsBase()
    {
        _propertyService = ServiceTestsCommon.Services.GetService<PropertyService>();
    }

    protected PropertyService PropertyService => _propertyService;
}