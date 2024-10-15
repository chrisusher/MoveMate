using System.Globalization;
using System.Text;
using Azure.Storage.Blobs;
using ChrisUsher.MoveMate.API.Services.Accounts;
using ChrisUsher.MoveMate.API.Services.Costs;
using ChrisUsher.MoveMate.API.Services.Mortgages;
using ChrisUsher.MoveMate.API.Services.Properties;
using ChrisUsher.MoveMate.API.Services.Savings;
using ChrisUsher.MoveMate.API.Services.StampDuty;
using ChrisUsher.MoveMate.Shared.DTOs.Mortgages;
using ChrisUsher.MoveMate.Shared.DTOs.Properties;
using ChrisUsher.MoveMate.Shared.DTOs.Reports;
using ChrisUsher.MoveMate.Shared.DTOs.Savings;
using ChrisUsher.MoveMate.Shared.Enums;

namespace ChrisUsher.MoveMate.API.Services.Reports;

public class ReportsService
{
    private readonly AccountService _accountService;
    private readonly PropertyService _propertyService;
    private readonly SavingsService _savingsService;
    private readonly InterestService _interestService;
    private readonly MortgagePaymentService _mortgagePaymentService;
    private readonly CostService _costsService;
    private readonly StampDutyService _stampDutyService;
    private readonly StockService _stockService;
    private readonly BlobContainerClient _containerClient;

    public ReportsService(
        AccountService accountService,
        PropertyService propertyService,
        SavingsService savingsService,
        InterestService interestService,
        MortgagePaymentService mortgagePaymentService,
        CostService costsService,
        StampDutyService stampDutyService,
        StockService stockService,
        BlobServiceClient blobServiceClient
    )
    {
        _accountService = accountService;
        _propertyService = propertyService;
        _savingsService = savingsService;
        _interestService = interestService;
        _mortgagePaymentService = mortgagePaymentService;
        _costsService = costsService;
        _stampDutyService = stampDutyService;
        _stockService = stockService;
        _containerClient = blobServiceClient.GetBlobContainerClient("outputcache");
    }

    #region Public Methods

    public async Task<PropertyViabilityReport> GetPropertyViabilityReportAsync(Property property, PropertyViabilityReportRequest request)
    {
        if (property == null)
        {
            throw new DataNotFoundException("Property passed to PropertyViabilityReport was not found.");
        }
        var account = await _accountService.GetAccountAsync(property.AccountId);
        var currentProperty = await _propertyService.GetCurrentPropertyAsync(property.AccountId);

        var requestJson = JsonSerializer.Serialize(request);
        var report = JsonSerializer.Deserialize<PropertyViabilityReport>(requestJson);

        report.SaleDate = account.EstimatedSaleDate;
        report.Property = property;

        var purchasePrice = 0.0;

        if (request.CurrentPropertySalePrice.HasValue)
        {
            report.Equity = request.CurrentPropertySalePrice.Value - (currentProperty.Equity?.RemainingMortgage ?? 0);
        }
        if (request.PurchasePrice.HasValue)
        {
            purchasePrice = request.PurchasePrice.Value;
            property.MinValue = purchasePrice;
            property.MaxValue = purchasePrice;
        }

        var savings = await _savingsService.GetSavingsAccountsAsync(property.AccountId);
        var costs = await _costsService.GetCostsAsync(property.AccountId);

        report.SavingsAccounts = CalculateSavings(savings, request.CaseType, report.SaleDate);
        report.Costs = _costsService.CalculateCosts(costs, request.CaseType, property, currentProperty);

        switch (request.CaseType)
        {
            case CaseType.BestCase:
                if (report.Equity == 0)
                {
                    report.Equity = currentProperty.MaxValue - currentProperty.Equity.RemainingMortgage;
                }
                if (purchasePrice == 0)
                {
                    purchasePrice = property.MinValue;
                }

                report.MonthlyMortgagePayments = CalculateMonthlyPayments(purchasePrice, report.TotalSavings - report.TotalCosts, report.Equity, request.InterestRate, request.Years);
                break;

            case CaseType.MiddleCase:
                if (report.Equity == 0)
                {
                    report.Equity = Math.Round((currentProperty.MaxValue + currentProperty.MinValue) / 2 - currentProperty.Equity.RemainingMortgage, 2);
                }
                if (purchasePrice == 0)
                {
                    purchasePrice = Math.Round((property.MaxValue + property.MinValue) / 2, 2);
                }

                report.MonthlyMortgagePayments = CalculateMonthlyPayments(purchasePrice, report.TotalSavings - report.TotalCosts, report.Equity, request.InterestRate, request.Years);
                break;

            case CaseType.WorstCase:
                if (report.Equity == 0)
                {
                    report.Equity = currentProperty.MinValue - currentProperty.Equity.RemainingMortgage;
                }
                if (purchasePrice == 0)
                {
                    purchasePrice = property.MaxValue;
                }

                report.MonthlyMortgagePayments = CalculateMonthlyPayments(purchasePrice, report.TotalSavings - report.TotalCosts, report.Equity, request.InterestRate, request.Years);
                break;
        }

        report.Property.PurchasePrice = purchasePrice;
        return report;
    }

