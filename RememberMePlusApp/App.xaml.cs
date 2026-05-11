using RememberMePlusApp.Application.Alarms;
using RememberMePlusApp.Infrastructure.Data;

namespace RememberMePlusApp
{
    // No se puede pone como clase base Application porque ya hay un espacio de nombre: RememberMePlusApp.Application
    // Para evitar conflictos de nombres, se ha cambiado la base de la clase a Microsoft.Maui.Controls.Application
    public partial class App : Microsoft.Maui.Controls.Application
    {
        private readonly IDatabaseInitializer _databaseInitializer;
        private readonly IAlarmStartupService _alarmStartupService;

        public App(
            IDatabaseInitializer databaseInitializer,
            IAlarmStartupService alarmStartupService)
        {
            _databaseInitializer = databaseInitializer;
            _alarmStartupService = alarmStartupService;
            InitializeComponent();
        }

        protected override async void OnStart()
        {
            base.OnStart();
            await _databaseInitializer.InitializeAsync();
            await _alarmStartupService.StartAsync();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new AppShell());
        }
    }
}