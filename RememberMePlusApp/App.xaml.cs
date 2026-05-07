using RememberMePlusApp.Infrastructure.Data;

namespace RememberMePlusApp
{
    public partial class App : Microsoft.Maui.Controls.Application
    {
        private readonly IDatabaseInitializer _databaseInitializer;

        public App(IDatabaseInitializer databaseInitializer)
        {
            _databaseInitializer = databaseInitializer;
            InitializeComponent();
        }

        protected override async void OnStart()
        {
            base.OnStart();
            await _databaseInitializer.InitializeAsync();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new AppShell());
        }
    }
}