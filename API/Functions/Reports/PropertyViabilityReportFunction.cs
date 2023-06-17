using System.Net;
using ChrisUsher.MoveMate.API.Services.Properties;
using ChrisUsher.MoveMate.API.Services.Reports;
using ChrisUsher.MoveMate.Shared.DTOs.Reports;
using ChrisUsher.MoveMate.Shared.Enums;
using Microsoft.OpenApi.Models;

namespace ChrisUsher.MoveMate.API.Functions.Reports;

public class PropertyViabilityReportFunction : HttpFunction
{
    private readonly ILogger<PropertyViabilityReportFunction> _logger;
    private readonly ReportsService _reportService;
    private readonly PropertyService _propertyService;

    public PropertyViabilityReportFunction(
        ILoggerFactory loggerFactory,
        PropertyService propertyService,
        ReportsService reportService
    )
    {
        _logger = loggerFactory.CreateLogger<PropertyViabilityReportFunction>();
        _reportService = reportService;
        _propertyService = propertyService;
    }

    [OpenApiOperation(operationId: "CreatePropertyViabilityReport", tags: new[] { "Reports" }, Summary = "")]
    [OpenApiResponseWithBody(HttpStatusCode.OK, "application/json", typeof(PropertyViabilityReport))]
    [OpenApiParameter(name: "accountId", In = ParameterLocation.Path,  Required = true, Type = typeof(Guid))]
    [OpenApiParameter(name: "propertyId", In = ParameterLocation.Path,  Required = true, Type = typeof(Guid))]
    [OpenApiParameter(name: "interestRate", In = ParameterLocation.Query,  Required = true, Type = typeof(double))]
    [OpenApiParameter(name: "caseModel", In = ParameterLocation.Query,  Required = true, Type = typeof(CaseType))]
    [OpenApiParameter(name: "years", In = ParameterLocation.Query,  Required = true, Type = typeof(int))]
    [Function("CreatePropertyViabilityReport")]
    public async Task<HttpResponseData> CalculateInterest([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "Reports/PropertyViability/{accountId}/{propertyId}")] HttpRequestData request,
        Guid accountId,
        Guid propertyId,
        double interestRate,
        string caseModel,
        int years)
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

            var property = await _propertyService.GetPropertyAsync(accountId, propertyId);

            var report = await _reportService.GetPropertyViabilityReport(property, interestRate, caseType, years);
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
