using ChrisUsher.MoveMate.API.Services.StampDuty;
using ChrisUsher.MoveMate.Shared.DTOs.Properties;
using ChrisUsher.MoveMate.Shared.DTOs.Properties.StampDuty;

namespace Services.Tests.StampDuty;

[TestFixture]
public class StampDutyServiceScotlandTests
{
    private readonly StampDutyService _stampDutyService;

    public StampDutyServiceScotlandTests()
    {
        _stampDutyService = ServiceTestsCommon.Services.GetService<StampDutyService>();
    }

    [TestCase(429000, 15_650)]
    [TestCase(500000, 22_750)]
    [TestCase(1500000, 137_750)]
    [TestCase(1600000, 149_750)]
    [Test]
    public void CalculateStampDuty_FirstTime_CalculatesCorrectAmount(double purchasePrice, double expectedStampDuty)
    {
        var property = new Property
        {
            PurchasePrice = purchasePrice,
            MaxValue = purchasePrice,
            MinValue = purchasePrice
        };
        var stampDutyRequest = new StampDutyRequest
        {
            ResidentialType = PropertyResidentialType.Residential,
            FirstTime = true,
            Location = UKRegionType.Scotland,
        };

        var stampDuty = _stampDutyService.CalculateStampDuty(property, stampDutyRequest, CaseType.WorstCase);

        Assert.That(stampDuty.Amount, Is.EqualTo(expectedStampDuty), "Stamp Duty calculated was not as expected.");
    }

    [TestCase(429000, 16_250)]
    [TestCase(500000, 23_350)]
    [TestCase(1500000, 138_350)]
    [TestCase(1600000, 150_350)]
    [Test]
    public void CalculateStampDuty_CalculatesCorrectAmount(double purchasePrice, double expectedStampDuty)
    {
        var property = new Property
        {
            PurchasePrice = purchasePrice,
            MaxValue = purchasePrice,
            MinValue = purchasePrice
        };
        var stampDutyRequest = new StampDutyRequest
        {
            ResidentialType = PropertyResidentialType.Residential,
            Location = UKRegionType.Scotland,
        };

        var stampDuty = _stampDutyService.CalculateStampDuty(property, stampDutyRequest, CaseType.WorstCase);

        Assert.That(stampDuty.Amount, Is.EqualTo(expectedStampDuty), "Stamp Duty calculated was not as expected.");
    }

    [Test]
    public void CalculateStampDuty_NonResidential_ThrowsUnsupportedException()
    {
        var property = new Property();

        var stampDutyRequest = new StampDutyRequest
        {
            ResidentialType = PropertyResidentialType.NonResidential,
            Location = UKRegionType.Scotland,
        };

        Assert.Throws<Exception>(() =>
        {
            _stampDutyService.CalculateStampDuty(property, stampDutyRequest, CaseType.WorstCase);
        });
    }

    [TestCase(429000, 50_570)]
    [TestCase(500000, 63_350)]
    [TestCase(1500000, 258_350)]
    [TestCase(1600000, 278_350)]
    [Test]
    public void CalculateStampDuty_AdditionalProperty_CalculatesCorrectAmount(double purchasePrice, double expectedStampDuty)
    {
        var property = new Property
        {
            PurchasePrice = purchasePrice,
            MaxValue = purchasePrice,
            MinValue = purchasePrice
        };
        var stampDutyRequest = new StampDutyRequest
        {
            ResidentialType = PropertyResidentialType.Residential,
            Location = UKRegionType.Scotland,
            AdditionalProperty = true,
        };

        var stampDuty = _stampDutyService.CalculateStampDuty(property, stampDutyRequest, CaseType.WorstCase);

        Assert.That(stampDuty.Amount, Is.EqualTo(expectedStampDuty), "Stamp Duty calculated was not as expected.");
    }
}