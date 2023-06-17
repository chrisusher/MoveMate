using System.Net;
using ChrisUsher.MoveMate.API.Services.Mortgages;
using ChrisUsher.MoveMate.Shared.DTOs.Savings;
using Microsoft.OpenApi.Models;

namespace ChrisUsher.MoveMate.API.Functions.Savings;

public class CalculateMortgagePaymentFunction : HttpFunction
{
    private readonly ILogger _logger;
    private readonly MortgagePaymentService _mortgagePaymentService;

    public CalculateMortgagePaymentFunction(
        ILoggerFactory loggerFactory,
        MortgagePaymentService mortgagePaymentService)
    {
        _logger = loggerFactory.CreateLogger<CalculateMortgagePaymentFunction>();
        _mortgagePaymentService = mortgagePaymentService;
    }
    
    [OpenApiOperation(operationId: "CalculateMortgagePayment", tags: new[] { "Property Calculations" }, Summary = "")]
    [OpenApiResponseWithBody(HttpStatusCode.OK, "application/json", typeof(SavingsInterestBreakdown))]
    [OpenApiParameter(name: "loanAmount", In = ParameterLocation.Query, Required = true, Type = typeof(double))]
    [OpenApiParameter(name: "interestRate", In = ParameterLocation.Query, Required = true, Type = typeof(double))]
    [OpenApiParameter(name: "years", In = ParameterLocation.Query, Required = true, Type = typeof(int))]
    [Function("CalculateMortgagePayment")]
    public async Task<HttpResponseData> CalculateInterest([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "Mortgages/MonthlyPayments")] HttpRequestData request,
        double loanAmount,
        double interestRate,
        int years)
    {
        HttpResponseData response;

        try
        {
            response = request.CreateResponse(HttpStatusCode.OK);

            var interestResponse = _mortgagePaymentService.CalculateMonthlyMortgagePayment(loanAmount, interestRate, years);
            await response.WriteAsJsonAsync(interestResponse);
        }
        catch (Exception ex)
        {
            _logger.LogError(new EventId(Convert.ToInt32(DateTime.UtcNow.ToString("HHmmss"))), ex, ex.Message);
            throw;
        }
        return response;
    }
}