using App.Pages;
using App.ViewModels;
using Microsoft.Extensions.Configuration;
using System.Reflection;

namespace App;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
        var a = Assembly.GetExecutingAssembly();
        using var stream = a.GetManifestResourceStream("App.appsettings.json");
        var config = new ConfigurationBuilder()
            .AddJsonStream(stream)
            .Build();

        var builder = MauiApp.CreateBuilder();
        builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

        builder.Configuration.AddConfiguration(config);
        builder.Services.AddSingleton<IConnectivity>(Connectivity.Current);
        
        builder.Services.AddSingleton<MainViewModel>();
        builder.Services.AddSingleton<MainPage>();
        
        builder.Services.AddSingleton<HubViewModel>();
        builder.Services.AddSingleton<HubPage>();

        return builder.Build();
	}
}
