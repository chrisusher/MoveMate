namespace ChrisUsher.MoveMate.Shared.DTOs.Properties.StampDuty;

public class StampDutyResponse
{
    [JsonPropertyName("calculationDate")]
    public DateTime CalculationDate => DateTime.UtcNow;

    [JsonPropertyName("purchasePrice")]
    public double PurchasePrice { get; set; }

    [JsonPropertyName("stampDuty")]
    public double Amount => Math.Round(Breakdown.Sum(x => x.TaxDue), 2);

    [JsonPropertyName("breakdown")]
    public List<StampDutyBreakdown> Breakdown { get; set; } = new List<StampDutyBreakdown>();
}