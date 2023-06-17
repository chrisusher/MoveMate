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

    [JsonPropertyName("equity")]
    public Equity Equity { get; set; }

    public Property ToProperty()
    {
        var property = new Property
        {
            Name = Name,
            MaxValue = MaxValue,
            MinValue = MinValue,
            PropertyType = PropertyType
        };

        if(Equity != null)
        {
            property.Equity = Equity;
        }

        return property;
    }
}