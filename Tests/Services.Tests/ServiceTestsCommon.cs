using ChrisUsher.MoveMate.API.Database;
using ChrisUsher.MoveMate.API.Services;
using ChrisUsher.MoveMate.API.Services.Accounts;
using ChrisUsher.MoveMate.API.Services.Properties;
using ChrisUsher.MoveMate.Shared.DTOs.Accounts;
using ChrisUsher.MoveMate.Shared.DTOs.Properties;
using Microsoft.EntityFrameworkCore;

namespace Services.Tests
{
    public class ServiceTestsCommon
    {
        private static Account _account;
        private static Property _purchaseProperty;
        private static ServiceProvider _services;

        public static Account DefaultAccount
        {
            get
            {
                if (_account == null)
                {
                    _account = Services.GetService<AccountService>()
                        .GetAccountAsync(Guid.Parse("7570e5af-0e67-40b0-af4b-80ac362de7e1"))
                        .Result;
                }
                return _account;
            }
        }

        public static Property DefaultPurchaseProperty
        {
            get
            {
                if (_purchaseProperty == null)
                {
                    _purchaseProperty = Services.GetService<PropertyService>()
                        .GetPropertiesAsync(DefaultAccount.AccountId, PropertyType.ToPurchase)
                        .Result
                        .First();
                }
                return _purchaseProperty;
            }
        }

        public static ServiceProvider Services
        {
            get
            {
                if (_services == null)
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

            services.AddMoveMateServices();

            return services.BuildServiceProvider();
        }
    }
}