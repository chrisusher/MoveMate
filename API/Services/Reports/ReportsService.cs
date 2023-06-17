using ChrisUsher.MoveMate.API.Services.Accounts;
using ChrisUsher.MoveMate.API.Services.Mortgages;
using ChrisUsher.MoveMate.API.Services.Properties;
using ChrisUsher.MoveMate.API.Services.Savings;
using ChrisUsher.MoveMate.Shared.DTOs.Mortgages;
using ChrisUsher.MoveMate.Shared.DTOs.Properties;
using ChrisUsher.MoveMate.Shared.DTOs.Reports;
using ChrisUsher.MoveMate.Shared.DTOs.Savings;
using ChrisUsher.MoveMate.Shared.Enums;

namespace ChrisUsher.MoveMate.API.Services.Reports;

public class ReportsService
{
    private readonly AccountService _accountService;
    private readonly PropertyService _propertyService;
    private readonly SavingsService _savingsService;
    private readonly InterestService _interestService;
    private readonly MortgagePaymentService _mortgagePaymentService;

    public ReportsService(
        AccountService accountService,
        PropertyService propertyService,
        SavingsService savingsService,
        InterestService interestService,
        MortgagePaymentService mortgagePaymentService
    )
    {
        _accountService = accountService;
        _propertyService = propertyService;
        _savingsService = savingsService;
        _interestService = interestService;
        _mortgagePaymentService = mortgagePaymentService;
    }

    public async Task<PropertyViabilityReport> GetPropertyViabilityReport(Property property, double interestRate, CaseType caseType, int years)
    {
        if(property == null)
        {
            throw new DataNotFoundException("Property passed to PropertyViabilityReport was not found.");
        }
        var account = await _accountService.GetAccountAsync(property.AccountId);
        var currentProperty = await _propertyService.GetCurrentPropertyAsync(property.AccountId);

        var report = new PropertyViabilityReport
        {
            SaleDate = account.EstimatedSaleDate,
            Property = property,
            InterestRate = interestRate,
            Years = years,
            CaseType = caseType
        };

        var savings = await _savingsService.GetSavingsAccountsAsync(property.AccountId);

        report.SavingsAccounts = CalculateSavings(savings, caseType, report.SaleDate);
        report.TotalSavings = Math.Round(report.SavingsAccounts.Sum(s => s.InitialBalance), 2);

        switch(caseType)
        {
            case CaseType.BestCase:
                report.Equity = currentProperty.MaxValue - currentProperty.Equity.RemainingMortgage;

                report.MonthlyMortgagePayments = CalculateMonthlyPayments(property.MinValue, report.TotalSavings, report.Equity, interestRate, years);
                break;
            
            case CaseType.MiddleCase:
                report.Equity = Math.Round((currentProperty.MaxValue + currentProperty.MaxValue) / 2 - currentProperty.Equity.RemainingMortgage, 2);

                report.MonthlyMortgagePayments = CalculateMonthlyPayments(Math.Round((property.MaxValue + property.MinValue) / 2, 2), report.TotalSavings, report.Equity, interestRate, years);
                break;
            
            case CaseType.WorstCase:
                report.Equity = currentProperty.MinValue - currentProperty.Equity.RemainingMortgage;

                report.MonthlyMortgagePayments = CalculateMonthlyPayments(property.MaxValue, report.TotalSavings, report.Equity, interestRate, years);
                break;
        }

        return report;
    }

    private List<MonthlyMortgagePayment> CalculateMonthlyPayments(double purchasePrice, double totalSavings, double equity, double interestRate, int years)
    {
        var monthlyPayments = new List<MonthlyMortgagePayment>();

        var currentSavingAmount = 0;

        while(currentSavingAmount < totalSavings)
        {
            var deposit = equity + currentSavingAmount;

            monthlyPayments.Add(new MonthlyMortgagePayment
            {
                CashRequired = currentSavingAmount,
                MonthlyPayment = _mortgagePaymentService.CalculateMonthlyMortgagePayment(purchasePrice - deposit, interestRate, years),
                TotalDeposit = deposit
            });

            currentSavingAmount += 10000;
        }

        return monthlyPayments;
    }

    private List<SavingsAccount> CalculateSavings(List<SavingsAccount> savings, CaseType caseType, DateTime? saleDate)
    {
        var calculatedSavings = new List<SavingsAccount>();

        foreach(var saving in savings)
        {
            if(saving.Fluctuations != null)
            {
                switch(caseType)
                {
                    case CaseType.BestCase:
                        calculatedSavings.Add(new SavingsAccount
                        {
                            Name = saving.Name,
                            InitialBalance = saving.Fluctuations.BestCase,
                            SavingsRate = saving.SavingsRate
                        });
                        break;

                    case CaseType.MiddleCase:
                        calculatedSavings.Add(new SavingsAccount
                        {
                            Name = saving.Name,
                            InitialBalance = Math.Round((saving.Fluctuations.BestCase + saving.Fluctuations.WorstCase) / 2, 2),
                            SavingsRate = saving.SavingsRate
                        });
                        break;
                    
                    case CaseType.WorstCase:
                        calculatedSavings.Add(new SavingsAccount
                        {
                            Name = saving.Name,
                            InitialBalance = saving.Fluctuations.WorstCase,
                            SavingsRate = saving.SavingsRate
                        });
                        break;
                }
                continue;
            }
        
            calculatedSavings.Add(new SavingsAccount
            {
                Name = saving.Name,
                SavingsRate = saving.SavingsRate,
                InitialBalance = _interestService.CalculateInterest(saving, saleDate).NewTotal
            });
        }

        return calculatedSavings;
    }
}