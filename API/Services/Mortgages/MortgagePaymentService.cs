using MortgageCalcHelperLib;

namespace ChrisUsher.MoveMate.API.Services.Mortgages;

public class MortgagePaymentService
{
    public MortgagePaymentService()
    {
    }

    public double CalculateMonthlyMortgagePayment(double loanAmount, double interestRate, int years)
    {
        return MortgageCalcHelper.ComputeMonthlyPayment(loanAmount, years, interestRate);
    }
}