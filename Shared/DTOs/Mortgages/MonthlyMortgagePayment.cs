namespace ChrisUsher.MoveMate.Shared.DTOs.Mortgages
{
    public class MonthlyMortgagePayment
    {
        [JsonPropertyName("monthlyPayment")]
        public double MonthlyPayment { get; set; }

        [JsonPropertyName("totalDeposit")]
        public double TotalDeposit { get; set; }

        [JsonPropertyName("cashRequired")]
        public double CashRequired { get; set; }
    }
}