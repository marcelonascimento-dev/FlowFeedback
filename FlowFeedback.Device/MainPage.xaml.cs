using CommunityToolkit.Mvvm.Messaging;
using FlowFeedback.Device.ViewModels;

namespace FlowFeedback.Device;

public partial class MainPage : ContentPage
{
    public MainPage(MainViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;

        WeakReferenceMessenger.Default.Register<UpdateThemeMessage>(this, (r, m) =>
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                AplicarCores(m.Primaria, m.Secundaria);
            });
        });
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is MainViewModel vm)
        {
            await vm.CarregarConfiguracaoInicialCommand.ExecuteAsync(null);
        }
    }

    public void AplicarCores(string primaria, string secundaria)
    {
        var colorP = Color.FromArgb(primaria);
        var colorS = Color.FromArgb(secundaria);

        Application.Current.Resources["Primary"] = colorP;

        Application.Current.Resources["PrimaryDark"] = colorP.WithLuminosity(colorP.GetLuminosity() * 0.8f);

        Application.Current.Resources["Secondary"] = colorS;
    }
}