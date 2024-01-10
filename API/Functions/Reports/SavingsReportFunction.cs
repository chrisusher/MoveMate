using System.Net;
using ChrisUsher.MoveMate.API.Services.Reports;
using ChrisUsher.MoveMate.API.Services.Savings;
using ChrisUsher.MoveMate.Shared.DTOs.Reports;
using ChrisUsher.MoveMate.Shared.DTOs.Savings;
using ChrisUsher.MoveMate.Shared.Enums;
using Microsoft.OpenApi.Models;

namespace ChrisUsher.MoveMate.API.Functions.Reports;

public class SavingsReportFunction : HttpFunction
{
    private readonly ReportsService _reportsService;
    private readonly ILogger<SavingsReportFunction> _logger;

    public SavingsReportFunction(
        ILoggerFactory loggerFactory,
        ReportsService reportsService
    )
    {
        _reportsService = reportsService;
        _logger = loggerFactory.CreateLogger<SavingsReportFunction>();
    }

    [OpenApiOperation(operationId: "SavingsReportFunction", tags: new[] { "Reports" }, Summary = "")]
    [OpenApiResponseWithBody(HttpStatusCode.OK, "application/json", typeof(SavingsReport))]
    [OpenApiParameter(name: "accountId", In = ParameterLocation.Path, Required = true, Type = typeof(Guid))]
    [OpenApiParameter(name: "caseModel", In = ParameterLocation.Query, Required = true, Type = typeof(CaseType))]
    [Function("SavingsReportFunction")]
    public async Task<HttpResponseData> CalculateInterest([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "Reports/SavingsReport/{accountId}")] HttpRequestData request,
        Guid accountId,
        string caseModel)
    {
        HttpResponseData response;

        try
        {
            var caseType = CaseType.MiddleCase;

            if (!string.IsNullOrEmpty(caseModel))
            {
                caseType = Enum.Parse<CaseType>(caseModel, true);
            }

            response = request.CreateResponse(HttpStatusCode.OK);

            var report = await _reportsService.GetSavingReportAsync(accountId, caseType);
            
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
