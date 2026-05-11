using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RememberMePlusApp.Infrastructure;
using RememberMePlusApp.ViewModels;

namespace RememberMePlusApp
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            builder.Services.AddInfrastructure();
            builder.Services.AddTransient<MainPageViewModel>();
            builder.Services.AddTransient<AddTaskPageViewModel>();
            builder.Services.AddTransient<MainPage>();
            builder.Services.AddTransient<AddTaskPage>();
            builder.Services.AddSingleton<App>();

#if DEBUG
            builder.Logging.AddDebug();
            builder.Logging.SetMinimumLevel(LogLevel.Debug);
#endif

            return builder.Build();
        }
    }
}
