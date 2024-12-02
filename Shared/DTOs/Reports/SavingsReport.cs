using ChrisUsher.MoveMate.Shared.DTOs.Savings;
using ChrisUsher.MoveMate.Shared.Enums;

namespace ChrisUsher.MoveMate.Shared.DTOs.Reports
{
    public class SavingsReport : ReportBase
    {
        [JsonPropertyName("caseType")]
        public CaseType CaseType { get; set; }

        [JsonPropertyName("futureDate")]
        public DateTime? FutureDate { get; set; }

        [JsonPropertyName("savings")]
        public List<ReportSavingsAccount> Savings { get; set; } = new();

        [JsonPropertyName("totalSavings")]
        public double TotalSavings { get; set; } = 0;
    }
}