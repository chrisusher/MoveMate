using ChrisUsher.MoveMate.Shared.DTOs.Properties;
using ChrisUsher.MoveMate.Shared.DTOs.Properties.StampDuty;
using ChrisUsher.MoveMate.Shared.Enums;

namespace ChrisUsher.MoveMate.API.Services.StampDuty;

public class StampDutyService
{
    private double _amount = 0;

    public StampDutyResponse CalculateStampDuty(Property property, StampDutyRequest requestBody, CaseType caseType)
    {
        switch (caseType)
        {
            case CaseType.MiddleCase:
                _amount = Math.Round((property.MaxValue + property.MinValue) / 2, 2);
                break;
            case CaseType.WorstCase:
                _amount = property.MaxValue;
                break;
            case CaseType.BestCase:
                _amount = property.MinValue;
                break;
        }

        return CalculateStampDuty(_amount, requestBody);
    }

    public static List<StampDutyBreakdown> CalculateStampDutyBreakdown(double amount, List<StampDutyBand> taxRequirements)
    {
        var response = new List<StampDutyBreakdown>();

        foreach (var band in taxRequirements)
        {
            var currentBreakdown = new StampDutyBreakdown();

            if (amount > band.UpperBound)
            {
                currentBreakdown.Amount = amount - band.LowerBound;

                if (currentBreakdown.Amount > band.UpperBound - band.LowerBound)
                {
                    currentBreakdown.Amount = band.UpperBound - band.LowerBound;
                }

                currentBreakdown.TaxDue = Math.Round(currentBreakdown.Amount / 100 * band.TaxRate, 2);
            }
            else if (amount > band.LowerBound)
            {
                currentBreakdown.Amount = amount - band.LowerBound;
                currentBreakdown.TaxDue = Math.Round(currentBreakdown.Amount / 100 * band.TaxRate, 2);
            }
            else
            {
                currentBreakdown.Amount = 0;
                currentBreakdown.TaxDue = 0;
            }

            response.Add(currentBreakdown);
        }

        return response;
    }

    private static StampDutyResponse CalculateStampDuty(double amount, StampDutyRequest stampDutyRequest)
    {
        return stampDutyRequest.Location switch
        {
            UKRegionType.Wales => WalesStampDutyService.CalculateStampDuty(amount, stampDutyRequest),
            UKRegionType.England => EnglandStampDutyService.CalculateStampDuty(amount, stampDutyRequest),
            UKRegionType.Scotland => ScotlandStampDutyService.CalculateStampDuty(amount, stampDutyRequest),

            _ => throw new Exception($"Location : {stampDutyRequest.Location} is not yet supported by this Function."),
        };
    }
}
