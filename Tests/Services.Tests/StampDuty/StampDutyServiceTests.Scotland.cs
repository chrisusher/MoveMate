using ChrisUsher.MoveMate.API.Services.StampDuty;
using ChrisUsher.MoveMate.Shared.DTOs.Properties;
using ChrisUsher.MoveMate.Shared.DTOs.StampDuty;

namespace Services.Tests.StampDuty
{
    [TestFixture]
    public class StampDutyServiceScotlandTests
    {
        private readonly StampDutyService _stampDutyService;

        public StampDutyServiceScotlandTests()
        {
            _stampDutyService = ServiceTestsCommon.Services.GetService<StampDutyService>();
        }

        [TestCase(429000, 15650)]
        [TestCase(500000, 22750)]
        [TestCase(1500000, 137750)]
        [TestCase(1600000, 149750)]
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

        [TestCase(429000, 16250)]
        [TestCase(500000, 23350)]
        [TestCase(1500000, 138350)]
        [TestCase(1600000, 150350)]
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

        [TestCase(429000, 41990)]
        [TestCase(500000, 53350)]
        [TestCase(1500000, 228350)]
        [TestCase(1600000, 246350)]
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
}