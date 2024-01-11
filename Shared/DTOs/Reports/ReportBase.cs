namespace ChrisUsher.MoveMate.Shared.DTOs.Reports
{
    public class ReportBase
    {
        [JsonPropertyName("reportDate")]
        public DateTime ReportDate => DateTime.UtcNow;
    }
}