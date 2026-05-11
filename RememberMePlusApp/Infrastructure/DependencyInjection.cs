using Microsoft.Extensions.DependencyInjection;
using RememberMePlusApp.Infrastructure.Data;

namespace RememberMePlusApp.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddSingleton<IDbConnectionFactory, SqliteDbConnectionFactory>();
        services.AddSingleton<IUnitOfWorkFactory, UnitOfWorkFactory>();
        services.AddSingleton<IAppRepository, AppRepository>();
        services.AddSingleton<ITaskRepository, DapperReminderTaskRepository>();
        services.AddSingleton<IDatabaseInitializer, SqliteDatabaseInitializer>();
        services.AddSingleton<IDatabaseSchemaRepository, DatabaseSchemaRepository>();

        return services;
    }
}
