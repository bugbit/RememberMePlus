using Dapper;

namespace RememberMePlusApp.Infrastructure.Data;

public sealed class AppRepository : IAppRepository
{
    /// <summary>
    /// Obtiene el primer registro de la tabla App.
    /// </summary>
    public async Task<AppRecord?> GetFirstAsync(IUnitOfWork unitOfWork, CancellationToken cancellationToken = default)
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

        return await unitOfWork.Connection.QueryFirstOrDefaultAsync<AppRecord>(
            new CommandDefinition(query, transaction: unitOfWork.Transaction, cancellationToken: cancellationToken));
    }
}
