using CommunityToolkit.Maui;
using FlowFeedback.Device;
using FlowFeedback.Device.Services;
using FlowFeedback.Device.ViewModels;
using FlowFeedback.Device.Views;
using UraniumUI;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();

        AppDomain.CurrentDomain.UnhandledException += (s, e) => { /* Silêncio */ };

        TaskScheduler.UnobservedTaskException += (s, e) =>
        {
            e.SetObserved(); // Impede o fechamento por erro em Tasks assíncronas
        };

        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                fonts.AddMaterialSymbolsFonts(); // Ícones do Google
            });

#if ANDROID
        builder.Services.AddSingleton<FlowFeedback.Device.Interfaces.IDeviceIdentifier, FlowFeedback.Device.Platforms.Android.AndroidDeviceIdentifier>();
#else
    // Fallback para Windows/Outros (opcional)
    builder.Services.AddSingleton<FlowFeedback.Device.Interfaces.IDeviceIdentifier, DefaultDeviceIdentifier>();
#endif
        builder.Services.AddSingleton<ConfigurationService>();
        builder.Services.AddSingleton<DatabaseService>();
        builder.Services.AddTransient<MainViewModel>();
        builder.Services.AddSingleton<MainPage>();
        builder.Services.AddTransient<VotacaoPage>();
        builder.Services.AddTransient<VotacaoViewModel>();
        builder.Services.AddTransient<AgradecimentoPage>();
        builder.Services.AddTransient<AgradecimentoViewModel>();
        builder.Services.AddSingleton(new HttpClient { BaseAddress = new Uri(AppConfig.BaseApiUrl) });

        return builder.Build();
    }
}