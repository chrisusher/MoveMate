using ChrisUsher.MoveMate.API.Services.StampDuty;
using ChrisUsher.MoveMate.Shared.DTOs.Properties;
using ChrisUsher.MoveMate.Shared.DTOs.StampDuty;

namespace Services.Tests.StampDuty
{
    [TestFixture]
    public class StampDutyServiceTests
    {
        private readonly StampDutyService _stampDutyService;

        public StampDutyServiceTests()
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
    }
}