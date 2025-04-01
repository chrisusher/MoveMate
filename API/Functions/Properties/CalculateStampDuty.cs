using ChrisUsher.MoveMate.API.Services.Properties;
using ChrisUsher.MoveMate.API.Services.StampDuty;
using ChrisUsher.MoveMate.Shared.DTOs.Properties.StampDuty;
using ChrisUsher.MoveMate.Shared.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;

namespace ChrisUsher.MoveMate.API.Functions.Properties
{
    public class CalculateStampDuty : HttpFunction
    {
        private readonly ILogger _logger;
        private readonly StampDutyService _stampDutyService;
        private readonly PropertyService _propertyService;

        public CalculateStampDuty(
            ILoggerFactory loggerFactory,
            StampDutyService stampDutyService,
            PropertyService propertyService)
        {
            _logger = loggerFactory.CreateLogger<CalculateStampDuty>();
            _stampDutyService = stampDutyService;
            _propertyService = propertyService;
        }

        [OpenApiOperation(operationId: "CalculateStampDuty", tags: new[] { "Property Calculations" }, Summary = "Calculates the Stamp Duty for a Property")]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "x-functions-key", In = OpenApiSecurityLocationType.Header)]
        [OpenApiRequestBody("application/json", typeof(StampDutyRequest))]
        [OpenApiResponseWithBody(HttpStatusCode.OK, "application/json", typeof(StampDutyResponse))]
        [OpenApiParameter(name: "accountId", In = ParameterLocation.Path,  Required = true, Type = typeof(Guid))]
        [OpenApiParameter(name: "propertyId", In = ParameterLocation.Path, Required = true, Type = typeof(Guid))]
        [OpenApiParameter(name: "caseModel", In = ParameterLocation.Query, Required = false, Type = typeof(CaseType))]
        [Function("CalculateStampDuty")]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Function, "post", Route = "Accounts/{accountId}/Properties/{propertyId}/Calculations/StampDuty")] HttpRequestData request,
            Guid accountId,
            Guid propertyId,
            [FromQuery] string caseModel)
        {
            HttpResponseData response;

            try
            {
                var caseType = CaseType.MiddleCase;

                if(!string.IsNullOrEmpty(caseModel))
                {
                    caseType = Enum.Parse<CaseType>(caseModel, true);
                }

                var property = await _propertyService.GetPropertyAsync(accountId, propertyId);

                var requestContent = await request.ReadAsStringAsync();
                var requestBody = JsonSerializer.Deserialize<StampDutyRequest>(requestContent, JsonOptions);

                response = request.CreateResponse(HttpStatusCode.OK);
                var stampDutyBody = _stampDutyService.CalculateStampDuty(property, requestBody, caseType);

                await response.WriteAsJsonAsync(stampDutyBody);
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
