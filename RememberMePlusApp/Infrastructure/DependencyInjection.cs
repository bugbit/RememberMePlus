using Microsoft.Extensions.DependencyInjection;
using RememberMePlusApp.Domain.Tasks;
using RememberMePlusApp.Infrastructure.Data.Abstractions;
using RememberMePlusApp.Infrastructure.Data.Dapper;
using RememberMePlusApp.Infrastructure.Data.Dapper.Repositories;
using RememberMePlusApp.Infrastructure.Data.Dapper.Schema;
using RememberMePlusApp.Infrastructure.Data.Options;

namespace RememberMePlusApp.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddOptions<SqliteDataOptions>();
        services.AddSingleton<IDbConnectionFactory, SqliteDbConnectionFactory>();
        services.AddSingleton<IDatabaseInitializer, SqliteDatabaseInitializer>();
        services.AddScoped<IReminderTaskRepository, DapperReminderTaskRepository>();

        return services;
    }
}
