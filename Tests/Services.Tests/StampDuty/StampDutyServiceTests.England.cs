using ChrisUsher.MoveMate.API.Services.StampDuty;
using ChrisUsher.MoveMate.Shared.DTOs.Properties;
using ChrisUsher.MoveMate.Shared.DTOs.Properties.StampDuty;

namespace Services.Tests.StampDuty
{
    [TestFixture]
    public class StampDutyServiceEnglandTests
    {
        private readonly StampDutyService _stampDutyService;

        public StampDutyServiceEnglandTests()
        {
            _stampDutyService = ServiceTestsCommon.Services.GetService<StampDutyService>();
        }

        [TestCase(429000, 6_450)]
        [TestCase(500000, 10_000)]
        [TestCase(1500000, 93_750)]
        [TestCase(1600000, 105_750)]
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
                Location = UKRegionType.England,
            };

            var stampDuty = _stampDutyService.CalculateStampDuty(property, stampDutyRequest, CaseType.WorstCase);

            Assert.That(stampDuty.Amount, Is.EqualTo(expectedStampDuty), "Stamp Duty calculated was not as expected.");
        }

        [TestCase(429000, 11_450)]
        [TestCase(500000, 15_000)]
        [TestCase(1500000, 93_750)]
        [TestCase(1600000, 105_750)]
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
                Location = UKRegionType.England,
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
                Location = UKRegionType.England,
            };

            Assert.Throws<Exception>(() =>
            {
                _stampDutyService.CalculateStampDuty(property, stampDutyRequest, CaseType.WorstCase);
            });
        }

        [TestCase(429000, 32_900)]
        [TestCase(500000, 40_000)]
        [TestCase(1500000, 168_750)]
        [TestCase(1600000, 185_750)]
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
                Location = UKRegionType.England,
                AdditionalProperty = true,
            };

            var stampDuty = _stampDutyService.CalculateStampDuty(property, stampDutyRequest, CaseType.WorstCase);

            Assert.That(stampDuty.Amount, Is.EqualTo(expectedStampDuty), "Stamp Duty calculated was not as expected.");
        }
    }
}