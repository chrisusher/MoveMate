using System.Net;

namespace ChrisUsher.MoveMate.API.Functions
{
    public class CalculateStampDuty
    {
        private readonly ILogger _logger;

        public CalculateStampDuty(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<CalculateStampDuty>();
        }

        [OpenApiOperation(operationId: "CalculateStampDuty", Summary = "Calculates the Stamp Duty for a Property")]
        [Function("CalculateStampDuty")]
        public HttpResponseData Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "")] HttpRequestData req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");

            response.WriteString("Welcome to Azure Functions!");

            return response;
        }
    }
}
