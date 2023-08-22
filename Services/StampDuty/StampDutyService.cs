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

            return CalculateStampDuty(_amount, requestBody.Location);
        }

        private StampDutyResponse CalculateStampDuty(double amount, UKRegionType location)
        {
            return location switch
            {
                UKRegionType.Wales => CalculateWalesStampDuty(amount),
                _ => throw new Exception($"Location : {location} is not yet supported by this Function."),
            };
        }

        private StampDutyResponse CalculateWalesStampDuty(double amount)
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

                if(taxableAmount > 175000)
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

            if(amount > 400000)
            {
                taxableAmount = amount - 400000;

                if(taxableAmount > 350000)
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

            if(amount > 750000)
            {
                taxableAmount = amount - 750000;

                if(taxableAmount > 750000)
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

            if(amount > 1500000)
            {
                taxableAmount = amount - 1500000;

                currentBreakdown.Amount = taxableAmount;
                currentBreakdown.TaxDue = Math.Round(taxableAmount / 100 * currentBreakdown.Percentage, 2);
            }
            response.Breakdown.Add(currentBreakdown);

            return response;
        }
    }
}