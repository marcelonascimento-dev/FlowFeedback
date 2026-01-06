using FlowFeedback.Device.ViewModels;

namespace FlowFeedback.Device.Views;

public partial class AgradecimentoPage : ContentPage
{
    public AgradecimentoPage(AgradecimentoViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}