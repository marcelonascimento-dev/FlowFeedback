using CommunityToolkit.Maui;
using FlowFeedback.Device;
using FlowFeedback.Device.Services;
using FlowFeedback.Device.ViewModels;
using UraniumUI;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                fonts.AddMaterialSymbolsFonts(); // Ícones do Google
            });
        builder.Services.AddSingleton<DatabaseService>();
        builder.Services.AddTransient<MainViewModel>();
        builder.Services.AddSingleton<MainPage>();
        builder.Services.AddSingleton(new HttpClient { BaseAddress = new Uri("https://localhost:7274/api/") });

        return builder.Build();
    }
}