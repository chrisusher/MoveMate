using ChrisUsher.MoveMate.Shared.DTOs.StampDuty;
using ChrisUsher.MoveMate.Shared.Enums;

namespace ChrisUsher.MoveMate.API.Services.StampDuty
{
    public static class WalesStampDutyService
    {
        public static StampDutyResponse CalculateWalesStampDuty(double amount, StampDutyRequest stampDutyRequest)
        {
            if(stampDutyRequest.TaxRate == UKTaxType.HigherTax)
            {
                return CalculateHigherTaxStampDuty(amount);
            }
            if(stampDutyRequest.ResidentialType == PropertyResidentialType.NonResidential)
            {
                return CalculateNonResidentialStampDuty(amount);
            }
            return CalculateMainTaxWalesStampDuty(amount);
        }

        private static StampDutyResponse CalculateMainTaxWalesStampDuty(double amount)
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

            double taxableAmount;

            taxableAmount = amount < 180000 ? amount : 180000;

            response.Breakdown.Add(new StampDutyBreakdown
            {
                Amount = taxableAmount,
                TaxDue = Math.Round(taxableAmount / 100 * 4, 2),
                Percentage = 4,
                Name = "Up to 180,000"
            });

            currentBreakdown = new StampDutyBreakdown
            {
                Name = "Above 180,000 and up to 250,000",
                Percentage = 7.5,
            };

            if (amount > 180000)
            {
                taxableAmount = amount - 180000;

                if(taxableAmount > 70000)
                {
                    taxableAmount = 70000;
                }

                currentBreakdown.Amount = taxableAmount;
                currentBreakdown.TaxDue = Math.Round(taxableAmount / 100 * currentBreakdown.Percentage, 2);
            }
            response.Breakdown.Add(currentBreakdown);

            currentBreakdown = new StampDutyBreakdown
            {
                Name = "250,000 and up to 400,000",
                Percentage = 9,
            };

            if(amount > 250000)
            {
                taxableAmount = amount - 250000;

                if(taxableAmount > 150000)
                {
                    taxableAmount = 150000;
                }

                currentBreakdown.Amount = taxableAmount;
                currentBreakdown.TaxDue = Math.Round(taxableAmount / 100 * currentBreakdown.Percentage, 2);
            }
            response.Breakdown.Add(currentBreakdown);

            currentBreakdown = new StampDutyBreakdown
            {
                Name = "Above 400,000 and up to 750,000",
                Percentage = 11.5,
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
                Percentage = 14,
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
                Percentage = 16,
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

                if(taxableAmount > 25000)
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

            if(amount > 250000)
            {
                taxableAmount = amount - 250000;

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
                Name = "Above 1,000,000",
                Percentage = 6,
            };

            if(amount > 1000000)
            {
                taxableAmount = amount - 1000000;

                currentBreakdown.Amount = taxableAmount;
                currentBreakdown.TaxDue = Math.Round(taxableAmount / 100 * currentBreakdown.Percentage, 2);
            }
            response.Breakdown.Add(currentBreakdown);

            return response;
        }
    }
}