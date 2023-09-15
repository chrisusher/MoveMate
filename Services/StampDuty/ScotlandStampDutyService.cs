using ChrisUsher.MoveMate.Shared.DTOs.StampDuty;
using ChrisUsher.MoveMate.Shared.Enums;

namespace ChrisUsher.MoveMate.API.Services.StampDuty
{
    public static class ScotlandStampDutyService
    {
        public static StampDutyResponse CalculateStampDuty(double amount, StampDutyRequest stampDutyRequest)
        {
            if(stampDutyRequest.FirstTime)
            {
                return CalculateFirstTimeStampDuty(amount);
            }
            if(stampDutyRequest.AdditionalProperty)
            {
                return CalculateAdditionalStampDuty(amount);
            }
            if(stampDutyRequest.ResidentialType == PropertyResidentialType.NonResidential)
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

                if(taxableAmount > 75000)
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

                if(taxableAmount > 75000)
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

                if(taxableAmount > 425000)
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
            StampDutyBreakdown currentBreakdown;

            var response = new StampDutyResponse
            {
                PurchasePrice = amount
            };

            currentBreakdown = new StampDutyBreakdown
            {
                Amount = amount < 145000 ? amount : 145000,
                Percentage = 6,
                Name = "Up to 145,000"
            };

            double taxableAmount;

            if(amount > 0)
            {
                taxableAmount = currentBreakdown.Amount;

                currentBreakdown.TaxDue = Math.Round(taxableAmount / 100 * currentBreakdown.Percentage, 2);
            }
            response.Breakdown.Add(currentBreakdown);

            currentBreakdown = new StampDutyBreakdown
            {
                Percentage = 8,
                Name = "Above 145,000 and up to 250,000"
            };

            if(amount > 145000)
            {
                taxableAmount = amount - 145000;

                if(taxableAmount > 105000)
                {
                    taxableAmount = 105000;
                }

                currentBreakdown.TaxDue = Math.Round(taxableAmount / 100 * currentBreakdown.Percentage, 2);
            }
            response.Breakdown.Add(currentBreakdown);

            currentBreakdown = new StampDutyBreakdown
            {
                Percentage = 11,
                Name = "Above 250,000 and up to 325,000"
            };

            if(amount > 250000)
            {
                taxableAmount = amount - 250000;

                if(taxableAmount > 75000)
                {
                    taxableAmount = 75000;
                }

                currentBreakdown.TaxDue = Math.Round(taxableAmount / 100 * currentBreakdown.Percentage, 2);
            }
            response.Breakdown.Add(currentBreakdown);

            currentBreakdown = new StampDutyBreakdown
            {
                Percentage = 16,
                Name = "Above 325,000 and up to 750,000"
            };

            if(amount > 325000)
            {
                taxableAmount = amount - 325000;

                if(taxableAmount > 425000)
                {
                    taxableAmount = 425000;
                }

                currentBreakdown.TaxDue = Math.Round(taxableAmount / 100 * currentBreakdown.Percentage, 2);
            }
            response.Breakdown.Add(currentBreakdown);

            currentBreakdown = new StampDutyBreakdown
            {
                Name = "Above 750,000",
                Percentage = 18,
            };

            if(amount > 750000)
            {
                taxableAmount = amount - 750000;

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

                if(taxableAmount > 105000)
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

            if(amount > 250000)
            {
                taxableAmount = amount - 250000;

                if(taxableAmount > 75000)
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

            if(amount > 325000)
            {
                taxableAmount = amount - 325000;

                if(taxableAmount > 425000)
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

            if(amount > 750000)
            {
                taxableAmount = amount - 750000;

                currentBreakdown.Amount = taxableAmount;
                currentBreakdown.TaxDue = Math.Round(taxableAmount / 100 * currentBreakdown.Percentage, 2);
            }
            response.Breakdown.Add(currentBreakdown);

            return response;
        }
    }
}