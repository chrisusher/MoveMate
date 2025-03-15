using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Configurations;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;

namespace ChrisUsher.MoveMate.API;

public class OpenAPIDocumentation : IOpenApiConfigurationOptions
{
    public List<IDocumentFilter> DocumentFilters { get; set; }

    public bool ForceHttp { get; set; } = FunctionEnvironment.IsDevelopment;

    public bool ForceHttps { get; set; } = !FunctionEnvironment.IsDevelopment;

    public bool IncludeRequestingHostName { get; set; } = true;

    public OpenApiInfo Info { get; set; } = new()
    {
        Title = "MoveMate API",
        Description = $"API for managing House Moves{Environment.NewLine}",
        Contact = new OpenApiContact
        {
            Email = "chrisushertester@gmail.com",
            Name = "Enquiry"
        },
        Version = Environment.GetEnvironmentVariable("OpenApi__DocVersion") ?? "0.1.0"
    };

    public OpenApiVersionType OpenApiVersion { get; set; } = OpenApiVersionType.V3;

    public List<OpenApiServer> Servers { get; set; } = DefaultOpenApiConfigurationOptions.GetHostNames();
}