    public async Task<SavingsReport> GetSavingReportAsync(Guid accountId, CaseType caseType)
    {
        var blobName = $"{DateTime.UtcNow.ToString("yyy-MM-dd")}.json";
        var fullBlobName = $"saving-reports/{accountId}/{blobName}";

        if (_containerClient.GetBlobs().Any(x => x.Name.Contains(blobName))
        )
        {
            var blobClient = _containerClient.GetBlobClient(fullBlobName);

            using (var stream = await blobClient.OpenReadAsync())
            {
                return await JsonSerializer.DeserializeAsync<SavingsReport>(stream, ServicesCommon.JsonOptions);
            }
        }

        var totalBalance = 0.0;

        var report = new SavingsReport
        {
            Savings = (await _savingsService.GetSavingsAccountsAsync(accountId))
                .Select(ReportSavingsAccount.FromSavingsAccount)
                .ToList()
        };

        for (int index = 0; index < report.Savings.Count; index++)
        {
            var savingsAccount = report.Savings[index];

            if (savingsAccount.Fluctuations != null)
            {
                switch (caseType)
                {
                    case CaseType.MiddleCase:
                        report.Savings[index].CurrentBalance = Math.Round((savingsAccount.Fluctuations.WorstCase + savingsAccount.Fluctuations.BestCase) / 2, 2);
                        break;
                    case CaseType.WorstCase:
                        report.Savings[index].CurrentBalance += savingsAccount.Fluctuations.WorstCase;
                        break;
                    case CaseType.BestCase:
                        report.Savings[index].CurrentBalance += savingsAccount.Fluctuations.BestCase;
                        break;
                }
                totalBalance += report.Savings[index].CurrentBalance;
                continue;
            }

            if (savingsAccount.Balances.Any())
            {
                report.Savings[index].CurrentBalance = savingsAccount.Balances
                    .OrderBy(x => x.Created)
                    .Last()
                    .Balance;

                totalBalance += report.Savings[index].CurrentBalance;
                continue;
            }

            report.Savings[index].CurrentBalance = savingsAccount.InitialBalance;
            totalBalance += report.Savings[index].CurrentBalance;
        }
        report.TotalSavings = Math.Round(totalBalance, 2);

        var jsonContent = await SerializeAsync(report);

        using (MemoryStream stream = new(Encoding.UTF8.GetBytes(jsonContent)))
        {
            await _containerClient.UploadBlobAsync(fullBlobName, stream);
        }

        return report;
    }

    public async Task<SavingsOverTimeReport> GetSavingsOverTimeReportAsync(Guid accountId)
    {
        var report = new SavingsOverTimeReport();
        var allBlobs = _containerClient.GetBlobs(prefix: $"saving-reports/{accountId}/");

        foreach (var blob in allBlobs)
        {
            var blobClient = _containerClient.GetBlobClient(blob.Name);

            using (var stream = await blobClient.OpenReadAsync())
            {
                var reportEntry = await JsonSerializer.DeserializeAsync<SavingsReport>(stream, ServicesCommon.JsonOptions);

                // var fileName = blob.Name.Substring(blob.Name.LastIndexOf("/"));
                var fileNameWithoutPath = Path.GetFileNameWithoutExtension(blob.Name);

                report.Summaries.Add(new()
                {
                    SummaryDate = DateTime.ParseExact(fileNameWithoutPath, "yyyy-MM-dd", CultureInfo.CurrentCulture)
                });

                foreach (var saving in reportEntry.Savings)
                {
                    report.Summaries.Last().Savings.Add(new SavingsSummary
                    {
                        CurrentBalance = saving.CurrentBalance,
                        Name = saving.Name
                    });
                }
            }
        }

        return report;
    }

