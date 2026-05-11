using RememberMePlusApp.Domain.App;

namespace RememberMePlusApp.Infrastructure.Data.Dapper.Repositories;

public sealed class DapperRepository : IAppRepository
{
    /// <summary>
    /// Obtiene el primer registro de la tabla App.
    /// </summary>
    public async Task<AppRecord?> GetFirstAsync(IUnitOfWork unitOfWork, CancellationToken cancellationToken = default)
    {
        if (unitOfWork is not ISqlExecutor sqlExecutor)
        {
            return null;
        }

        const string query = """
            SELECT
                id_app AS IdApp,
                version AS Version,
                relative_offset_minutes AS RelativeOffsetMinutes,
                snooze_minutes AS SnoozeMinutes
            FROM App
            ORDER BY id_app
            LIMIT 1;
            """;

        return await sqlExecutor.QueryFirstOrDefaultAsync<AppRecord>(query, cancellationToken: cancellationToken);
    }
}
