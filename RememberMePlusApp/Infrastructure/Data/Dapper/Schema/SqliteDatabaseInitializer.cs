using Dapper;
using RememberMePlusApp.Infrastructure.Data.Abstractions;

namespace RememberMePlusApp.Infrastructure.Data.Dapper.Schema;

public sealed class SqliteDatabaseInitializer(IDbConnectionFactory connectionFactory) : IDatabaseInitializer
{
    private readonly IDbConnectionFactory _connectionFactory = connectionFactory;

    public async Task InitializeAsync(CancellationToken cancellationToken = default)
    {
        const string sql = """
            CREATE TABLE IF NOT EXISTS ReminderTasks (
                Id TEXT NOT NULL PRIMARY KEY,
                Title TEXT NOT NULL,
                DueAt TEXT NOT NULL,
                Priority INTEGER NOT NULL,
                IsActive INTEGER NOT NULL,
                ReactivatedAt TEXT NULL,
                TaskPostponeMinutes INTEGER NULL
            );

            CREATE INDEX IF NOT EXISTS IX_ReminderTasks_IsActive_DueAt
            ON ReminderTasks (IsActive, DueAt);
            """;

        using var connection = _connectionFactory.CreateConnection();
        var command = new CommandDefinition(sql, cancellationToken: cancellationToken);

        await connection.ExecuteAsync(command);
    }
}
