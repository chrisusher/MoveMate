using ChrisUsher.MoveMate.Shared.Enums;

namespace ChrisUsher.MoveMate.Shared.DTOs.Properties.StampDuty;

public class StampDutyRequest
{
    [JsonPropertyName("location")]
    public UKRegionType Location { get; set; }

    [JsonPropertyName("residentialOrNot")]
    public PropertyResidentialType ResidentialType { get; set; }

    [JsonPropertyName("additionalProperty")]
    public bool AdditionalProperty { get; set; }

    [JsonPropertyName("firstTime")]
    public bool FirstTime { get; set; }

    [JsonPropertyName("taxRate")]
    public UKTaxType TaxRate { get; set; }
}