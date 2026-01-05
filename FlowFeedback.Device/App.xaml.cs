using Microsoft.Extensions.DependencyInjection;

namespace FlowFeedback.Device
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new AppShell());
        }

        public void AplicarCores(string primaria, string secundaria)
        {
            Current?.Resources["PrimaryColor"] = Color.FromArgb(primaria);
            Current?.Resources["SecondaryColor"] = Color.FromArgb(secundaria);
        }
    }
}