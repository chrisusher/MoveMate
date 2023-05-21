using ChrisUsher.MoveMate.API.Database;
using Microsoft.Azure.Functions.Worker.Extensions.OpenApi.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ChrisUsher.MoveMate.API.Services.StampDuty;
using System.Text.Json.Serialization;

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

        services.ConfigureHttpJsonOptions(options => 
        {
            options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
            options.SerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
            options.SerializerOptions.WriteIndented = true;
        });

        services.AddSingleton<StampDutyService>();
    })
    .Build();

host.Run();
