using ChrisUsher.MoveMate.Shared.Enums;

namespace ChrisUsher.MoveMate.Shared.StampDuty
{
    public class StampDutyRequest
    {
        [JsonPropertyName("location")]
        public UKRegionType Location { get; set; }

        [JsonPropertyName("residentialOrNot")]
        public PropertyResidentialType ResidentialType { get; set; }

        [JsonPropertyName("additionalProperty")]
        public bool AdditionalProperty { get; set; }

        [JsonPropertyName("purchasePrice")]
        public double PurchasePrice { get; set; }
    }
}