namespace ChrisUsher.MoveMate.API.Services.Costs
{
    public static class CostCalculations
    {
        private static double _vatRate = 0.2;

        public static double CalculateVAT(double costWithoutVAT)
        {
            return Math.Round(costWithoutVAT + (costWithoutVAT * _vatRate), 2);
        }
    }
}