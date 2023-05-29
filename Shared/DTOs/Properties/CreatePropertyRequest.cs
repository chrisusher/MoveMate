using ChrisUsher.MoveMate.Shared.Enums;

namespace ChrisUsher.MoveMate.Shared.DTOs.Properties;

public class CreatePropertyRequest
{
    [JsonPropertyName("name")]
    public string Name { get; set; }
    
    [JsonPropertyName("maxValue")]
    public double MaxValue { get; set; }
    
    [JsonPropertyName("minValue")]
    public double MinValue { get; set; }
    
    [JsonConverter(typeof(JsonStringEnumConverter))]
    [JsonPropertyName("propertyType")]
    public PropertyType PropertyType { get; set; } = PropertyType.ToPurchase;

    public Property ToProperty()
    {
        return new Property
        {
            Name = Name,
            MaxValue = MaxValue,
            MinValue = MinValue,
            PropertyType = PropertyType
        };
    }
}