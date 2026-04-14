using Dapper;

namespace RememberMePlusApp.Infrastructure.Data;

public sealed class AppRepository(IDbConnectionFactory dbConnectionFactory) : IAppRepository
{
    private readonly IDbConnectionFactory _dbConnectionFactory = dbConnectionFactory;

    /// <summary>
    /// Obtiene el primer registro de la tabla App.
    /// </summary>
    public async Task<AppRecord?> GetFirstAsync(CancellationToken cancellationToken = default)
    {
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

        await using var connection = _dbConnectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken);

        return await connection.QueryFirstOrDefaultAsync<AppRecord>(
            new CommandDefinition(query, cancellationToken: cancellationToken));
    }
}
