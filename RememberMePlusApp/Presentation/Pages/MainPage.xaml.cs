using RememberMePlusApp.Presentation.ViewModels;

namespace RememberMePlusApp.Presentation.Pages;

public partial class MainPage : ContentPage
{
    public MainPage(HomeViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
