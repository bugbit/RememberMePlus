using Microsoft.Extensions.DependencyInjection;
using RememberMePlusApp.Application.Tasks;
using RememberMePlusApp.Infrastructure.Data.Abstractions;
using RememberMePlusApp.Infrastructure.Data.Dapper;
using RememberMePlusApp.Infrastructure.Data.Dapper.Repositories;
using RememberMePlusApp.Infrastructure.Data.Dapper.Schema;
using RememberMePlusApp.Infrastructure.Data.Options;

namespace RememberMePlusApp.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureData(
        this IServiceCollection services,
        string databasePath)
    {
        services.Configure<SqliteDataOptions>(options =>
        {
            options.DatabasePath = databasePath;
        });

        services.AddSingleton<IDbConnectionFactory, SqliteDbConnectionFactory>();
        services.AddSingleton<IDatabaseInitializer, SqliteDatabaseInitializer>();
        services.AddScoped<IReminderTaskRepository, DapperReminderTaskRepository>();

        return services;
    }
}
