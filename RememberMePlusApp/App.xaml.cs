using RememberMePlusApp.Application.Alarms;
using RememberMePlusApp.Infrastructure.Data;

namespace RememberMePlusApp
{
    public partial class App : Application
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