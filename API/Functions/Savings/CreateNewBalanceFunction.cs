using ChrisUsher.MoveMate.API.Services.Savings;
using ChrisUsher.MoveMate.Shared.DTOs.Savings;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;

namespace ChrisUsher.MoveMate.API.Functions.Savings;

public class CreateNewBalanceFunction : HttpFunction
{
    private readonly ILogger _logger;
    private readonly SavingsService _savingsService;

    public CreateNewBalanceFunction(
        ILoggerFactory loggerFactory,
        SavingsService savingsService)
    {
        _logger = loggerFactory.CreateLogger<CreateNewBalanceFunction>();
        _savingsService = savingsService;
    }
    
    [OpenApiOperation(operationId: "CreateNewBalance", tags: new[] { "Savings" }, Summary = "")]
    [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "x-functions-key", In = OpenApiSecurityLocationType.Header)]
    [OpenApiResponseWithBody(HttpStatusCode.OK, "application/json", typeof(SavingsAccount))]
    [OpenApiParameter(name: "accountId", In = ParameterLocation.Path, Required = true, Type = typeof(Guid))]
    [OpenApiParameter(name: "savingsId", In = ParameterLocation.Path, Required = true, Type = typeof(Guid))]
    [OpenApiParameter(name: "balance", In = ParameterLocation.Query, Required = true, Type = typeof(double))]
    [Function("CreateNewBalance")]
    public async Task<HttpResponseData> CreateNewBalance([HttpTrigger(AuthorizationLevel.Function, "post", Route = "Accounts/{accountId}/Savings/{savingsId}/Balance")] HttpRequestData request,
        Guid accountId,
        Guid savingsId,
        double balance)
    {
        HttpResponseData response;

        try
        {
            response = request.CreateResponse(HttpStatusCode.OK);
            var responseBody = await _savingsService.AddNewBalanceAsync(accountId, savingsId, balance);

            await response.WriteAsJsonAsync(responseBody);
        }
        catch(InvalidRequestException invalidRequest)
        {
            response = request.CreateResponse(HttpStatusCode.BadRequest);
            await response.WriteStringAsync(invalidRequest.Message);
            return response;
        }
        catch(DataNotFoundException dataNotFound)
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