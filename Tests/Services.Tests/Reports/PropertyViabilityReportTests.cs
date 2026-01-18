using ChrisUsher.MoveMate.API.Services.Reports;
using ChrisUsher.MoveMate.API.Services.StampDuty;
using ChrisUsher.MoveMate.Shared.DTOs.Properties;
using ChrisUsher.MoveMate.Shared.DTOs.Reports;
using ChrisUsher.MoveMate.Shared.DTOs.Properties.StampDuty;
using ChrisUsher.MoveMate.API.Services.Savings;

namespace Services.Tests.Reports;

[TestFixture]
public class PropertyViabilityReportTests
{
    private readonly SavingsService _savingsService;
    private readonly StampDutyService _stampDutyService;
    private readonly ReportsService _reportService;
    private Property _property;
    private PropertyViabilityReport _viabilityReport;

    public PropertyViabilityReportTests()
    {
        _reportService = ServiceTestsCommon.Services.GetService<ReportsService>();
        _ = ServiceTestsCommon.DefaultCurrentProperty;
        _savingsService = ServiceTestsCommon.Services.GetService<SavingsService>();
        _stampDutyService = ServiceTestsCommon.Services.GetService<StampDutyService>();
    }

    [OneTimeSetUp]
    public async Task ClassSetup()
    {
        await _savingsService.CreateSavingsAccountAsync(ServiceTestsCommon.DefaultAccount.AccountId, new()
        {
            Name = "Test Savings",
            InitialBalance = 55_000,
            SavingsRate = 3.0,
            SavingType = SavingType.ISA,
        });

        var purchasePrice = 410000;
        var request = new PropertyViabilityReportRequest
        {
            CaseType = CaseType.BestCase,
            CurrentPropertySalePrice = 200_000,
            PurchasePrice = purchasePrice,
            InterestRate = 4.1,
            Years = 25,
        };
        _property = new Property
        {
            MaxValue = purchasePrice,
            MinValue = purchasePrice - 20000,
            PropertyType = PropertyType.ToPurchase,
            AccountId = ServiceTestsCommon.DefaultAccount.AccountId
        };

        _viabilityReport = await _reportService.GetPropertyViabilityReportAsync(_property, request);
    }

    [Test]
    public async Task GetPropertyViabilityReportAsync_PurchasePriceProvided_StampDutyIgnoresCase()
    {
        var expectedStampDuty = _stampDutyService.CalculateStampDuty(_property, new StampDutyRequest
        {
            ResidentialType = PropertyResidentialType.Residential,
            AdditionalProperty = false,
            Location = UKRegionType.Wales,
        }, CaseType.WorstCase);

        var stampDutyCost = _viabilityReport.Costs.FirstOrDefault(x => x.Name == "Stamp Duty");

        Assert.That(stampDutyCost, Is.Not.Null, "Stamp Duty cost not found on Property Viability Report.");
        Assert.That(stampDutyCost.FixedCost, Is.EqualTo(expectedStampDuty.Amount), "Stamp Duty cost not correct on Property Viability Report.");
    }

    [Test]
    public async Task GetPropertyViabilityReportAsync_MortgagePayments_IncludesMortgageRequired()
    {
        Assert.That(_viabilityReport.MonthlyMortgagePayments, Has.Count.GreaterThan(0), "No mortgage payments found on Property Viability Report.");

        var firstMortgagePayment = _viabilityReport.MonthlyMortgagePayments.First();

        Assert.That(firstMortgagePayment.MortgageRequired, Is.GreaterThan(0), "MortgageRequired is not set on MonthlyMortgagePayment.");
    }

    [Test]
    public async Task GetPropertyViabilityReportAsync_MortgagePayments_MortgageRequired_IsDivisableBy10000()
    {
        Assert.That(_viabilityReport.MonthlyMortgagePayments, Has.Count.GreaterThanOrEqualTo(1), "No mortgage payments found on Property Viability Report.");

        var mortgagePayment = _viabilityReport.MonthlyMortgagePayments[1];

        Assert.That(mortgagePayment.MortgageRequired % 10000, Is.EqualTo(0), "MortgageRequired is not divisable by £10,000 on MonthlyMortgagePayment.");
    }
}