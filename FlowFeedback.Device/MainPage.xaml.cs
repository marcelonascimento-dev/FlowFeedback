using FlowFeedback.Device.ViewModels;

namespace FlowFeedback.Device;

public partial class MainPage : ContentPage
{
    public MainPage(MainViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is MainViewModel vm)
        {
            // Executa o comando de carga
            await vm.CarregarConfiguracaoInicialCommand.ExecuteAsync(null);

            // Verificação simples no Console de Saída
            System.Diagnostics.Debug.WriteLine($"[DEBUG] Quantidade de Alvos: {vm.Alvos.Count}");

            if (vm.Alvos.Count == 0)
            {
                System.Diagnostics.Debug.WriteLine("[AVISO] A lista de alvos está vazia após o carregamento.");
            }
        }
    }
}