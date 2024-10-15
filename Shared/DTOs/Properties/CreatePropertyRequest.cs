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

    [JsonPropertyName("notes")]
    public List<string> Notes { get; set; }

    [JsonPropertyName("marketDetails")]
    public PropertyMarketDetails MarketDetails { get; set; }

    public Property ToProperty()
    {
        var property = new Property
        {
            Name = Name,
            MaxValue = MaxValue,
            MinValue = MinValue,
            PropertyType = PropertyType,
            Notes = Notes
        };

        if(Equity != null)
        {
            property.Equity = Equity;
        }

        if(MarketDetails != null)
        {
            property.MarketDetails = MarketDetails;
        }

        return property;
    }
}