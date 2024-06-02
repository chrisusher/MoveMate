using Azure.Storage.Blobs;
using Bogus;
using ChrisUsher.MoveMate.API.Database;
using ChrisUsher.MoveMate.API.Services;
using ChrisUsher.MoveMate.API.Services.Accounts;
using ChrisUsher.MoveMate.API.Services.Properties;
using ChrisUsher.MoveMate.Shared.DTOs.Accounts;
using ChrisUsher.MoveMate.Shared.DTOs.Properties;
using Ductus.FluentDocker.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Services.Tests
{
    public static class ServiceTestsCommon
    {
        private static Faker _faker;
        private static Account _account;
        private static Property _currentProperty;
        private static Property _purchaseProperty;
        private static ServiceProvider _services;

        public static IConfigurationRoot Configuration { get; internal set; }

        public static Account DefaultAccount
        {
            get
            {
                if (_account == null)
                {
                    _account = Services.GetService<AccountService>()
                        .CreateAccountAsync(new()
                        {
                            Email = Faker.Internet.Email()
                        }).Result;
                }
                return _account;
            }
        }

        public static Property DefaultCurrentProperty
        {
            get
            {
                if (_currentProperty == null)
                {
                    _currentProperty = Services.GetService<PropertyService>()
                        .CreatePropertyAsync(DefaultAccount.AccountId, new()
                        {
                            PropertyType = PropertyType.Current,
                            Name = "Our House",
                            MaxValue = 300_000,
                            MinValue = 275_000,
                            Equity = new()
                            {
                                RemainingMortgage = 100_000,
                                Updated = DateTime.UtcNow
                            }
                        }).Result;
                }
                return _currentProperty;
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

        public static ICompositeService DockerServices { get; internal set; }

        public static Faker Faker
        {
            get
            {
                if (_faker == null)
                {
                    _faker = new Faker();
                }
                return _faker;
            }
        }

        public static ServiceProvider Services
        {
            get
            {
                if (_services == null)
                {
                    _services = RegisterServicesAsync().Result;
                }
                return _services;
            }
        }

        private static async Task<ServiceProvider> RegisterServicesAsync()
        {
            var services = new ServiceCollection();

            services.AddDbContext<DatabaseContext>(options =>
            {
                options.UseMongoDB(Configuration.GetConnectionString("Database"), "movemate-test");

                options.EnableSensitiveDataLogging();

#if DEBUG

                options.EnableDetailedErrors();
                options.LogTo(Console.WriteLine);

#endif
            });

            services.AddMoveMateServices(Configuration);

            var serviceCollection = services.BuildServiceProvider();

            var blobServiceClient = serviceCollection.GetRequiredService<BlobServiceClient>();
            await blobServiceClient.CreateBlobContainerAsync("outputcache");

            return serviceCollection;
        }
    }
}