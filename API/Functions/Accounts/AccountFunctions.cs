using System.Net;
using System.Text.Json;
using ChrisUsher.MoveMate.API.Database.Accounts;
using ChrisUsher.MoveMate.API.Services.Accounts;

namespace ChrisUsher.MoveMate.API.Functions
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
        [OpenApiRequestBody("application/json", typeof(AccountTable))]
        [OpenApiResponseWithBody(HttpStatusCode.OK, "application/json", typeof(AccountTable))]
        [Function("CreateAccount")]
        public async Task<HttpResponseData> CreateAccount([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "Accounts")] HttpRequestData request)
        {
            HttpResponseData response;

            try
            {
                var requestContent = await request.ReadAsStringAsync();
                var requestBody = JsonSerializer.Deserialize<AccountTable>(requestContent);

                response = request.CreateResponse(HttpStatusCode.OK);
                var responseBody = await _accountService.CreateAccountAsync(requestBody);

                await response.WriteAsJsonAsync(responseBody);
            }
            catch (Exception ex)
            {
                _logger.LogError(new EventId(Convert.ToInt32(DateTime.UtcNow.ToString("yyyyMMddHHmmss"))), ex, ex.Message);
                throw;
            }

            return response;
        }

        [OpenApiOperation(operationId: "GetAccount", tags: new[] { "Accounts" }, Summary = "")]
        [OpenApiResponseWithBody(HttpStatusCode.OK, "application/json", typeof(AccountTable))]
        [OpenApiParameter("accountId")]
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
                _logger.LogError(new EventId(Convert.ToInt32(DateTime.UtcNow.ToString("yyyyMMddHHmmss"))), ex, ex.Message);
                throw;
            }

            return response;
        }
    }
}
