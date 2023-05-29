using System.Net;
using ChrisUsher.MoveMate.API.Services.Properties;
using ChrisUsher.MoveMate.Shared.DTOs.Properties;

namespace ChrisUsher.MoveMate.API.Functions.Properties;

public class PropertyFunctions
{
    private readonly PropertyService _propertyService;
    private readonly ILogger<PropertyFunctions> _logger;

    public PropertyFunctions(
        ILoggerFactory loggerFactory,
        PropertyService propertyService)
    {
        _logger = loggerFactory.CreateLogger<PropertyFunctions>();
        _propertyService = propertyService;
    }
    
    [OpenApiOperation(operationId: "CreateProperty", tags: new[] { "Properties" }, Summary = "")]
    [OpenApiRequestBody("application/json", typeof(CreatePropertyRequest))]
    [OpenApiResponseWithBody(HttpStatusCode.OK, "application/json", typeof(Property))]
    [OpenApiParameter("accountId")]
    [Function("CreateProperty")]
    public async Task<HttpResponseData> CreateSavingsAccount([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "Accounts/{accountId}/Properties")] HttpRequestData request,
        Guid accountId)
    {
        HttpResponseData response;

        try
        {
            var requestContent = await request.ReadAsStringAsync();
            var requestBody = JsonSerializer.Deserialize<CreatePropertyRequest>(requestContent);

            response = request.CreateResponse(HttpStatusCode.OK);
            var responseBody = await _propertyService.CreatePropertyAsync(accountId, requestBody);

            await response.WriteAsJsonAsync(responseBody);
        }
        catch (Exception ex)
        {
            _logger.LogError(new EventId(Convert.ToInt32(DateTime.UtcNow.ToString("HHmmss"))), ex, ex.Message);
            throw;
        }

        return response;
    }
}