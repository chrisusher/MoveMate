namespace ChrisUsher.MoveMate.Shared.DTOs.Properties;

public class UpdatePropertyRequest : CreatePropertyRequest
{
    [JsonPropertyName("isDeleted")]
    public bool IsDeleted { get; set; }
    
    [JsonPropertyName("created")]
    public DateTime Created { get; set; }
}