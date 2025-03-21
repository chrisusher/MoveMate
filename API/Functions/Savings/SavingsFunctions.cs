﻿using ChrisUsher.MoveMate.API.Services.Savings;
using ChrisUsher.MoveMate.Shared.DTOs.Savings;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;

namespace ChrisUsher.MoveMate.API.Functions.Savings;

public class SavingsFunctions
{
    private readonly ILogger _logger;
    private readonly SavingsService _savingsService;

    public SavingsFunctions(
        ILoggerFactory loggerFactory,
        SavingsService savingsService)
    {
        _logger = loggerFactory.CreateLogger<SavingsFunctions>();
        _savingsService = savingsService;
    }
    
    [OpenApiOperation(operationId: "CreateSavingsAccount", tags: new[] { "Savings" }, Summary = "")]
    [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "x-functions-key", In = OpenApiSecurityLocationType.Header)]
    [OpenApiRequestBody("application/json", typeof(CreateSavingsAccountRequest))]
    [OpenApiResponseWithBody(HttpStatusCode.OK, "application/json", typeof(SavingsAccount))]
    [OpenApiParameter(name: "accountId", In = ParameterLocation.Path, Required = true, Type = typeof(Guid))]
    [Function("CreateSavingsAccount")]
    public async Task<HttpResponseData> CreateSavingsAccount([HttpTrigger(AuthorizationLevel.Function, "post", Route = "Accounts/{accountId}/Savings")] HttpRequestData request,
        Guid accountId)
    {
        HttpResponseData response;

        try
        {
            var requestContent = await request.ReadAsStringAsync();
            var requestBody = JsonSerializer.Deserialize<CreateSavingsAccountRequest>(requestContent);

            response = request.CreateResponse(HttpStatusCode.OK);
            var responseBody = await _savingsService.CreateSavingsAccountAsync(accountId, requestBody);

            await response.WriteAsJsonAsync(responseBody);
        }
        catch (Exception ex)
        {
            _logger.LogError(new EventId(Convert.ToInt32(DateTime.UtcNow.ToString("HHmmss"))), ex, ex.Message);
            throw;
        }

        return response;
    }
    
    [OpenApiOperation(operationId: "GetSavingsAccounts", tags: new[] { "Savings" }, Summary = "")]
    [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "x-functions-key", In = OpenApiSecurityLocationType.Header)]
    [OpenApiResponseWithBody(HttpStatusCode.OK, "application/json", typeof(List<SavingsAccount>))]
    [OpenApiParameter(name: "accountId", In = ParameterLocation.Path, Required = true, Type = typeof(Guid))]
    [Function("GetSavingsAccounts")]
    public async Task<HttpResponseData> GetSavingsAccounts([HttpTrigger(AuthorizationLevel.Function, "get", Route = "Accounts/{accountId}/Savings")] HttpRequestData request,
        Guid accountId)
    {
        HttpResponseData response;

        try
        {
            response = request.CreateResponse(HttpStatusCode.OK);
            var responseBody = await _savingsService.GetSavingsAccountsAsync(accountId);

            await response.WriteAsJsonAsync(responseBody);
        }
        catch (Exception ex)
        {
            _logger.LogError(new EventId(Convert.ToInt32(DateTime.UtcNow.ToString("HHmmss"))), ex, ex.Message);
            throw;
        }
        return response;
    }
    
    [OpenApiOperation(operationId: "GetSavingsAccount", tags: new[] { "Savings" }, Summary = "")]
    [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "x-functions-key", In = OpenApiSecurityLocationType.Header)]
    [OpenApiResponseWithBody(HttpStatusCode.OK, "application/json", typeof(SavingsAccount))]
    [OpenApiParameter(name: "accountId", In = ParameterLocation.Path, Required = true, Type = typeof(Guid))]
    [OpenApiParameter(name: "savingsId", In = ParameterLocation.Path, Required = true, Type = typeof(Guid))]
    [Function("GetSavingsAccount")]
    public async Task<HttpResponseData> GetSavingsAccount([HttpTrigger(AuthorizationLevel.Function, "get", Route = "Accounts/{accountId}/Savings/{savingsId}")] HttpRequestData request,
        Guid accountId,
        Guid savingsId)
    {
        HttpResponseData response;

        try
        {
            response = request.CreateResponse(HttpStatusCode.OK);
            var responseBody = await _savingsService.GetSavingsAccountAsync(accountId, savingsId);

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

    [OpenApiOperation(operationId: "UpdateSavingsAccount", tags: new[] { "Savings" }, Summary = "")]
    [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "x-functions-key", In = OpenApiSecurityLocationType.Header)]
    [OpenApiRequestBody("application/json", typeof(UpdateSavingsAccountRequest))]
    [OpenApiResponseWithBody(HttpStatusCode.OK, "application/json", typeof(SavingsAccount))]
    [OpenApiParameter(name: "accountId", In = ParameterLocation.Path, Required = true, Type = typeof(Guid))]
    [OpenApiParameter(name: "savingsId", In = ParameterLocation.Path, Required = true, Type = typeof(Guid))]
    [Function("UpdateSavingsAccount")]
    public async Task<HttpResponseData> UpdateAccount([HttpTrigger(AuthorizationLevel.Function, "put", Route = "Accounts/{accountId}/Savings/{savingsId}")] HttpRequestData request,
        Guid accountId,
        Guid savingsId)
    {
        HttpResponseData response;

        try
        {
            var requestContent = await request.ReadAsStringAsync();
            var requestBody = JsonSerializer.Deserialize<UpdateSavingsAccountRequest>(requestContent);

            response = request.CreateResponse(HttpStatusCode.OK);
            var responseBody = await _savingsService.UpdateSavingsAccountAsync(accountId, savingsId, requestBody);

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