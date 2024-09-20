using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Configurations;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.OpenApi.Models;

namespace ChrisUsher.MoveMate.API.Config;

public class OpenApiConfigOptions : DefaultOpenApiConfigurationOptions
{
    public override OpenApiInfo Info { get; set; } = new()
    {
        Version = GetOpenApiDocVersion(),
        Title = "MoveMate API",
        Description = "API for managing House Moves",
    };

    public override OpenApiVersionType OpenApiVersion { get; set; } = GetOpenApiVersion();

    public override bool ForceHttp => FunctionEnvironment.IsDevelopment;

    public override bool ForceHttps => !FunctionEnvironment.IsDevelopment;
}