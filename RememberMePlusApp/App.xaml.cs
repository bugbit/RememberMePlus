using RememberMePlusApp.Presentation.Shell;

namespace RememberMePlusApp;

// No se puede pone como clase base Application porque ya hay un espacio de nombre: RememberMePlusApp.Application
// Para evitar conflictos de nombres, se ha cambiado la base de la clase a Microsoft.Maui.Controls.Application
public partial class App : Microsoft.Maui.Controls.Application
{
    private readonly AppShell _appShell;

    public App(AppShell appShell)
    {
        InitializeComponent();
        _appShell = appShell;
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        return new Window(_appShell);
    }
}
