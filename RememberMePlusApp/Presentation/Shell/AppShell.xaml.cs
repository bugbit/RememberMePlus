using RememberMePlusApp.Presentation.Pages;

namespace RememberMePlusApp.Presentation.Shell;

public partial class AppShell : Microsoft.Maui.Controls.Shell
{
    public AppShell(MainPage mainPage)
    {
        InitializeComponent();

        Items.Add(new ShellContent
        {
            Title = "Home",
            Route = nameof(MainPage),
            Content = mainPage
        });
    }
}
