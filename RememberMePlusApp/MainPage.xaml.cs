using RememberMePlusApp.ViewModels;

namespace RememberMePlusApp
{
    public partial class MainPage : ContentPage
    {
        private readonly MainPageViewModel _viewModel;
        private readonly IDispatcherTimer _refreshTimer;
        private readonly IDispatcherTimer _pulseTimer;

        public MainPage(MainPageViewModel viewModel)
        {
            _viewModel = viewModel;
            InitializeComponent();
            BindingContext = _viewModel;

            _refreshTimer = Dispatcher.CreateTimer();
            _refreshTimer.Interval = TimeSpan.FromMinutes(1);
            _refreshTimer.Tick += async (_, _) => await _viewModel.LoadAsync();

            _pulseTimer = Dispatcher.CreateTimer();
            _pulseTimer.Interval = TimeSpan.FromSeconds(1);
            _pulseTimer.Tick += (_, _) => _viewModel.ToggleAttentionPulse();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            await _viewModel.LoadAsync();
            _refreshTimer.Start();
            _pulseTimer.Start();
        }

        protected override void OnDisappearing()
        {
            _refreshTimer.Stop();
            _pulseTimer.Stop();

            base.OnDisappearing();
        }
    }
}
