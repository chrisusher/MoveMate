using ChrisUsher.MoveMate.Shared.DTOs.Costs;
using ChrisUsher.MoveMate.Shared.DTOs.Mortgages;
using ChrisUsher.MoveMate.Shared.DTOs.Properties;
using ChrisUsher.MoveMate.Shared.DTOs.Savings;
using ChrisUsher.MoveMate.Shared.Enums;

namespace ChrisUsher.MoveMate.Shared.DTOs.Reports
{
    public class PropertyViabilityReport
    {
        [JsonPropertyName("estimatedSaleDate")]
        public DateTime? SaleDate { get; set; }

        [JsonPropertyName("reportDate")]
        public DateTime ReportDate => DateTime.UtcNow;

        [JsonPropertyName("totalSavings")]
        public double TotalSavings => Math.Round(SavingsAccounts.Sum(x => x.InitialBalance), 2);

        [JsonPropertyName("totalCosts")]
        public double TotalCosts => Math.Round(Costs.Sum(x => x.FixedCost), 2);

        [JsonPropertyName("property")]
        public Property Property { get; set; }

        [JsonPropertyName("equity")]
        public double Equity { get; set; }

        [JsonPropertyName("interestRate")]
        public double InterestRate { get; set; }

        [JsonPropertyName("years")]
        public int Years { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        [JsonPropertyName("case")]
        public CaseType CaseType { get; set; }

        [JsonPropertyName("savings")]
        public List<SavingsAccount> SavingsAccounts { get; set; } = new List<SavingsAccount>();

        [JsonPropertyName("mortgagePayments")]
        public List<MonthlyMortgagePayment> MonthlyMortgagePayments { get; set; } = new List<MonthlyMortgagePayment>();

        [JsonPropertyName("costs")]
        public List<Cost> Costs { get; set; } = new List<Cost>();
    }
}