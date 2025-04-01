using ChrisUsher.MoveMate.API.Services.Reports;
using ChrisUsher.MoveMate.API.Services.StampDuty;
using ChrisUsher.MoveMate.Shared.DTOs.Properties;
using ChrisUsher.MoveMate.Shared.DTOs.Reports;
using ChrisUsher.MoveMate.Shared.DTOs.Properties.StampDuty;

namespace Services.Tests.Reports
{
    [TestFixture]
    public class PropertyViabilityReportTests
    {
        private readonly StampDutyService _stampDutyService;
        private readonly ReportsService _reportService;

        public PropertyViabilityReportTests()
        {
            _stampDutyService = ServiceTestsCommon.Services.GetService<StampDutyService>();
            _reportService = ServiceTestsCommon.Services.GetService<ReportsService>();
            _ = ServiceTestsCommon.DefaultCurrentProperty;
        }

        [Test]
        public async Task GetPropertyViabilityReportAsync_PurchasePriceProvided_StampDutyIgnoresCase()
        {
            var purchasePrice = 410000;
            var request = new PropertyViabilityReportRequest
            {
                CaseType = CaseType.BestCase,
                CurrentPropertySalePrice = 0,
                PurchasePrice = purchasePrice,
            };
            var property = new Property
            {
                MaxValue = purchasePrice,
                MinValue = purchasePrice - 20000,
                PropertyType = PropertyType.ToPurchase,
                AccountId = ServiceTestsCommon.DefaultAccount.AccountId
            };

            var expectedStampDuty = _stampDutyService.CalculateStampDuty(property, new StampDutyRequest
            {
                ResidentialType = PropertyResidentialType.Residential,
                AdditionalProperty = false,
                Location = UKRegionType.Wales,
            }, CaseType.WorstCase);

            var viabilityReport = await _reportService.GetPropertyViabilityReportAsync(property, request);

            var stampDutyCost = viabilityReport.Costs.FirstOrDefault(x => x.Name == "Stamp Duty");

            Assert.That(stampDutyCost, Is.Not.Null, "Stamp Duty cost not found on Property Viability Report.");
            Assert.That(stampDutyCost.FixedCost, Is.EqualTo(expectedStampDuty.Amount), "Stamp Duty cost not correct on Property Viability Report.");
        }
    }
}