using RememberMePlusApp.Infrastructure.Data;

namespace RememberMePlusApp
{
    public partial class MainPage : ContentPage
    {
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;
        private readonly IAppRepository _appRepository;

        public MainPage(IUnitOfWorkFactory unitOfWorkFactory, IAppRepository appRepository)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
            _appRepository = appRepository;
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            await using var uow = await _unitOfWorkFactory.CreateAsync();
            var app = await _appRepository.GetFirstAsync(uow);

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
