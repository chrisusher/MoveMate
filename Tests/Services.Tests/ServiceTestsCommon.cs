using ChrisUsher.MoveMate.API.Database;
using ChrisUsher.MoveMate.API.Repositories;
using ChrisUsher.MoveMate.API.Services.Costs;
using ChrisUsher.MoveMate.API.Services.StampDuty;
using Microsoft.EntityFrameworkCore;

namespace Services.Tests
{
    public class ServiceTestsCommon
    {
        private static ServiceProvider _services;

        public static ServiceProvider Services 
        {
            get
            {
                if(_services == null)
                {
                    _services = RegisterServices();
                }
                return _services;
            }
        }

        private static ServiceProvider RegisterServices()
        {
            var services = new ServiceCollection();

            services.AddDbContext<DatabaseContext>(options => 
            {
                options.UseCosmos(Environment.GetEnvironmentVariable("MOVEMATE_COSMOS_CONNECTIONSTRING"), "movemate-test");

                options.EnableSensitiveDataLogging();

                #if DEBUG
                
                options.EnableDetailedErrors();
                options.LogTo(Console.WriteLine);

                #endif
            });

            services.AddSingleton<CostRepository>();
            services.AddSingleton<StampDutyService>();
            services.AddSingleton<CostService>();

            return services.BuildServiceProvider();
        }        
    }
}