    public async Task<StockInvestmentReport> GetStockInvestmentReportAsync(Guid accountId, Guid savingsId)
    {
        var stockSavingsAccount = await _savingsService.GetSavingsAccountAsync(accountId, savingsId);
        
        if (stockSavingsAccount == null)
        {
            throw new DataNotFoundException(message: "Savings account passed to StockInvestmentReport was not found.");
        }

        if (stockSavingsAccount.SavingType != SavingType.StocksAndShares)
        {
            throw new InvalidRequestException(message: "Savings account passed to StockInvestmentReport is not a Stocks and Shares account.");
        }

        var stocksInAccountTask = _stockService.GetStocksAsync(stockSavingsAccount.SavingsId);

        var report = new StockInvestmentReport();

        foreach(var stock in await stocksInAccountTask)
        {
            var lastBalance = stock.Balances.Last();

            report.Stocks.Add(new()
            {
                AmountInvested = lastBalance.AmountInvested,
                StockId = stock.StockId,
                StockName = stock.StockName,
                CurrentValue = lastBalance.Balance,
            });
        }

        return report;
    }

    #endregion

    #region Private Methods

    private List<MonthlyMortgagePayment> CalculateMonthlyPayments(double purchasePrice, double totalSavings, double equity, double interestRate, int years)
    {
        var monthlyPayments = new List<MonthlyMortgagePayment>();

        var currentSavingAmount = 0;

        while (currentSavingAmount < totalSavings)
        {
            var deposit = equity + currentSavingAmount;

            monthlyPayments.Add(new MonthlyMortgagePayment
            {
                CashRequired = currentSavingAmount,
                MonthlyPayment = _mortgagePaymentService.CalculateMonthlyMortgagePayment(purchasePrice - deposit, interestRate, years),
                TotalDeposit = deposit
            });

            currentSavingAmount += 10000;
        }

        return monthlyPayments;
    }

    private List<SavingsAccount> CalculateSavings(List<SavingsAccount> savings, CaseType caseType, DateTime? saleDate)
    {
        var calculatedSavings = new List<SavingsAccount>();

        foreach (var saving in savings)
        {
            if (saving.Fluctuations != null)
            {
                switch (caseType)
                {
                    case CaseType.BestCase:
                        calculatedSavings.Add(new SavingsAccount
                        {
                            Name = saving.Name,
                            InitialBalance = saving.Fluctuations.BestCase,
                            SavingsRate = saving.SavingsRate
                        });
                        break;

                    case CaseType.MiddleCase:
                        calculatedSavings.Add(new SavingsAccount
                        {
                            Name = saving.Name,
                            InitialBalance = Math.Round((saving.Fluctuations.BestCase + saving.Fluctuations.WorstCase) / 2, 2),
                            SavingsRate = saving.SavingsRate
                        });
                        break;

                    case CaseType.WorstCase:
                        calculatedSavings.Add(new SavingsAccount
                        {
                            Name = saving.Name,
                            InitialBalance = saving.Fluctuations.WorstCase,
                            SavingsRate = saving.SavingsRate
                        });
                        break;
                }
                continue;
            }

            calculatedSavings.Add(new SavingsAccount
            {
                Name = saving.Name,
                SavingsRate = saving.SavingsRate,
                InitialBalance = _interestService.CalculateInterest(saving, saleDate).NewTotal
            });
        }

        return calculatedSavings;
    }

    private async Task<string> SerializeAsync<T>(T objectToSerialise)
    {
        using var stream = new MemoryStream();

        await JsonSerializer.SerializeAsync(stream, objectToSerialise, ServicesCommon.JsonOptions);

        stream.Position = 0;
        using var reader = new StreamReader(stream);

        return await reader.ReadToEndAsync();
    }

    #endregion
}