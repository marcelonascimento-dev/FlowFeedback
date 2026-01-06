using FlowFeedback.Device.ViewModels;
using System.ComponentModel;

namespace FlowFeedback.Device.Views;

public partial class VotacaoPopup : CommunityToolkit.Maui.Views.Popup
{
    public VotacaoPopup()
    {
        InitializeComponent();
    }

    protected override void OnBindingContextChanged()
    {
        base.OnBindingContextChanged();

        if (BindingContext is VotacaoPopupViewModel vm)
        {
            vm.PropertyChanged += OnViewModelPropertyChanged;
        }
    }

    private async void OnViewModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(VotacaoPopupViewModel.MostrarTags))
        {
            var vm = (VotacaoPopupViewModel)sender!;

            if (vm.MostrarTags)
            {
                await Task.WhenAll(
                    ViewTags.FadeToAsync(1, 400, Easing.CubicOut),
                    ViewTags.TranslateToAsync(0, 0, 400, Easing.CubicOut)
                );
            }

            if (e.PropertyName == nameof(VotacaoPopupViewModel.MostrarAgradecimento) && vm.MostrarAgradecimento)
            {
                ViewSucesso.Opacity = 0;
                ViewSucesso.Scale = 0.5;

                await ViewSucesso.FadeToAsync(1, 400);
                await ViewSucesso.ScaleToAsync(1, 500, Easing.SpringOut);
            }

        }
    }
}