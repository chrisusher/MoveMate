using ChrisUsher.MoveMate.API.Services.Reports;
using ChrisUsher.MoveMate.Shared.DTOs.Reports;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;

namespace ChrisUsher.MoveMate.API.Functions.Reports;

public class InvestmentReportFunction : HttpFunction
{
    private readonly ReportsService _reportsService;
    private readonly ILogger<InvestmentReportFunction> _logger;

    public InvestmentReportFunction(
        ILoggerFactory loggerFactory,
        ReportsService reportsService
    )
    {
        _reportsService = reportsService;
        _logger = loggerFactory.CreateLogger<InvestmentReportFunction>();
    }

    [OpenApiOperation(operationId: "InvestmentReportFunction", tags: new[] { "Reports" }, Summary = "")]
    [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "x-functions-key", In = OpenApiSecurityLocationType.Header)]
    [OpenApiResponseWithBody(HttpStatusCode.OK, "application/json", typeof(SavingsReport))]
    [OpenApiParameter(name: "accountId", In = ParameterLocation.Path, Required = true, Type = typeof(Guid))]
    [OpenApiParameter(name: "savingsId", In = ParameterLocation.Path, Required = true, Type = typeof(Guid))]
    [Function("InvestmentReportFunction")]
    public async Task<HttpResponseData> CreateReportAsync([HttpTrigger(AuthorizationLevel.Function, "get", Route = "Reports/InvestmentReport/{accountId}/{savingsId}")] HttpRequestData request,
        Guid accountId,
        Guid savingsId)
    {
        HttpResponseData response;

        try
        {
            response = request.CreateResponse(HttpStatusCode.OK);

            var report = await _reportsService.GetStockInvestmentReportAsync(accountId, savingsId);
            
            await response.WriteAsJsonAsync(report);
        }
        catch (DataNotFoundException dataNotFound)
        {
            response = request.CreateResponse(HttpStatusCode.NotFound);
            await response.WriteStringAsync(dataNotFound.Message);

            return response;
        }
        catch (InvalidRequestException invalidRequest)
        {
            response = request.CreateResponse(HttpStatusCode.BadRequest);
            await response.WriteStringAsync(invalidRequest.Message);

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
