using ChrisUsher.MoveMate.API.Services.Savings;
using ChrisUsher.MoveMate.Shared.DTOs.Savings.Stocks;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;

namespace ChrisUsher.MoveMate.API.Functions.Savings;

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
    [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "x-functions-key", In = OpenApiSecurityLocationType.Header)]
    [OpenApiRequestBody("application/json", typeof(CreateStocksAndSharesRequest))]
    [OpenApiResponseWithBody(HttpStatusCode.OK, "application/json", typeof(StocksAndSharesDetails))]
    [OpenApiParameter(name: "accountId", In = ParameterLocation.Path, Required = true, Type = typeof(Guid))]
    [OpenApiParameter(name: "savingsId", In = ParameterLocation.Path, Required = true, Type = typeof(Guid))]
    [Function("CreateStockAccount")]
    public async Task<HttpResponseData> CreateStockAccountAsync([HttpTrigger(AuthorizationLevel.Function, "post", Route = "Accounts/{accountId}/Savings/{savingsId}/Stocks")] HttpRequestData request,
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
    
    [OpenApiOperation(operationId: "GetStockAccounts", tags: new[] { "Stocks" }, Summary = "")]
    [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "x-functions-key", In = OpenApiSecurityLocationType.Header)]
    [OpenApiResponseWithBody(HttpStatusCode.OK, "application/json", typeof(List<StocksAndSharesDetails>))]
    [OpenApiParameter(name: "accountId", In = ParameterLocation.Path, Required = true, Type = typeof(Guid))]
    [OpenApiParameter(name: "savingsId", In = ParameterLocation.Path, Required = true, Type = typeof(Guid))]
    [Function("GetStockAccounts")]
    public async Task<HttpResponseData> GetStockAccountsAsync([HttpTrigger(AuthorizationLevel.Function, "get", Route = "Accounts/{accountId}/Savings/{savingsId}/Stocks")] HttpRequestData request,
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
    
    [OpenApiOperation(operationId: "GetStockAccount", tags: new[] { "Stocks" }, Summary = "")]
    [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "x-functions-key", In = OpenApiSecurityLocationType.Header)]
    [OpenApiResponseWithBody(HttpStatusCode.OK, "application/json", typeof(StocksAndSharesDetails))]
    [OpenApiParameter(name: "accountId", In = ParameterLocation.Path, Required = true, Type = typeof(Guid))]
    [OpenApiParameter(name: "savingsId", In = ParameterLocation.Path, Required = true, Type = typeof(Guid))]
    [OpenApiParameter(name: "stockId", In = ParameterLocation.Path, Required = true, Type = typeof(Guid))]
    [Function("GetStockAccount")]
    public async Task<HttpResponseData> GetStockAccountAsync([HttpTrigger(AuthorizationLevel.Function, "get", Route = "Accounts/{accountId}/Savings/{savingsId}/Stocks/{stockId}")] HttpRequestData request,
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
    [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "x-functions-key", In = OpenApiSecurityLocationType.Header)]
    [OpenApiRequestBody("application/json", typeof(UpdateStocksAndSharesRequest))]
    [OpenApiResponseWithBody(HttpStatusCode.OK, "application/json", typeof(StocksAndSharesDetails))]
    [OpenApiParameter(name: "accountId", In = ParameterLocation.Path, Required = true, Type = typeof(Guid))]
    [OpenApiParameter(name: "savingsId", In = ParameterLocation.Path, Required = true, Type = typeof(Guid))]
    [OpenApiParameter(name: "stockId", In = ParameterLocation.Path, Required = true, Type = typeof(Guid))]
    [Function("UpdateStockAccount")]
    public async Task<HttpResponseData> UpdateStockAccountAsync([HttpTrigger(AuthorizationLevel.Function, "put", Route = "Accounts/{accountId}/Savings/{savingsId}/Stocks/{stockId}")] HttpRequestData request,
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

    [OpenApiOperation(operationId: "AddStockBalance", tags: new[] { "Stocks" }, Summary = "")]
    [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "x-functions-key", In = OpenApiSecurityLocationType.Header)]
    [OpenApiResponseWithBody(HttpStatusCode.OK, "application/json", typeof(StocksAndSharesDetails))]
    [OpenApiParameter(name: "accountId", In = ParameterLocation.Path, Required = true, Type = typeof(Guid))]
    [OpenApiParameter(name: "savingsId", In = ParameterLocation.Path, Required = true, Type = typeof(Guid))]
    [OpenApiParameter(name: "stockId", In = ParameterLocation.Path, Required = true, Type = typeof(Guid))]
    [OpenApiParameter(name: "currentBalance", In = ParameterLocation.Query, Required = true, Type = typeof(double))]
    [OpenApiParameter(name: "additionalInvestment", In = ParameterLocation.Query, Required = true, Type = typeof(double))]
    [Function("AddStockBalance")]
    public async Task<HttpResponseData> AddStockBalanceAsync([HttpTrigger(AuthorizationLevel.Function, "post", Route = "Accounts/{accountId}/Savings/{savingsId}/Stocks/{stockId}/Balance")] HttpRequestData request,
        Guid accountId,
        Guid savingsId,
        Guid stockId,
        double currentBalance,
        double additionalInvestment)
    {
        HttpResponseData response;

        try
        {
            var requestContent = await request.ReadAsStringAsync();

            response = request.CreateResponse(HttpStatusCode.OK);
            var responseBody = await _stockService.AddBalanceToStockAsync(savingsId, stockId, currentBalance, additionalInvestment);

            await response.WriteAsJsonAsync(responseBody);
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