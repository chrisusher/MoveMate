using ChrisUsher.MoveMate.Shared.DTOs.Properties.StampDuty;
using ChrisUsher.MoveMate.Shared.Enums;

namespace ChrisUsher.MoveMate.API.Services.StampDuty;

public static class ScotlandStampDutyService
{
    public static StampDutyResponse CalculateStampDuty(double amount, StampDutyRequest stampDutyRequest)
    {
        if (stampDutyRequest.FirstTime)
        {
            return CalculateFirstTimeStampDuty(amount);
        }
        if (stampDutyRequest.AdditionalProperty)
        {
            return CalculateAdditionalStampDuty(amount);
        }
        if (stampDutyRequest.ResidentialType == PropertyResidentialType.NonResidential)
        {
            throw new Exception("Non-Residential is not yet supported by the Land Transaction Tax Calculator.");
        }
        return CalculateMainTaxStampDuty(amount);
    }

    private static StampDutyResponse CalculateFirstTimeStampDuty(double amount)
    {
        StampDutyBreakdown currentBreakdown;

        var response = new StampDutyResponse
        {
            PurchasePrice = amount
        };

        response.Breakdown.Add(new StampDutyBreakdown
        {
            Amount = amount < 175000 ? amount : 175000,
            TaxDue = 0,
            Percentage = 0,
            Name = "Up to 175,000"
        });

        currentBreakdown = new StampDutyBreakdown
        {
            Name = "Above 175,000 and up to 250,000",
            Percentage = 2,
        };

        double taxableAmount;

        if (amount > 175000)
        {
            taxableAmount = amount - 175000;

            if (taxableAmount > 75000)
            {
                taxableAmount = 75000;
            }

            currentBreakdown.Amount = taxableAmount;
            currentBreakdown.TaxDue = Math.Round(taxableAmount / 100 * currentBreakdown.Percentage, 2);
        }

        response.Breakdown.Add(currentBreakdown);

        currentBreakdown = new StampDutyBreakdown
        {
            Name = "Above 250,000 and up to 325,000",
            Percentage = 5,
        };

        if (amount > 250000)
        {
            taxableAmount = amount - 250000;

            if (taxableAmount > 75000)
            {
                taxableAmount = 75000;
            }

            currentBreakdown.Amount = taxableAmount;
            currentBreakdown.TaxDue = Math.Round(taxableAmount / 100 * currentBreakdown.Percentage, 2);
        }

        response.Breakdown.Add(currentBreakdown);

        currentBreakdown = new StampDutyBreakdown
        {
            Name = "Above 325,000 and up to 750,000",
            Percentage = 10,
        };

        if (amount > 325000)
        {
            taxableAmount = amount - 325000;

            if (taxableAmount > 425000)
            {
                taxableAmount = 425000;
            }

            currentBreakdown.Amount = taxableAmount;
            currentBreakdown.TaxDue = Math.Round(taxableAmount / 100 * currentBreakdown.Percentage, 2);
        }

        response.Breakdown.Add(currentBreakdown);

        currentBreakdown = new StampDutyBreakdown
        {
            Name = "Above 750,000",
            Percentage = 12,
        };

        if (amount > 750000)
        {
            taxableAmount = amount - 750000;

            currentBreakdown.Amount = taxableAmount;
            currentBreakdown.TaxDue = Math.Round(taxableAmount / 100 * currentBreakdown.Percentage, 2);
        }

        response.Breakdown.Add(currentBreakdown);

        return response;
    }

    private static StampDutyResponse CalculateAdditionalStampDuty(double amount)
    {
        var response = new StampDutyResponse
        {
            PurchasePrice = amount
        };

        var taxRequirements = new List<StampDutyBand>
            {
                new ()
                {
                    LowerBound = 0,
                    UpperBound = 145_000,
                    TaxRate = 8
                },
                new ()
                {
                    LowerBound = 145_000,
                    UpperBound = 250_000,
                    TaxRate = 10
                },
                new ()
                {
                    LowerBound = 250_000,
                    UpperBound = 325_000,
                    TaxRate = 13
                },
                new ()
                {
                    LowerBound = 325_000,
                    UpperBound = 750_000,
                    TaxRate = 18
                },
                new ()
                {
                    LowerBound = 750_000,
                    UpperBound = 1_000_000_000,
                    TaxRate = 20
                },
            };

        response.Breakdown = StampDutyService.CalculateStampDutyBreakdown(amount, taxRequirements);

        return response;
    }

    private static StampDutyResponse CalculateMainTaxStampDuty(double amount)
    {
        StampDutyBreakdown currentBreakdown;

        var response = new StampDutyResponse
        {
            PurchasePrice = amount
        };

        response.Breakdown.Add(new StampDutyBreakdown
        {
            Amount = amount < 145000 ? amount : 145000,
            TaxDue = 0,
            Percentage = 0,
            Name = "Up to 145,000"
        });

        currentBreakdown = new StampDutyBreakdown
        {
            Name = "Above 145,000 and up to 250,000",
            Percentage = 2,
        };

        double taxableAmount;

        if (amount > 145000)
        {
            taxableAmount = amount - 145000;

            if (taxableAmount > 105000)
            {
                taxableAmount = 105000;
            }

            currentBreakdown.Amount = taxableAmount;
            currentBreakdown.TaxDue = Math.Round(taxableAmount / 100 * currentBreakdown.Percentage, 2);
        }
        response.Breakdown.Add(currentBreakdown);

        currentBreakdown = new StampDutyBreakdown
        {
            Name = "Above 250,000 and up to 325,000",
            Percentage = 5,
        };

        if (amount > 250000)
        {
            taxableAmount = amount - 250000;

            if (taxableAmount > 75000)
            {
                taxableAmount = 75000;
            }

            currentBreakdown.Amount = taxableAmount;
            currentBreakdown.TaxDue = Math.Round(taxableAmount / 100 * currentBreakdown.Percentage, 2);
        }
        response.Breakdown.Add(currentBreakdown);

        currentBreakdown = new StampDutyBreakdown
        {
            Name = "Above 325,000 and up to 750,000",
            Percentage = 10,
        };

        if (amount > 325000)
        {
            taxableAmount = amount - 325000;

            if (taxableAmount > 425000)
            {
                taxableAmount = 425000;
            }

            currentBreakdown.Amount = taxableAmount;
            currentBreakdown.TaxDue = Math.Round(taxableAmount / 100 * currentBreakdown.Percentage, 2);
        }
        response.Breakdown.Add(currentBreakdown);

        currentBreakdown = new StampDutyBreakdown
        {
            Name = "Above 750,000",
            Percentage = 12,
        };

        if (amount > 750000)
        {
            taxableAmount = amount - 750000;

            currentBreakdown.Amount = taxableAmount;
            currentBreakdown.TaxDue = Math.Round(taxableAmount / 100 * currentBreakdown.Percentage, 2);
        }
        response.Breakdown.Add(currentBreakdown);

        return response;
    }
}