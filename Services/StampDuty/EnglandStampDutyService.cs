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
                Name = "Above 925,000 and up to 1,500,000",
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

        private static StampDutyResponse CalculateAdditionalStampDuty(double amount)
        {
            StampDutyBreakdown currentBreakdown;

            var response = new StampDutyResponse
            {
                PurchasePrice = amount
            };

            currentBreakdown = new StampDutyBreakdown
            {
                Amount = amount < 250000 ? amount : 250000,
                Percentage = 3,
                Name = "Up to 250,000"
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
                Name = "Above 250,000 and up to 925,000"
            };

            if(amount > 250000)
            {
                taxableAmount = amount - 250000;

                if(taxableAmount > 675000)
                {
                    taxableAmount = 675000;
                }

                currentBreakdown.TaxDue = Math.Round(taxableAmount / 100 * currentBreakdown.Percentage, 2);
            }
            response.Breakdown.Add(currentBreakdown);

            currentBreakdown = new StampDutyBreakdown
            {
                Percentage = 13,
                Name = "Above 925,000 and up to 1,500,000"
            };

            if(amount > 925000)
            {
                taxableAmount = amount - 925000;

                if(taxableAmount > 575000)
                {
                    taxableAmount = 575000;
                }

                currentBreakdown.TaxDue = Math.Round(taxableAmount / 100 * currentBreakdown.Percentage, 2);
            }
            response.Breakdown.Add(currentBreakdown);

            currentBreakdown = new StampDutyBreakdown
            {
                Name = "Above 1,500,000",
                Percentage = 15,
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

        private static StampDutyResponse CalculateMainTaxStampDuty(double amount)
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
                Name = "Above 925,000 and up to 1,500,000",
                Percentage = 10,
            };

            if(amount > 925000)
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