using ChrisUsher.MoveMate.API.Database;
using Microsoft.Azure.Functions.Worker.Extensions.OpenApi.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


var config = new ConfigurationBuilder()
#if DEBUG
    .AddJsonFile("local.settings.json")
#endif
    .AddEnvironmentVariables()
    .Build();

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureOpenApi()
    .ConfigureServices(services =>
    {
        services.AddDbContext<DatabaseContext>(options =>
            options.UseCosmos(config["Database__AccountName"], config["Database__Key"], config["Database__DatabaseName"]));
    })
    .Build();

host.Run();
