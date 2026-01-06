namespace FlowFeedback.Device
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(Views.VotacaoPage), typeof(Views.VotacaoPage));
            Routing.RegisterRoute(nameof(Views.AgradecimentoPage), typeof(Views.AgradecimentoPage));
        }
    }
}
