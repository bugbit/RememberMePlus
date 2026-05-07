using RememberMePlusApp.ViewModels;

namespace RememberMePlusApp
{
    public partial class MainPage : ContentPage
    {
        private readonly MainPageViewModel _viewModel;
        private CancellationTokenSource? _autoRefreshCancellationTokenSource;
        private CancellationTokenSource? _attentionPulseCancellationTokenSource;

        public MainPage(MainPageViewModel viewModel)
        {
            _viewModel = viewModel;
            InitializeComponent();
            BindingContext = _viewModel;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await _viewModel.LoadAsync();
            StartAttentionPulse();
            StartAutoRefresh();
        }

        protected override void OnDisappearing()
        {
            StopAutoRefresh();
            StopAttentionPulse();
            base.OnDisappearing();
        }

        private void StartAutoRefresh()
        {
            StopAutoRefresh();
            _autoRefreshCancellationTokenSource = new CancellationTokenSource();
            _ = RefreshPeriodicallyAsync(_autoRefreshCancellationTokenSource.Token);
        }

        private void StopAutoRefresh()
        {
            _autoRefreshCancellationTokenSource?.Cancel();
            _autoRefreshCancellationTokenSource?.Dispose();
            _autoRefreshCancellationTokenSource = null;
        }


        private void StartAttentionPulse()
        {
            StopAttentionPulse();

            if (!_viewModel.HasOverdueTasks)
            {
                return;
            }

            _attentionPulseCancellationTokenSource = new CancellationTokenSource();
            _ = PulseOverdueSectionAsync(_attentionPulseCancellationTokenSource.Token);
        }

        private void StopAttentionPulse()
        {
            _attentionPulseCancellationTokenSource?.Cancel();
            _attentionPulseCancellationTokenSource?.Dispose();
            _attentionPulseCancellationTokenSource = null;
            OverdueSectionBorder.Scale = 1;
        }

        private async Task PulseOverdueSectionAsync(CancellationToken cancellationToken)
        {
            try
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    await OverdueSectionBorder.ScaleTo(1.015, 450, Easing.CubicInOut);
                    await OverdueSectionBorder.ScaleTo(1, 450, Easing.CubicInOut);
                    await Task.Delay(TimeSpan.FromSeconds(2), cancellationToken);
                }
            }
            catch (OperationCanceledException)
            {
            }
        }

        private async Task RefreshPeriodicallyAsync(CancellationToken cancellationToken)
        {
            try
            {
                using var timer = new PeriodicTimer(TimeSpan.FromMinutes(1));

                while (await timer.WaitForNextTickAsync(cancellationToken))
                {
                    await MainThread.InvokeOnMainThreadAsync(async () =>
                    {
                        await _viewModel.LoadAsync(cancellationToken);
                        StartAttentionPulse();
                    });
                }
            }
            catch (OperationCanceledException)
            {
            }
        }
    }
}
