using Dapper;
using RememberMePlusApp.Infrastructure.Data.Abstractions;

namespace RememberMePlusApp.Infrastructure.Data.Dapper.Schema;

public sealed class SqliteDatabaseInitializer : IDatabaseInitializer
{
    private const string CreateReminderTasksTableSql = """
        CREATE TABLE IF NOT EXISTS ReminderTasks (
            Id TEXT NOT NULL PRIMARY KEY,
            Title TEXT NOT NULL,
            DueAt TEXT NOT NULL,
            Priority INTEGER NOT NULL,
            IsActive INTEGER NOT NULL,
            CompletedAt TEXT NULL,
            ReactivatedFrom TEXT NULL,
            CustomPostponeMinutes INTEGER NULL
        );
        """;

    private readonly IDbConnectionFactory _connectionFactory;

    public SqliteDatabaseInitializer(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task EnsureCreatedAsync(CancellationToken cancellationToken)
    {
        await using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken);
        await connection.ExecuteAsync(new CommandDefinition(CreateReminderTasksTableSql, cancellationToken: cancellationToken));
    }
}
