using ChrisUsher.MoveMate.Shared.Enums;

namespace ChrisUsher.MoveMate.Shared.DTOs.Reports
{
    public class PropertyViabilityReportRequest : ReportBase
    {
        [JsonPropertyName("interestRate")]
        public double InterestRate { get; set; }

        [JsonPropertyName("years")]
        public int Years { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        [JsonPropertyName("case")]
        public CaseType CaseType { get; set; } = CaseType.MiddleCase;

        [JsonPropertyName("currentPropertySalePrice")]
        public double? CurrentPropertySalePrice { get; set; }

        [JsonPropertyName("purchasePrice")]
        public double? PurchasePrice { get; set; }
    }
}