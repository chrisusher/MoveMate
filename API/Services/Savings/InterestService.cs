using ChrisUsher.MoveMate.Shared.DTOs.Savings;

namespace ChrisUsher.MoveMate.API.Services.Savings
{
    public class InterestService
    {
        public SavingsInterestBreakdown CalculateInterest(SavingsAccount account, DateTime endDate)
        {
            var response = new SavingsInterestBreakdown();

            var date = account.Created;
            var interest = 0.0;
            var principal = account.InitialBalance;

            while (date < endDate)
            {
                var dailyInterest = GetDailyInterest(principal, account.SavingsRate);
                interest += dailyInterest;
                principal += dailyInterest;

                date = date.AddDays(1);

                if (date.DayOfWeek == DayOfWeek.Monday)
                {
                    principal += account.MonthlySavingsAmount / 4;
                }
                if (date.Day == 1)
                {
                    response.MonthlyBreakdown.Add(new MonthlyInterestBreakdown
                    {
                        Deposits = account.MonthlySavingsAmount,
                        Interest = Math.Round(interest, 2),
                        Balance = Math.Round(principal, 2),
                        Date = date.AddDays(-1).Date
                    });

                    interest = 0.0;
                }
            }

            if (response.MonthlyBreakdown.Count == 0)
            {
                response.MonthlyBreakdown.Add(new MonthlyInterestBreakdown
                {
                    Deposits = account.MonthlySavingsAmount,
                    Interest = Math.Round(interest, 2),
                    Balance = Math.Round(principal, 2),
                    Date = endDate.Date
                });
            }

            return response;
        }

        private double GetDailyInterest(double principal, double savingsRate)
        {
            return principal * (savingsRate / 100) * 0.002739726027;
        }
    }
}