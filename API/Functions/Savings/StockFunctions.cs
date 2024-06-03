using System.Net;
using ChrisUsher.MoveMate.API.Services.Savings;
using ChrisUsher.MoveMate.Shared.DTOs.Savings.Stocks;
using Microsoft.OpenApi.Models;

namespace ChrisUsher.MoveMate.API.Functions.Stock;

public class StockFunctions
{
    private readonly ILogger _logger;
    private readonly StockService _stockService;

    public StockFunctions(
        ILoggerFactory loggerFactory,
        StockService StockService)
    {
        _logger = loggerFactory.CreateLogger<StockFunctions>();
        _stockService = StockService;
    }
    
    [OpenApiOperation(operationId: "CreateStockAccount", tags: new[] { "Stocks" }, Summary = "")]
    [OpenApiRequestBody("application/json", typeof(CreateStocksAndSharesRequest))]
    [OpenApiResponseWithBody(HttpStatusCode.OK, "application/json", typeof(StocksAndSharesDetails))]
    [OpenApiParameter(name: "accountId", In = ParameterLocation.Path, Required = true, Type = typeof(Guid))]
    [OpenApiParameter(name: "savingsId", In = ParameterLocation.Path, Required = true, Type = typeof(Guid))]
    [Function("CreateStockAccount")]
    public async Task<HttpResponseData> CreateStockAccount([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "Accounts/{accountId}/Savings/{savingsId}/Stock")] HttpRequestData request,
        Guid accountId,
        Guid savingsId)
    {
        HttpResponseData response;

        try
        {
            var requestContent = await request.ReadAsStringAsync();
            var requestBody = JsonSerializer.Deserialize<CreateStocksAndSharesRequest>(requestContent);

            response = request.CreateResponse(HttpStatusCode.OK);
            var responseBody = await _stockService.CreateStockAsync(accountId, savingsId, requestBody);

            await response.WriteAsJsonAsync(responseBody);
        }
        catch (Exception ex)
        {
            _logger.LogError(new EventId(Convert.ToInt32(DateTime.UtcNow.ToString("HHmmss"))), ex, ex.Message);
            throw;
        }

        return response;
    }
    
    [OpenApiOperation(operationId: "GetStockAccounts", tags: new[] { "Stocks" }, Summary = "")]
    [OpenApiResponseWithBody(HttpStatusCode.OK, "application/json", typeof(List<StocksAndSharesDetails>))]
    [OpenApiParameter(name: "accountId", In = ParameterLocation.Path, Required = true, Type = typeof(Guid))]
    [OpenApiParameter(name: "savingsId", In = ParameterLocation.Path, Required = true, Type = typeof(Guid))]
    [Function("GetStockAccounts")]
    public async Task<HttpResponseData> GetStockAccounts([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "Accounts/{accountId}/Savings/{savingsId}/Stock")] HttpRequestData request,
        Guid accountId,
        Guid savingsId)
    {
        HttpResponseData response;

        try
        {
            response = request.CreateResponse(HttpStatusCode.OK);
            var responseBody = await _stockService.GetStocksAsync(savingsId);

            await response.WriteAsJsonAsync(responseBody);
        }
        catch (Exception ex)
        {
            _logger.LogError(new EventId(Convert.ToInt32(DateTime.UtcNow.ToString("HHmmss"))), ex, ex.Message);
            throw;
        }
        return response;
    }
    
    [OpenApiOperation(operationId: "GetStockAccount", tags: new[] { "Stocks" }, Summary = "")]
    [OpenApiResponseWithBody(HttpStatusCode.OK, "application/json", typeof(StocksAndSharesDetails))]
    [OpenApiParameter(name: "accountId", In = ParameterLocation.Path, Required = true, Type = typeof(Guid))]
    [OpenApiParameter(name: "savingsId", In = ParameterLocation.Path, Required = true, Type = typeof(Guid))]
    [OpenApiParameter(name: "stockId", In = ParameterLocation.Path, Required = true, Type = typeof(Guid))]
    [Function("GetStockAccount")]
    public async Task<HttpResponseData> GetStockAccount([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "Accounts/{accountId}/Savings/{savingsId}/Stock/{StockId}")] HttpRequestData request,
        Guid accountId,
        Guid savingsId,
        Guid stockId)
    {
        HttpResponseData response;

        try
        {
            response = request.CreateResponse(HttpStatusCode.OK);
            var responseBody = await _stockService.GetStockAsync(savingsId, stockId);

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

    [OpenApiOperation(operationId: "UpdateStockAccount", tags: new[] { "Stocks" }, Summary = "")]
    [OpenApiRequestBody("application/json", typeof(UpdateStocksAndSharesRequest))]
    [OpenApiResponseWithBody(HttpStatusCode.OK, "application/json", typeof(StocksAndSharesDetails))]
    [OpenApiParameter(name: "accountId", In = ParameterLocation.Path, Required = true, Type = typeof(Guid))]
    [OpenApiParameter(name: "savingsId", In = ParameterLocation.Path, Required = true, Type = typeof(Guid))]
    [OpenApiParameter(name: "stockId", In = ParameterLocation.Path, Required = true, Type = typeof(Guid))]
    [Function("UpdateStockAccount")]
    public async Task<HttpResponseData> UpdateAccount([HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "Accounts/{accountId}/Savings/{savingsId}/Stock/{StockId}")] HttpRequestData request,
        Guid accountId,
        Guid savingsId,
        Guid stockId)
    {
        HttpResponseData response;

        try
        {
            var requestContent = await request.ReadAsStringAsync();
            var requestBody = JsonSerializer.Deserialize<UpdateStocksAndSharesRequest>(requestContent);

            response = request.CreateResponse(HttpStatusCode.OK);
            var responseBody = await _stockService.UpdateStockAsync(accountId, savingsId, stockId, requestBody);

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