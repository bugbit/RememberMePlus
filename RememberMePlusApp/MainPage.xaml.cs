using RememberMePlusApp.Infrastructure.Data;

namespace RememberMePlusApp
{
    public partial class MainPage : ContentPage
    {
        private readonly IAppRepository _appRepository;

        public MainPage(IAppRepository appRepository)
        {
            _appRepository = appRepository;
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            var app = await _appRepository.GetFirstAsync();

            if (app is not null)
            {
                LabelIdApp.Text = app.IdApp.ToString();
                LabelVersion.Text = app.Version.ToString();
                LabelRelativeOffset.Text = app.RelativeOffsetMinutes.ToString();
                LabelSnooze.Text = app.SnoozeMinutes.ToString();
            }
        }
    }
}
