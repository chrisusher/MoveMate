using ChrisUsher.MoveMate.API.Services.StampDuty;
using ChrisUsher.MoveMate.Shared.DTOs.Properties;
using ChrisUsher.MoveMate.Shared.DTOs.Properties.StampDuty;

namespace Services.Tests.StampDuty
{
    [TestFixture]
    public class StampDutyServiceWalesTests
    {
        private readonly StampDutyService _stampDutyService;

        public StampDutyServiceWalesTests()
        {
            _stampDutyService = ServiceTestsCommon.Services.GetService<StampDutyService>();
        }

        [TestCase(429000, 12675)]
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
                Location = UKRegionType.Wales,
            };

            var stampDuty = _stampDutyService.CalculateStampDuty(property, stampDutyRequest, CaseType.WorstCase);

            Assert.That(stampDuty.Amount, Is.EqualTo(expectedStampDuty), "Stamp Duty calculated was not as expected.");
        }

        [TestCase(429000, 9200)]
        [Test]
        public void CalculateStampDuty_NonResidential_CalculatesCorrectAmount(double purchasePrice, double expectedStampDuty)
        {
            var property = new Property
            {
                PurchasePrice = purchasePrice,
                MaxValue = purchasePrice,
                MinValue = purchasePrice
            };
            var stampDutyRequest = new StampDutyRequest
            {
                ResidentialType = PropertyResidentialType.NonResidential,
                Location = UKRegionType.Wales,
            };

            var stampDuty = _stampDutyService.CalculateStampDuty(property, stampDutyRequest, CaseType.WorstCase);

            Assert.That(stampDuty.Amount, Is.EqualTo(expectedStampDuty), "Stamp Duty calculated was not as expected.");
        }

        [TestCase(429000, 33_575)]
        [Test]
        public void CalculateStampDuty_HigherTax_CalculatesCorrectAmount(double purchasePrice, double expectedStampDuty)
        {
            var property = new Property
            {
                PurchasePrice = purchasePrice,
                MaxValue = purchasePrice,
                MinValue = purchasePrice
            };
            var stampDutyRequest = new StampDutyRequest
            {
                ResidentialType = PropertyResidentialType.NonResidential,
                Location = UKRegionType.Wales,
                TaxRate = UKTaxType.HigherTax
            };

            var stampDuty = _stampDutyService.CalculateStampDuty(property, stampDutyRequest, CaseType.WorstCase);

            Assert.That(stampDuty.Amount, Is.EqualTo(expectedStampDuty), "Stamp Duty calculated was not as expected.");
        }
    }
}