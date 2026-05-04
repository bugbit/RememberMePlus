using RememberMePlusApp.ViewModels;

namespace RememberMePlusApp
{
    public partial class AddTaskPage : ContentPage
    {
        public AddTaskPage(AddTaskPageViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }
}
