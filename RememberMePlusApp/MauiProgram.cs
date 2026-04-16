using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RememberMePlusApp.Infrastructure.Data;

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
            builder.Services.AddSingleton<IAppRepository, AppRepository>();
            builder.Services.AddSingleton<IDatabaseInitializer, DatabaseInitializer>();
            builder.Services.AddSingleton<IDatabaseSchemaRepository, DatabaseSchemaRepository>();

#if DEBUG
            builder.Logging.AddDebug();
            builder.Logging.SetMinimumLevel(LogLevel.Debug);
#endif

            var app = builder.Build();
            var databaseInitializer = app.Services.GetRequiredService<IDatabaseInitializer>();
            databaseInitializer.InitializeAsync().GetAwaiter().GetResult();

            return app;
        }
    }
}
