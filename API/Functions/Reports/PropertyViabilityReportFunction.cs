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
    [OpenApiRequestBody("application/json", typeof(PropertyViabilityReportRequest))]
    [OpenApiResponseWithBody(HttpStatusCode.OK, "application/json", typeof(PropertyViabilityReport))]
    [OpenApiParameter(name: "accountId", In = ParameterLocation.Path,  Required = true, Type = typeof(Guid))]
    [OpenApiParameter(name: "propertyId", In = ParameterLocation.Path,  Required = true, Type = typeof(Guid))]
    [Function("CreatePropertyViabilityReport")]
    public async Task<HttpResponseData> CalculateInterest([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "Reports/PropertyViability/{accountId}/{propertyId}")] HttpRequestData request,
        Guid accountId,
        Guid propertyId)
    {
        HttpResponseData response;

        try
        {
            var requestContent = await request.ReadAsStringAsync();
            var requestBody = JsonSerializer.Deserialize<PropertyViabilityReportRequest>(requestContent);

            response = request.CreateResponse(HttpStatusCode.OK);

            var property = await _propertyService.GetPropertyAsync(accountId, propertyId);

            var report = await _reportService.GetPropertyViabilityReport(property, requestBody);
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
