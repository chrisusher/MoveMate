using ChrisUsher.MoveMate.API.Repositories;
using ChrisUsher.MoveMate.API.Services.Accounts;
using ChrisUsher.MoveMate.API.Services.Mortgages;
using ChrisUsher.MoveMate.API.Services.Savings;
using ChrisUsher.MoveMate.API.Services.StampDuty;
using ChrisUsher.MoveMate.API.Services.Properties;
using ChrisUsher.MoveMate.API.Services.Reports;
using Microsoft.Extensions.DependencyInjection;
using ChrisUsher.MoveMate.API.Services.Costs;

namespace ChrisUsher.MoveMate.API.Services
{
    public static class MoveMateServiceExtensions
    {
        public static IServiceCollection AddMoveMateServices(this IServiceCollection services)
        {
            #region Repositories
            
            services.AddSingleton<AccountRepository>();
            services.AddSingleton<CostRepository>();
            services.AddSingleton<PropertyRepository>();
            services.AddSingleton<SavingsRepository>();

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
        
            #endregion

            return services;
        }
    }
}