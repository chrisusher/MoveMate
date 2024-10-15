using ChrisUsher.MoveMate.API.Services.Accounts;
using ChrisUsher.MoveMate.API.Services.Costs;
using ChrisUsher.MoveMate.API.Services.Mortgages;
using ChrisUsher.MoveMate.API.Services.Properties;
using ChrisUsher.MoveMate.API.Services.Reports;
using ChrisUsher.MoveMate.API.Services.Repositories;
using ChrisUsher.MoveMate.API.Services.Savings;
using ChrisUsher.MoveMate.API.Services.StampDuty;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Azure;

namespace ChrisUsher.MoveMate.API.Services
{
    public static class MoveMateServiceExtensions
    {
        public static IServiceCollection AddMoveMateServices(this IServiceCollection services, IConfiguration configuration)
        {
            #region Azure Services

            services.AddAzureClients(config => 
            {
                config.AddBlobServiceClient(configuration.GetConnectionString("AzureStorage"));
            });

            #endregion

            #region Repositories
            
            services.AddSingleton<AccountRepository>();
            services.AddSingleton<CostRepository>();
            services.AddSingleton<PropertyRepository>();
            services.AddSingleton<SavingsRepository>();
            services.AddSingleton<StockRepository>();
            services.AddSingleton<MigrationsRepository>();

            #endregion

            #region Services

            services.AddSingleton<AccountService>();
            services.AddSingleton<CostService>();
            services.AddSingleton<InterestService>();
            services.AddSingleton<MortgagePaymentService>();
            services.AddSingleton<PropertyService>();
            services.AddSingleton<ReportsService>();
            services.AddSingleton<SavingsService>();
            services.AddSingleton<StampDutyService>();
            services.AddSingleton<StockService>();
            services.AddSingleton<MigrationsService>();
        
            #endregion

            return services;
        }
    }
}