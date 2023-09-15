using ChrisUsher.MoveMate.Shared.DTOs.Properties;
using ChrisUsher.MoveMate.Shared.DTOs.StampDuty;
using ChrisUsher.MoveMate.Shared.Enums;

namespace ChrisUsher.MoveMate.API.Services.StampDuty
{
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

        private StampDutyResponse CalculateStampDuty(double amount, StampDutyRequest stampDutyRequest)
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
}