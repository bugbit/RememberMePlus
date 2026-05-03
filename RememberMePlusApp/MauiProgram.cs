using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RememberMePlusApp.Infrastructure.Data;
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

            builder.Services.AddSingleton<IDbConnectionFactory, DbConnectionFactory>();
            builder.Services.AddSingleton<IUnitOfWorkFactory, UnitOfWorkFactory>();
            builder.Services.AddSingleton<IAppRepository, AppRepository>();
            builder.Services.AddSingleton<ITaskRepository, TaskRepository>();
            builder.Services.AddTransient<MainPageViewModel>();
            builder.Services.AddTransient<AddTaskPageViewModel>();
            builder.Services.AddSingleton<IDatabaseInitializer, DatabaseInitializer>();
            builder.Services.AddSingleton<IDatabaseSchemaRepository, DatabaseSchemaRepository>();
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
