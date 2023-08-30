using Microsoft.Azure.Functions.Worker.Extensions.OpenApi.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json.Serialization;
using ChrisUsher.MoveMate.API.Database;
using Azure.Core.Serialization;
using ChrisUsher.MoveMate.API.Services;

var config = new ConfigurationBuilder()
#if DEBUG
    .AddJsonFile("local.settings.json", false)
#endif
    .AddEnvironmentVariables()
    .Build();

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults(worker =>
    {
        var options = new JsonSerializerOptions
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            WriteIndented = true
        };
        options.Converters.Add(new JsonStringEnumConverter());

        worker.Serializer = new JsonObjectSerializer(options);
    })
    .ConfigureOpenApi()
    .ConfigureServices(services =>
    {
        services.AddDbContext<DatabaseContext>(options => 
        {
            options.UseCosmos(config["Database:AccountName"], config["Database:Key"], config["Database:DatabaseName"]);

            options.EnableSensitiveDataLogging();

            #if DEBUG
            
            options.EnableDetailedErrors();
            options.LogTo(Console.WriteLine);

            #endif
        });

        services.AddMoveMateServices();
    })
    .Build();

    var dbContext = host.Services.GetRequiredService<DatabaseContext>();
    await dbContext.Database.EnsureCreatedAsync();

host.Run();
