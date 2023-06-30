using System.Net;
using ChrisUsher.MoveMate.API.Services.Savings;
using ChrisUsher.MoveMate.Shared.DTOs.Reports;
using ChrisUsher.MoveMate.Shared.Enums;
using Microsoft.OpenApi.Models;

namespace ChrisUsher.MoveMate.API.Functions.Reports;

public class SavingsReportFunction : HttpFunction
{
    private readonly SavingsService _savingsService;
    private readonly ILogger<SavingsReportFunction> _logger;

    public SavingsReportFunction(
        ILoggerFactory loggerFactory,
        SavingsService savingsService
    )
    {
        _savingsService = savingsService;
        _logger = loggerFactory.CreateLogger<SavingsReportFunction>();
    }

    [OpenApiOperation(operationId: "SavingsReportFunction", tags: new[] { "Reports" }, Summary = "")]
    [OpenApiResponseWithBody(HttpStatusCode.OK, "application/json", typeof(SavingsReport))]
    [OpenApiParameter(name: "accountId", In = ParameterLocation.Path,  Required = true, Type = typeof(Guid))]
    [OpenApiParameter(name: "caseModel", In = ParameterLocation.Query,  Required = true, Type = typeof(CaseType))]
    [Function("SavingsReportFunction")]
    public async Task<HttpResponseData> CalculateInterest([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "Reports/SavingsReport/{accountId}")] HttpRequestData request,
        Guid accountId,
        string caseModel)
    {
        HttpResponseData response;

        try
        {
            var caseType = CaseType.MiddleCase;

            if(!string.IsNullOrEmpty(caseModel))
            {
                caseType = Enum.Parse<CaseType>(caseModel, true);
            }

            response = request.CreateResponse(HttpStatusCode.OK);

            var totalBalance = 0.0;

            var report = new SavingsReport
            {
                Savings = await _savingsService.GetSavingsAccountsAsync(accountId)
            };

            foreach(var savingsAccount in report.Savings)
            {
                if(savingsAccount.Fluctuations != null)
                {
                    switch(caseType)
                    {
                        case CaseType.MiddleCase:
                            totalBalance += Math.Round((savingsAccount.Fluctuations.WorstCase + savingsAccount.Fluctuations.BestCase) / 2, 2);
                            break;
                        case CaseType.WorstCase:
                            totalBalance += savingsAccount.Fluctuations.WorstCase;
                            break;
                        case CaseType.BestCase:
                            totalBalance += savingsAccount.Fluctuations.BestCase;
                            break;
                    }
                    continue;
                }

                if(savingsAccount.Balances.Any())
                {
                    totalBalance += savingsAccount.Balances
                        .OrderBy(x => x.Created)
                        .Last()
                        .Balance;

                    continue;
                }

                totalBalance += savingsAccount.InitialBalance;
            }
            report.TotalSavings = totalBalance;
            await response.WriteAsJsonAsync(report);
        }
        catch (DataNotFoundException dataNotFound)
        {
            response = request.CreateResponse(HttpStatusCode.NotFound);
            await response.WriteStringAsync(dataNotFound.Message);

            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(new EventId(Convert.ToInt32(DateTime.UtcNow.ToString("HHmmss"))), ex, ex.Message);
            throw;
        }
        return response;
    }
}
