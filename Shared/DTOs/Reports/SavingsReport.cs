using ChrisUsher.MoveMate.Shared.DTOs.Savings;

namespace ChrisUsher.MoveMate.Shared.DTOs.Reports
{
    public class SavingsReport
    {
        [JsonPropertyName("reportDate")]
        public DateTime ReportDate => DateTime.UtcNow;

        [JsonPropertyName("totalSavings")]
        public double TotalSavings { get; set; } = 0;

        [JsonPropertyName("savings")]
        public List<SavingsAccount> Savings { get; set; } = new List<SavingsAccount>();
    }
}