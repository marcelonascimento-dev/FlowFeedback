using CommunityToolkit.Mvvm.ComponentModel;

namespace FlowFeedback.Device.ViewModels;

public partial class AgradecimentoViewModel : ObservableObject
{
    public AgradecimentoViewModel()
    {
        IniciarRetorno();
    }

    private async void IniciarRetorno()
    {
        await Task.Delay(3000);

        await Shell.Current.GoToAsync("//MainPage");
    }
}