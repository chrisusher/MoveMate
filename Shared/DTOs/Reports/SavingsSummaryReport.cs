using ChrisUsher.MoveMate.Shared.DTOs.Savings;

namespace ChrisUsher.MoveMate.Shared.DTOs.Reports
{
    public class SavingsSummaryReport
    {
        [JsonPropertyName("totalSavings")]
        public double TotalSavings => Savings.Sum(x => x.CurrentBalance);

        [JsonPropertyName("summaryDate")]
        public DateTime SummaryDate { get; set; }

        [JsonPropertyName("savings")]
        public List<SavingsSummary> Savings { get; set; } = new();
    }
}