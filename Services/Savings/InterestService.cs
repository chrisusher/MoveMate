using ChrisUsher.MoveMate.Shared.DTOs.Savings;

namespace ChrisUsher.MoveMate.API.Services.Savings
{
    public class InterestService
    {
        public SavingsInterestBreakdown CalculateInterest(SavingsAccount account, DateTime? endDate)
        {
            if(endDate == null)
            {
                endDate = DateTime.UtcNow;
            }

            var response = new SavingsInterestBreakdown();

            var date = account.Created;
            var interest = 0.0;
            var principal = account.InitialBalance;

            if(account.Balances.Any())
            {
                var latestBalance = account.Balances
                    .OrderBy(x => x.Created)
                    .Last();

                date = latestBalance.Created;
                principal = latestBalance.Balance;
            }

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
                    Date = endDate.Value.Date
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