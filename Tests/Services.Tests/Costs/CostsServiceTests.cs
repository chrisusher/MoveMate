using ChrisUsher.MoveMate.API.Services.Costs;
using ChrisUsher.MoveMate.Shared.DTOs.Properties;
using ChrisUsher.MoveMate.Shared.DTOs.Costs;

namespace Services.Tests.Costs;

[TestFixture]
public class CostServiceTests
{
    private readonly CostService _costService;

    public CostServiceTests()
    {
        _costService = ServiceTestsCommon.Services.GetService<CostService>();
    }

    [TestCase(270000, 2916)]
    [Test]
    public void CalculateCosts_IncludesVAT(double salePrice, double expectedCost)
    {
        var costs = new List<Cost>
            {
                new Cost
                {
                    Name = "Estate Agent Cost",
                    PercentageOfSale = 0.9,
                    IncludesVAT = false
                }
            };

        var purchaseProperty = new Property
        {
            PropertyType = PropertyType.ToPurchase,
            MaxValue = 429000,
            MinValue = 420000
        };

        var currentProperty = new Property
        {
            MaxValue = salePrice,
            MinValue = salePrice,
            PropertyType = PropertyType.Current,
            Name = "Our House"
        };

        var calculatedCosts = _costService.CalculateCosts(costs, CaseType.MiddleCase, purchaseProperty, currentProperty);

        var estateAgentCost = calculatedCosts.FirstOrDefault(x => x.Name == costs[0].Name);

        Assert.That(estateAgentCost, Is.Not.Null, "Estate Agent cost not found.");

        Assert.That(estateAgentCost.FixedCost, Is.EqualTo(expectedCost), "Expected cost did not contain VAT");
    }

    [Test]
    public async Task CreateCost_IncludesVAT_HasVAT()
    {
        var cost = new Cost
        {
            Name = "Estate Agent Cost",
            PercentageOfSale = 0.9,
            IncludesVAT = true
        };

        var createdCost = await _costService.CreateCostAsync(ServiceTestsCommon.DefaultAccount.AccountId, cost);

        createdCost = await _costService.GetCostAsync(ServiceTestsCommon.DefaultAccount.AccountId, createdCost.CostId);

        Assert.That(createdCost, Is.Not.Null, "Cost was not created");
        Assert.That(createdCost.IncludesVAT, Is.True, "Cost did not include VAT");
    }
}