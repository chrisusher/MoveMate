namespace ChrisUsher.MoveMate.Shared.DTOs.Properties;

public class Property : UpdatePropertyRequest
{
    [JsonPropertyName("propertyId")]
    public Guid PropertyId { get; set; }

    [JsonPropertyName("accountId")]
    public Guid AccountId { get; set; }

    [JsonPropertyName("purchasePrice")]
    public double? PurchasePrice { get; set; }
}