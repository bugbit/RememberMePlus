using Microsoft.Extensions.DependencyInjection;
using RememberMePlusApp.Application.Alarms;
using RememberMePlusApp.Infrastructure.Alarms;
using RememberMePlusApp.Infrastructure.Data;
using RememberMePlusApp.Infrastructure.Data.Dapper.Repositories;

namespace RememberMePlusApp.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddSingleton<IDbConnectionFactory, SqliteDbConnectionFactory>();
        services.AddSingleton<IUnitOfWorkFactory, UnitOfWorkFactory>();
        services.AddSingleton<IAppRepository, DapperRepository>();
        services.AddSingleton<ITaskRepository, DapperReminderTaskRepository>();
        services.AddSingleton<IDatabaseInitializer, SqliteDatabaseInitializer>();
        services.AddSingleton<IDatabaseSchemaRepository, DapperDatabaseSchemaRepository>();
        services.AddSingleton<IAlarmStartupService, AlarmStartupService>();
        services.AddSingleton<IAlarmTriggerHandler, AlarmTriggerHandler>();
        services.AddSingleton<IAlarmActionService, AlarmActionService>();
#if ANDROID
        services.AddSingleton<IAlarmScheduler, Platforms.Android.Alarms.AndroidAlarmScheduler>();
        services.AddSingleton<IAlarmNotificationService, Platforms.Android.Alarms.AndroidAlarmNotificationService>();
#else
        services.AddSingleton<IAlarmScheduler, TimerAlarmScheduler>();
        services.AddSingleton<IAlarmNotificationService, MauiAlarmNotificationService>();
#endif

        return services;
    }
}
