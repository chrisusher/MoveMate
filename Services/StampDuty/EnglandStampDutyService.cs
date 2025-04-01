using ChrisUsher.MoveMate.Shared.DTOs.Properties.StampDuty;
using ChrisUsher.MoveMate.Shared.Enums;

namespace ChrisUsher.MoveMate.API.Services.StampDuty;

public static class EnglandStampDutyService
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
        if (amount > 500_000)
        {
            return CalculateFirstTimeStampDutyLargeAmount(amount);
        }

        var response = new StampDutyResponse
        {
            PurchasePrice = amount
        };

        var taxRequirements = new List<StampDutyBand>
            {
                new ()
                {
                    LowerBound = 0,
                    UpperBound = 300_000,
                    TaxRate = 0
                },
                new ()
                {
                    LowerBound = 300_000,
                    UpperBound = 500_000,
                    TaxRate = 5
                },
                new ()
                {
                    LowerBound = 500_000,
                    UpperBound = 925_000,
                    TaxRate = 5
                },
                new ()
                {
                    LowerBound = 925_000,
                    UpperBound = 1_500_000,
                    TaxRate = 10
                },
                new ()
                {
                    LowerBound = 1_500_000,
                    UpperBound = 1_000_000_000,
                    TaxRate = 12
                },
            };

        response.Breakdown = StampDutyService.CalculateStampDutyBreakdown(amount, taxRequirements);

        return response;
    }

    private static StampDutyResponse CalculateFirstTimeStampDutyLargeAmount(double amount)
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
                    UpperBound = 125_000,
                    TaxRate = 0
                },
                new ()
                {
                    LowerBound = 125_000,
                    UpperBound = 250_000,
                    TaxRate = 2
                },
                new ()
                {
                    LowerBound = 250_000,
                    UpperBound = 925_000,
                    TaxRate = 5
                },
                new ()
                {
                    LowerBound = 925_000,
                    UpperBound = 1_500_000,
                    TaxRate = 10
                },
                new ()
                {
                    LowerBound = 1_500_000,
                    UpperBound = 1_000_000_000,
                    TaxRate = 12
                },
            };

        response.Breakdown = StampDutyService.CalculateStampDutyBreakdown(amount, taxRequirements);

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
                    UpperBound = 125_000,
                    TaxRate = 5
                },
                new ()
                {
                    LowerBound = 125_000,
                    UpperBound = 250_000,
                    TaxRate = 7
                },
                new ()
                {
                    LowerBound = 250_000,
                    UpperBound = 925_000,
                    TaxRate = 10
                },
                new ()
                {
                    LowerBound = 925_000,
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

    private static StampDutyResponse CalculateMainTaxStampDuty(double amount)
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
                    UpperBound = 125_000,
                    TaxRate = 0
                },
                new ()
                {
                    LowerBound = 125_000,
                    UpperBound = 250_000,
                    TaxRate = 2
                },
                new ()
                {
                    LowerBound = 250_000,
                    UpperBound = 925_000,
                    TaxRate = 5
                },
                new ()
                {
                    LowerBound = 925_000,
                    UpperBound = 1_500_000,
                    TaxRate = 10
                },
                new ()
                {
                    LowerBound = 1_500_000,
                    UpperBound = 1_000_000_000,
                    TaxRate = 12
                },
            };

        response.Breakdown = StampDutyService.CalculateStampDutyBreakdown(amount, taxRequirements);

        return response;
    }
}