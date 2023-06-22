using System.Net;
using ChrisUsher.MoveMate.API.Services.Costs;
using ChrisUsher.MoveMate.Shared.DTOs.Costs;
using Microsoft.OpenApi.Models;

namespace ChrisUsher.MoveMate.API.Functions.Costs;

public class CostsFunctions
{
    private readonly ILogger _logger;
    private readonly CostService _costService;

    public CostsFunctions(
        ILoggerFactory loggerFactory,
        CostService costService)
    {
        _logger = loggerFactory.CreateLogger<CostsFunctions>();
        _costService = costService;
    }
    
    [OpenApiOperation(operationId: "CreateCost", tags: new[] { "Costs" }, Summary = "")]
    [OpenApiRequestBody("application/json", typeof(CreateCostRequest))]
    [OpenApiResponseWithBody(HttpStatusCode.OK, "application/json", typeof(Cost))]
    [OpenApiParameter(name: "accountId", In = ParameterLocation.Path, Required = true, Type = typeof(Guid))]
    [Function("CreateCost")]
    public async Task<HttpResponseData> CreateCost([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "Accounts/{accountId}/Costs")] HttpRequestData request,
        Guid accountId)
    {
        HttpResponseData response;

        try
        {
            var requestContent = await request.ReadAsStringAsync();
            var requestBody = JsonSerializer.Deserialize<CreateCostRequest>(requestContent);

            response = request.CreateResponse(HttpStatusCode.OK);
            var responseBody = await _costService.CreateCostAsync(accountId, requestBody);

            await response.WriteAsJsonAsync(responseBody);
        }
        catch (Exception ex)
        {
            _logger.LogError(new EventId(Convert.ToInt32(DateTime.UtcNow.ToString("HHmmss"))), ex, ex.Message);
            throw;
        }

        return response;
    }
    
    [OpenApiOperation(operationId: "GetCosts", tags: new[] { "Costs" }, Summary = "")]
    [OpenApiResponseWithBody(HttpStatusCode.OK, "application/json", typeof(List<Cost>))]
    [OpenApiParameter(name: "accountId", In = ParameterLocation.Path, Required = true, Type = typeof(Guid))]
    [Function("GetCosts")]
    public async Task<HttpResponseData> GetCosts([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "Accounts/{accountId}/Costs")] HttpRequestData request,
        Guid accountId)
    {
        HttpResponseData response;

        try
        {
            response = request.CreateResponse(HttpStatusCode.OK);
            var responseBody = await _costService.GetCostsAsync(accountId);

            await response.WriteAsJsonAsync(responseBody);
        }
        catch (Exception ex)
        {
            _logger.LogError(new EventId(Convert.ToInt32(DateTime.UtcNow.ToString("HHmmss"))), ex, ex.Message);
            throw;
        }
        return response;
    }
    
    [OpenApiOperation(operationId: "GetCost", tags: new[] { "Costs" }, Summary = "")]
    [OpenApiResponseWithBody(HttpStatusCode.OK, "application/json", typeof(Cost))]
    [OpenApiParameter(name: "accountId", In = ParameterLocation.Path, Required = true, Type = typeof(Guid))]
    [OpenApiParameter(name: "costId", In = ParameterLocation.Path, Required = true, Type = typeof(Guid))]
    [Function("GetCost")]
    public async Task<HttpResponseData> GetCost([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "Accounts/{accountId}/Costs/{costId}")] HttpRequestData request,
        Guid accountId,
        Guid costId)
    {
        HttpResponseData response;

        try
        {
            response = request.CreateResponse(HttpStatusCode.OK);
            var responseBody = await _costService.GetCostAsync(accountId, costId);

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

    [OpenApiOperation(operationId: "UpdateCost", tags: new[] { "Costs" }, Summary = "")]
    [OpenApiRequestBody("application/json", typeof(UpdateCostRequest))]
    [OpenApiResponseWithBody(HttpStatusCode.OK, "application/json", typeof(Cost))]
    [OpenApiParameter(name: "accountId", In = ParameterLocation.Path, Required = true, Type = typeof(Guid))]
    [OpenApiParameter(name: "costId", In = ParameterLocation.Path, Required = true, Type = typeof(Guid))]
    [Function("UpdateCost")]
    public async Task<HttpResponseData> UpdateAccount([HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "Accounts/{accountId}/Costs/{costId}")] HttpRequestData request,
        Guid accountId,
        Guid costId)
    {
        HttpResponseData response;

        try
        {
            var requestContent = await request.ReadAsStringAsync();
            var requestBody = JsonSerializer.Deserialize<UpdateCostRequest>(requestContent);

            response = request.CreateResponse(HttpStatusCode.OK);
            var responseBody = await _costService.UpdateCostAsync(accountId, costId, requestBody);

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