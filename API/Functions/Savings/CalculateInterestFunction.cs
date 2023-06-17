using System.Net;
using ChrisUsher.MoveMate.API.Services.Accounts;
using ChrisUsher.MoveMate.API.Services.Savings;
using ChrisUsher.MoveMate.Shared.DTOs.Savings;
using Microsoft.OpenApi.Models;

namespace ChrisUsher.MoveMate.API.Functions.Savings;

public class CalculateInterestFunction : HttpFunction
{
    private readonly ILogger _logger;
    private readonly AccountService _accountService;
    private readonly SavingsService _savingsService;
    private readonly InterestService _interestService;

    public CalculateInterestFunction(
        ILoggerFactory loggerFactory,
        AccountService accountService,
        SavingsService savingsService,
        InterestService interestService)
    {
        _logger = loggerFactory.CreateLogger<CalculateInterestFunction>();
        _accountService = accountService;
        _savingsService = savingsService;
        _interestService = interestService;
    }
    
    [OpenApiOperation(operationId: "CalculateInterest", tags: new[] { "Savings Calculations" }, Summary = "")]
    [OpenApiResponseWithBody(HttpStatusCode.OK, "application/json", typeof(SavingsInterestBreakdown))]
    [OpenApiParameter(name: "accountId", In = ParameterLocation.Path, Required = true, Type = typeof(Guid))]
    [OpenApiParameter(name: "savingsId", In = ParameterLocation.Path, Required = true, Type = typeof(Guid))]
    [Function("CalculateInterest")]
    public async Task<HttpResponseData> CalculateInterest([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "Accounts/{accountId}/Savings/{savingsId}/CalculateInterest")] HttpRequestData request,
        Guid accountId,
        Guid savingsId)
    {
        HttpResponseData response;

        try
        {
            response = request.CreateResponse(HttpStatusCode.OK);

            var account = await _accountService.GetAccountAsync(accountId);
            var savingsAccount = await _savingsService.GetSavingsAccountAsync(accountId, savingsId);

            var interestResponse = _interestService.CalculateInterest(savingsAccount, account.EstimatedSaleDate);
            await response.WriteAsJsonAsync(interestResponse);
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