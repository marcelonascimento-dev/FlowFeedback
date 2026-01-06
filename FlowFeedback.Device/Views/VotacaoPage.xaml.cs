using FlowFeedback.Device.ViewModels;

namespace FlowFeedback.Device.Views;

public partial class VotacaoPage : ContentPage
{
    public VotacaoPage(VotacaoViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}