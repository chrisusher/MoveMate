using System.Net;
using ChrisUsher.MoveMate.API.Services.Properties;
using ChrisUsher.MoveMate.Shared.DTOs.Properties;
using ChrisUsher.MoveMate.Shared.Enums;
using Microsoft.OpenApi.Models;

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
    [OpenApiParameter(name: "accountId", 
In = ParameterLocation.Path,  Required = true, Type = typeof(Guid))]
    [Function("CreateProperty")]
    public async Task<HttpResponseData> CreateProperty([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "Accounts/{accountId}/Properties")] HttpRequestData request,
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
        catch(PropertyAlreadyExistsException propertyAlreadyExists)
        {
            response = request.CreateResponse(HttpStatusCode.Conflict);
            await response.WriteStringAsync(propertyAlreadyExists.Message);

            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(new EventId(Convert.ToInt32(DateTime.UtcNow.ToString("HHmmss"))), ex, ex.Message);
            throw;
        }

        return response;
    }

    [OpenApiOperation(operationId: "GetProperties", tags: new[] { "Properties" }, Summary = "")]
    [OpenApiResponseWithBody(HttpStatusCode.OK, "application/json", typeof(List<Property>))]
    [OpenApiParameter(name: "accountId", 
In = ParameterLocation.Path,  Required = true, Type = typeof(Guid))]
    [OpenApiParameter(name: "propertyType", In = ParameterLocation.Query, Type = typeof(PropertyType))]
    [Function("GetProperties")]
    public async Task<HttpResponseData> GetProperties([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "Accounts/{accountId}/Properties")] HttpRequestData request,
        Guid accountId,
        string propertyType)
    {
        HttpResponseData response;

        try
        {
            var propertyTypeEnum = PropertyType.ToPurchase;

            if (!string.IsNullOrEmpty(propertyType))
            {
                propertyTypeEnum = Enum.Parse<PropertyType>(propertyType, true);
            }

            response = request.CreateResponse(HttpStatusCode.OK);
            var responseBody = await _propertyService.GetPropertiesAsync(accountId, propertyTypeEnum);

            await response.WriteAsJsonAsync(responseBody);
        }
        catch (Exception ex)
        {
            _logger.LogError(new EventId(Convert.ToInt32(DateTime.UtcNow.ToString("HHmmss"))), ex, ex.Message);
            throw;
        }
        return response;
    }
    
    [OpenApiOperation(operationId: "GetProperty", tags: new[] { "Properties" }, Summary = "")]
    [OpenApiResponseWithBody(HttpStatusCode.OK, "application/json", typeof(Property))]
    [OpenApiParameter(name: "accountId", 
In = ParameterLocation.Path,  Required = true, Type = typeof(Guid))]
    [OpenApiParameter(name: "propertyId", 
In = ParameterLocation.Path,  Required = true, Type = typeof(Guid))]
    [Function("GetProperty")]
    public async Task<HttpResponseData> GetProperty([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "Accounts/{accountId}/Properties/{propertyId}")] HttpRequestData request,
        Guid accountId,
        Guid propertyId)
    {
        HttpResponseData response;

        try
        {
            response = request.CreateResponse(HttpStatusCode.OK);
            var responseBody = await _propertyService.GetPropertyAsync(accountId, propertyId);

            await response.WriteAsJsonAsync(responseBody);
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

    [OpenApiOperation(operationId: "UpdateProperty", tags: new[] { "Properties" }, Summary = "")]
    [OpenApiRequestBody("application/json", typeof(UpdatePropertyRequest))]
    [OpenApiResponseWithBody(HttpStatusCode.OK, "application/json", typeof(Property))]
    [OpenApiParameter(name: "accountId", 
In = ParameterLocation.Path,  Required = true, Type = typeof(Guid))]
    [OpenApiParameter(name: "propertyId", 
In = ParameterLocation.Path,  Required = true, Type = typeof(Guid))]
    [Function("UpdateProperty")]
    public async Task<HttpResponseData> UpdateProperty([HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "Accounts/{accountId}/Properties/{propertyId}")] HttpRequestData request,
        Guid accountId,
        Guid propertyId)
    {
        HttpResponseData response;

        try
        {
            var requestContent = await request.ReadAsStringAsync();
            var requestBody = JsonSerializer.Deserialize<UpdatePropertyRequest>(requestContent);

            response = request.CreateResponse(HttpStatusCode.OK);
            var responseBody = await _propertyService.UpdatePropertyAsync(accountId, propertyId, requestBody);

            await response.WriteAsJsonAsync(responseBody);
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