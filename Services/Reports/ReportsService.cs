using ChrisUsher.MoveMate.API.Services.Accounts;
using ChrisUsher.MoveMate.API.Services.Costs;
using ChrisUsher.MoveMate.API.Services.Mortgages;
using ChrisUsher.MoveMate.API.Services.Properties;
using ChrisUsher.MoveMate.API.Services.Savings;
using ChrisUsher.MoveMate.API.Services.StampDuty;
using ChrisUsher.MoveMate.Shared.DTOs.Costs;
using ChrisUsher.MoveMate.Shared.DTOs.Mortgages;
using ChrisUsher.MoveMate.Shared.DTOs.Properties;
using ChrisUsher.MoveMate.Shared.DTOs.Reports;
using ChrisUsher.MoveMate.Shared.DTOs.Savings;
using ChrisUsher.MoveMate.Shared.DTOs.StampDuty;
using ChrisUsher.MoveMate.Shared.Enums;

namespace ChrisUsher.MoveMate.API.Services.Reports;

public class ReportsService
{
    private readonly AccountService _accountService;
    private readonly PropertyService _propertyService;
    private readonly SavingsService _savingsService;
    private readonly InterestService _interestService;
    private readonly MortgagePaymentService _mortgagePaymentService;
    private readonly CostService _costsService;
    private readonly StampDutyService _stampDutyService;

    public ReportsService(
        AccountService accountService,
        PropertyService propertyService,
        SavingsService savingsService,
        InterestService interestService,
        MortgagePaymentService mortgagePaymentService,
        CostService costsService,
        StampDutyService stampDutyService
    )
    {
        _accountService = accountService;
        _propertyService = propertyService;
        _savingsService = savingsService;
        _interestService = interestService;
        _mortgagePaymentService = mortgagePaymentService;
        _costsService = costsService;
        _stampDutyService = stampDutyService;
    }

    public async Task<PropertyViabilityReport> GetPropertyViabilityReport(Property property, PropertyViabilityReportRequest request)
    {
        if (property == null)
        {
            throw new DataNotFoundException("Property passed to PropertyViabilityReport was not found.");
        }
        var account = await _accountService.GetAccountAsync(property.AccountId);
        var currentProperty = await _propertyService.GetCurrentPropertyAsync(property.AccountId);

        var requestJson = JsonSerializer.Serialize(request);
        var report = JsonSerializer.Deserialize<PropertyViabilityReport>(requestJson);

        report.SaleDate = account.EstimatedSaleDate;
        report.Property = property;

        var purchasePrice = 0.0;

        if (request.CurrentPropertySalePrice.HasValue)
        {
            report.Equity = request.CurrentPropertySalePrice.Value - currentProperty.Equity.RemainingMortgage;
        }
        if (request.PurchasePrice.HasValue)
        {
            purchasePrice = request.PurchasePrice.Value;
        }

        var savings = await _savingsService.GetSavingsAccountsAsync(property.AccountId);
        var costs = await _costsService.GetCostsAsync(property.AccountId);

        report.SavingsAccounts = CalculateSavings(savings, request.CaseType, report.SaleDate);
        report.Costs = _costsService.CalculateCosts(costs, request.CaseType, property, currentProperty);

        switch (request.CaseType)
        {
            case CaseType.BestCase:
                if (report.Equity == 0)
                {
                    report.Equity = currentProperty.MaxValue - currentProperty.Equity.RemainingMortgage;
                }
                if (purchasePrice == 0)
                {
                    purchasePrice = property.MinValue;
                }

                report.MonthlyMortgagePayments = CalculateMonthlyPayments(purchasePrice, report.TotalSavings - report.TotalCosts, report.Equity, request.InterestRate, request.Years);
                break;

            case CaseType.MiddleCase:
                if (report.Equity == 0)
                {
                    report.Equity = Math.Round((currentProperty.MaxValue + currentProperty.MinValue) / 2 - currentProperty.Equity.RemainingMortgage, 2);
                }
                if (purchasePrice == 0)
                {
                    purchasePrice = Math.Round((property.MaxValue + property.MinValue) / 2, 2);
                }

                report.MonthlyMortgagePayments = CalculateMonthlyPayments(purchasePrice, report.TotalSavings - report.TotalCosts, report.Equity, request.InterestRate, request.Years);
                break;

            case CaseType.WorstCase:
                if (report.Equity == 0)
                {
                    report.Equity = currentProperty.MinValue - currentProperty.Equity.RemainingMortgage;
                }
                if (purchasePrice == 0)
                {
                    purchasePrice = property.MaxValue;
                }

                report.MonthlyMortgagePayments = CalculateMonthlyPayments(purchasePrice, report.TotalSavings - report.TotalCosts, report.Equity, request.InterestRate, request.Years);
                break;
        }

        report.Property.PurchasePrice = purchasePrice;
        return report;
    }

    private List<MonthlyMortgagePayment> CalculateMonthlyPayments(double purchasePrice, double totalSavings, double equity, double interestRate, int years)
    {
        var monthlyPayments = new List<MonthlyMortgagePayment>();

        var currentSavingAmount = 0;

        while (currentSavingAmount < totalSavings)
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

        foreach (var saving in savings)
        {
            if (saving.Fluctuations != null)
            {
                switch (caseType)
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