using System.Net;
using ChrisUsher.MoveMate.API.Services.Reports;
using ChrisUsher.MoveMate.Shared.DTOs.Reports;
using ChrisUsher.MoveMate.Shared.Enums;
using Microsoft.OpenApi.Models;

namespace ChrisUsher.MoveMate.API.Functions.Reports;

public class SavingsOverTimeReportFunction : HttpFunction
{
    private readonly ReportsService _reportsService;
    private readonly ILogger<SavingsOverTimeReportFunction> _logger;

    public SavingsOverTimeReportFunction(
        ILoggerFactory loggerFactory,
        ReportsService reportsService
    )
    {
        _reportsService = reportsService;
        _logger = loggerFactory.CreateLogger<SavingsOverTimeReportFunction>();
    }

    [OpenApiOperation(operationId: "SavingsOverTimeReportFunction", tags: new[] { "Reports" }, Summary = "")]
    [OpenApiResponseWithBody(HttpStatusCode.OK, "application/json", typeof(SavingsOverTimeReport))]
    [OpenApiParameter(name: "accountId", In = ParameterLocation.Path, Required = true, Type = typeof(Guid))]
    [Function("SavingsOverTimeReportFunction")]
    public async Task<HttpResponseData> CreateReportAsync([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "Reports/SavingsReportOverTime/{accountId}")] HttpRequestData request,
        Guid accountId)
    {
        HttpResponseData response;

        try
        {
            response = request.CreateResponse(HttpStatusCode.OK);

            var report = await _reportsService.GetSavingsOverTimeReportAsync(accountId);
            
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
