namespace ChrisUsher.MoveMate.Shared.DTOs.Reports
{
    public class SavingsOverTimeReport : ReportBase
    {
        public List<SavingsSummaryReport> Summaries { get; set; } = new();
    }
}