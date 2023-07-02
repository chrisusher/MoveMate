using ChrisUsher.MoveMate.App.Config;
using Microsoft.Extensions.Configuration;
using RestSharp;

namespace ChrisUsher.MoveMate.App;

public class AppCommon
{
    public AppCommon(IConfiguration configuration)
    {
        Settings = configuration.GetSection("Settings")
            .Get<AppSettings>();
    }
    
    public RestClient ApiClient => new(options: new RestClientOptions(Settings.APIUrl));

    public AppSettings Settings { get; set; }
}