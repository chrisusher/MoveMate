using System.Net;
using System.Text.Json;
using ChrisUsher.MoveMate.API.Services.StampDuty;
using ChrisUsher.MoveMate.Shared.StampDuty;

namespace ChrisUsher.MoveMate.API.Functions
{
    public class CalculateStampDuty
    {
        private readonly ILogger _logger;
        private readonly StampDutyService _stampDutyService;

        public CalculateStampDuty(
            ILoggerFactory loggerFactory,
            StampDutyService stampDutyService)
        {
            _logger = loggerFactory.CreateLogger<CalculateStampDuty>();
            _stampDutyService = stampDutyService;
        }

        [OpenApiOperation(operationId: "CalculateStampDuty", Summary = "Calculates the Stamp Duty for a Property")]
        [OpenApiRequestBody("application/json", typeof(StampDutyRequest))]
        [OpenApiResponseWithBody(HttpStatusCode.OK, "application/json", typeof(StampDutyResponse))]
        [OpenApiParameter("propertyId")]
        [Function("CalculateStampDuty")]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "Properties/{propertyId}/Calculations/StampDuty")] HttpRequestData request,
            int propertyId)
        {
            HttpResponseData response;

            try
            {
                var requestContent = await request.ReadAsStringAsync();
                var requestBody = JsonSerializer.Deserialize<StampDutyRequest>(requestContent);

                response = request.CreateResponse(HttpStatusCode.OK);
                var stampDutyBody = _stampDutyService.CalculateStampDuty(requestBody);

                await response.WriteAsJsonAsync(stampDutyBody);
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
