namespace ChrisUsher.MoveMate.Shared.DTOs.Properties;

public class UpdatePropertyRequest : CreatePropertyRequest
{
    [JsonPropertyName("isDeleted")]
    public bool IsDeleted { get; set; }
    
    [JsonPropertyName("created")]
    public DateTime Created { get; set; }

    public Property ToProperty(Guid accountId, Guid propertyId)
    {
        return new Property
        {
            AccountId = accountId,
            PropertyId = propertyId,
            Created = Created,
            IsDeleted = IsDeleted,
            MaxValue = MaxValue,
            MinValue = MinValue,
            Name = Name,
            PropertyType = PropertyType,
            Equity = Equity
        };
    }
}