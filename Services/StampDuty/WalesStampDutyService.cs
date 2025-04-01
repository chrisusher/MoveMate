using ChrisUsher.MoveMate.Shared.DTOs.Properties.StampDuty;
using ChrisUsher.MoveMate.Shared.Enums;

namespace ChrisUsher.MoveMate.API.Services.StampDuty;

public static class WalesStampDutyService
{
    public static StampDutyResponse CalculateStampDuty(double amount, StampDutyRequest stampDutyRequest)
    {
        if (stampDutyRequest.TaxRate == UKTaxType.HigherTax)
        {
            return CalculateHigherTaxStampDuty(amount);
        }
        if (stampDutyRequest.ResidentialType == PropertyResidentialType.NonResidential)
        {
            return CalculateNonResidentialStampDuty(amount);
        }
        return CalculateMainTaxStampDuty(amount);
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
            Amount = amount < 225000 ? amount : 225000,
            TaxDue = 0,
            Percentage = 0,
            Name = "Up to 225,000"
        });

        currentBreakdown = new StampDutyBreakdown
        {
            Name = "Above 225,000 and up to 400,000",
            Percentage = 6,
        };

        double taxableAmount;

        if (amount > 225000)
        {
            taxableAmount = amount - 225000;

            if (taxableAmount > 175000)
            {
                taxableAmount = 175000;
            }

            currentBreakdown.Amount = taxableAmount;
            currentBreakdown.TaxDue = Math.Round(taxableAmount / 100 * currentBreakdown.Percentage, 2);
        }
        response.Breakdown.Add(currentBreakdown);

        currentBreakdown = new StampDutyBreakdown
        {
            Name = "Above 400,000 and up to 750,000",
            Percentage = 7.5,
        };

        if (amount > 400000)
        {
            taxableAmount = amount - 400000;

            if (taxableAmount > 350000)
            {
                taxableAmount = 350000;
            }

            currentBreakdown.Amount = taxableAmount;
            currentBreakdown.TaxDue = Math.Round(taxableAmount / 100 * currentBreakdown.Percentage, 2);
        }
        response.Breakdown.Add(currentBreakdown);

        currentBreakdown = new StampDutyBreakdown
        {
            Name = "Above 750,000 and up to 1,500,000",
            Percentage = 10,
        };

        if (amount > 750000)
        {
            taxableAmount = amount - 750000;

            if (taxableAmount > 750000)
            {
                taxableAmount = 750000;
            }

            currentBreakdown.Amount = taxableAmount;
            currentBreakdown.TaxDue = Math.Round(taxableAmount / 100 * currentBreakdown.Percentage, 2);
        }
        response.Breakdown.Add(currentBreakdown);

        currentBreakdown = new StampDutyBreakdown
        {
            Name = "Above 1,500,000",
            Percentage = 12,
        };

        if (amount > 1500000)
        {
            taxableAmount = amount - 1500000;

            currentBreakdown.Amount = taxableAmount;
            currentBreakdown.TaxDue = Math.Round(taxableAmount / 100 * currentBreakdown.Percentage, 2);
        }
        response.Breakdown.Add(currentBreakdown);

        return response;
    }

    private static StampDutyResponse CalculateHigherTaxStampDuty(double amount)
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
                    UpperBound = 180_000,
                    TaxRate = 5
                },
                new ()
                {
                    LowerBound = 180_000,
                    UpperBound = 250_000,
                    TaxRate = 8.5
                },
                new ()
                {
                    LowerBound = 250_000,
                    UpperBound = 400_000,
                    TaxRate = 10
                },
                new ()
                {
                    LowerBound = 400_000,
                    UpperBound = 750_000,
                    TaxRate = 12.5
                },
                new ()
                {
                    LowerBound = 750_000,
                    UpperBound = 1_500_000,
                    TaxRate = 15
                },
                new ()
                {
                    LowerBound = 1_500_000,
                    UpperBound = 1_000_000_000,
                    TaxRate = 17
                },
            };

        response.Breakdown = StampDutyService.CalculateStampDutyBreakdown(amount, taxRequirements);

        return response;
    }

    private static StampDutyResponse CalculateNonResidentialStampDuty(double amount)
    {
        StampDutyBreakdown currentBreakdown;

        var response = new StampDutyResponse
        {
            PurchasePrice = amount
        };

        response.Breakdown.Add(new StampDutyBreakdown
        {
            Amount = amount < 225000 ? amount : 225000,
            TaxDue = 0,
            Percentage = 0,
            Name = "Up to 225,000"
        });

        currentBreakdown = new StampDutyBreakdown
        {
            Name = "Above 225,000 and up to 250,000",
            Percentage = 1,
        };

        double taxableAmount;

        if (amount > 225000)
        {
            taxableAmount = amount - 225000;

            if (taxableAmount > 25000)
            {
                taxableAmount = 25000;
            }

            currentBreakdown.Amount = taxableAmount;
            currentBreakdown.TaxDue = Math.Round(taxableAmount / 100 * currentBreakdown.Percentage, 2);
        }
        response.Breakdown.Add(currentBreakdown);

        currentBreakdown = new StampDutyBreakdown
        {
            Name = "Above 250,000 and up to 1,000,000",
            Percentage = 5,
        };

        if (amount > 250000)
        {
            taxableAmount = amount - 250000;

            if (taxableAmount > 750000)
            {
                taxableAmount = 750000;
            }

            currentBreakdown.Amount = taxableAmount;
            currentBreakdown.TaxDue = Math.Round(taxableAmount / 100 * currentBreakdown.Percentage, 2);
        }
        response.Breakdown.Add(currentBreakdown);

        currentBreakdown = new StampDutyBreakdown
        {
            Name = "Above 1,000,000",
            Percentage = 6,
        };

        if (amount > 1000000)
        {
            taxableAmount = amount - 1000000;

            currentBreakdown.Amount = taxableAmount;
            currentBreakdown.TaxDue = Math.Round(taxableAmount / 100 * currentBreakdown.Percentage, 2);
        }
        response.Breakdown.Add(currentBreakdown);

        return response;
    }
}