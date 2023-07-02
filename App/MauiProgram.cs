using System.Reflection;
using ChrisUsher.MoveMate.App.Clients;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ChrisUsher.MoveMate.App;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var assembly = Assembly.GetExecutingAssembly();
        using var jsonStream = assembly.GetManifestResourceStream("ChrisUsher.MoveMate.App.appsettings.json");

        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<global::App.App>()
            .ConfigureFonts(fonts => { fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular"); });

        builder.Services.AddMauiBlazorWebView();
        builder.Configuration.AddJsonStream(jsonStream);

#if DEBUG
        builder.Services.AddBlazorWebViewDeveloperTools();
        builder.Logging.AddDebug();
#endif

        builder.Services.AddSingleton<AppCommon>();
        builder.Services.AddSingleton<AccountClient>();
        builder.Services.AddSingleton<SavingsClient>();

        return builder.Build();
    }
}