using ChrisUsher.MoveMate.Shared.DTOs.StampDuty;
using ChrisUsher.MoveMate.Shared.Enums;

namespace ChrisUsher.MoveMate.API.Services.StampDuty
{
    public static class EnglandStampDutyService
    {
        public static StampDutyResponse CalculateStampDuty(double amount, StampDutyRequest stampDutyRequest)
        {
            if(stampDutyRequest.FirstTime)
            {
                return CalculateFirstTimeStampDuty(amount);
            }
            if(stampDutyRequest.TaxRate == UKTaxType.HigherTax)
            {
                return CalculateHigherTaxStampDuty(amount);
            }
            if(stampDutyRequest.ResidentialType == PropertyResidentialType.NonResidential)
            {
                return CalculateNonResidentialStampDuty(amount);
            }
            return CalculateMainTaxStampDuty(amount);
        }

        private static StampDutyResponse CalculateFirstTimeStampDuty(double amount)
        {
            if(amount > 625000)
            {
                return CalculateFirstTimeStampDutyLargeAmount(amount);
            }
            StampDutyBreakdown currentBreakdown;

            var response = new StampDutyResponse
            {
                PurchasePrice = amount
            };

            response.Breakdown.Add(new StampDutyBreakdown
            {
                Amount = amount < 425000 ? amount : 425000,
                TaxDue = 0,
                Percentage = 0,
                Name = "Up to 425,000"
            });

            currentBreakdown = new StampDutyBreakdown
            {
                Name = "Above 425,000 and up to 625,000",
                Percentage = 5,
            };

            double taxableAmount;

            if (amount > 425000)
            {
                taxableAmount = amount - 425000;

                if(taxableAmount > 200000)
                {
                    taxableAmount = 200000;
                }

                currentBreakdown.Amount = taxableAmount;
                currentBreakdown.TaxDue = Math.Round(taxableAmount / 100 * currentBreakdown.Percentage, 2);
            }

            response.Breakdown.Add(currentBreakdown);



            return response;
        }

        private static StampDutyResponse CalculateFirstTimeStampDutyLargeAmount(double amount)
        {
            StampDutyBreakdown currentBreakdown;

            var response = new StampDutyResponse
            {
                PurchasePrice = amount
            };

            response.Breakdown.Add(new StampDutyBreakdown
            {
                Amount = amount < 250000 ? amount : 250000,
                TaxDue = 0,
                Percentage = 0,
                Name = "Up to 250,000"
            });

            currentBreakdown = new StampDutyBreakdown
            {
                Name = "Above 250,000 and up to 925,000",
                Percentage = 5,
            };

            double taxableAmount;

            if (amount > 250000)
            {
                taxableAmount = amount - 250000;

                if(taxableAmount > 675000)
                {
                    taxableAmount = 675000;
                }

                currentBreakdown.Amount = taxableAmount;
                currentBreakdown.TaxDue = Math.Round(taxableAmount / 100 * currentBreakdown.Percentage, 2);
            }

            response.Breakdown.Add(currentBreakdown);

            currentBreakdown = new StampDutyBreakdown
            {
                Name = "Above 925,000 and up to 1.5m",
                Percentage = 10,
            };

            if (amount > 925000)
            {
                taxableAmount = amount - 925000;

                if(taxableAmount > 575000)
                {
                    taxableAmount = 575000;
                }

                currentBreakdown.Amount = taxableAmount;
                currentBreakdown.TaxDue = Math.Round(taxableAmount / 100 * currentBreakdown.Percentage, 2);
            }

            response.Breakdown.Add(currentBreakdown);

            currentBreakdown = new StampDutyBreakdown
            {
                Name = "Above 1.5m",
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

        private static StampDutyResponse CalculateHigherTaxStampDuty(double amount)
        {
            StampDutyBreakdown currentBreakdown;

            var response = new StampDutyResponse
            {
                PurchasePrice = amount
            };

            return response;
        }    
    
        private static StampDutyResponse CalculateNonResidentialStampDuty(double amount)
        {
            StampDutyBreakdown currentBreakdown;

            var response = new StampDutyResponse
            {
                PurchasePrice = amount
            };

            return response;
        }
    }
}