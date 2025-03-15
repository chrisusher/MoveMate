using ChrisUsher.MoveMate.API.Services.Accounts;
using ChrisUsher.MoveMate.Shared.DTOs.Accounts;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;

namespace ChrisUsher.MoveMate.API.Functions.Accounts
{
    public class AccountFunctions
    {
        private readonly ILogger _logger;
        private readonly AccountService _accountService;

        public AccountFunctions(
            ILoggerFactory loggerFactory,
            AccountService accountService)
        {
            _logger = loggerFactory.CreateLogger<AccountFunctions>();
            _accountService = accountService;
        }

        [OpenApiOperation(operationId: "CreateAccount", tags: new[] { "Accounts" }, Summary = "")]
        [OpenApiSecurity("Http", SecuritySchemeType.Http, In = OpenApiSecurityLocationType.Header, Scheme = OpenApiSecuritySchemeType.Bearer, BearerFormat = "FunctionKey")]
        [OpenApiRequestBody("application/json", typeof(CreateAccountRequest))]
        [OpenApiResponseWithBody(HttpStatusCode.OK, "application/json", typeof(Account))]
        [Function("CreateAccount")]
        public async Task<HttpResponseData> CreateAccount([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "Accounts")] HttpRequestData request)
        {
            HttpResponseData response;

            try
            {
                var requestContent = await request.ReadAsStringAsync();
                var requestBody = JsonSerializer.Deserialize<CreateAccountRequest>(requestContent);

                response = request.CreateResponse(HttpStatusCode.OK);
                var responseBody = await _accountService.CreateAccountAsync(requestBody);

                await response.WriteAsJsonAsync(responseBody);
            }
            catch (Exception ex)
            {
                _logger.LogError(new EventId(Convert.ToInt32(DateTime.UtcNow.ToString("HHmmss"))), ex, ex.Message);
                throw;
            }

            return response;
        }

        [OpenApiOperation(operationId: "GetAccount", tags: new[] { "Accounts" }, Summary = "")]
        [OpenApiSecurity("Http", SecuritySchemeType.Http, In = OpenApiSecurityLocationType.Header, Scheme = OpenApiSecuritySchemeType.Bearer, BearerFormat = "FunctionKey")]
        [OpenApiResponseWithBody(HttpStatusCode.OK, "application/json", typeof(Account))]
        [OpenApiParameter(name: "accountId", In = ParameterLocation.Path, Required = true, Type = typeof(Guid))]
        [Function("GetAccount")]
        public async Task<HttpResponseData> GetAccount([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "Accounts/{accountId}")] HttpRequestData request,
            Guid accountId)
        {
            HttpResponseData response;

            try
            {
                response = request.CreateResponse(HttpStatusCode.OK);
                var responseBody = await _accountService.GetAccountAsync(accountId);

                await response.WriteAsJsonAsync(responseBody);
            }
            catch (Exception ex)
            {
                _logger.LogError(new EventId(Convert.ToInt32(DateTime.UtcNow.ToString("HHmmss"))), ex, ex.Message);
                throw;
            }

            return response;
        }

        [OpenApiOperation(operationId: "UpdateAccount", tags: new[] { "Accounts" }, Summary = "")]
        [OpenApiSecurity("Http", SecuritySchemeType.Http, In = OpenApiSecurityLocationType.Header, Scheme = OpenApiSecuritySchemeType.Bearer, BearerFormat = "FunctionKey")]
        [OpenApiRequestBody("application/json", typeof(UpdateAccountRequest))]
        [OpenApiResponseWithBody(HttpStatusCode.OK, "application/json", typeof(Account))]
        [OpenApiParameter(name: "accountId", In = ParameterLocation.Path, Required = true, Type = typeof(Guid))]
        [Function("UpdateAccount")]
        public async Task<HttpResponseData> UpdateAccount([HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "Accounts/{accountId}")] HttpRequestData request,
            Guid accountId)
        {
            HttpResponseData response;

            try
            {
                var requestContent = await request.ReadAsStringAsync();
                var requestBody = JsonSerializer.Deserialize<UpdateAccountRequest>(requestContent);

                response = request.CreateResponse(HttpStatusCode.OK);
                var responseBody = await _accountService.UpdateAccountAsync(accountId, requestBody);

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
}
